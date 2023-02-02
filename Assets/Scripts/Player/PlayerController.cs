using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{

    #region Atributos
    public float velocidadInicial, saltoInicial;
    private float velocidad, fuerzaSalto;
    private Rigidbody2D rigid;
    private SpriteRenderer spriteRenderer;
    public LayerMask capaSuelo;
    private Animator animador;
    public string tagHiedra;
    public string tagEnemigo;
    public string tagSalida;
    private MenuFinal menuFinal;
    private HUD_Controller hood;
    private GameController gameController;
    public GameObject gun;
    public AudioClip jumpSound, enemySound, exitSound;

    private bool ableToMove;
    #endregion

    #region Contructores

    private void Awake()
    {
        ableToMove = true;
        gameController = GameObject.Find("GameController").GetComponent<GameController>();
        hood = GameObject.FindGameObjectWithTag("UI").GetComponent<HUD_Controller>();
        menuFinal = GameObject.Find("EndLevel").GetComponent<MenuFinal>();
        velocidad = velocidadInicial;
        fuerzaSalto = saltoInicial;
        rigid = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animador = GetComponent<Animator>();
    }

    #endregion

    #region Metodos publicos

    public void AnyadirVida(float vida)
    {
        hood.AddTime(vida);
    }

    #endregion

    #region Metodos privados

    private void Update()
    {
        if (ableToMove)
        {
            ProcesarMovimiento();
        }
        else
        {
            rigid.velocity =new Vector2(0,rigid.velocity.y);
        }
        ProcesarSalto();
    }

    /// <summary>
    /// Procesa el salto, si el jugador ha pulsado la tecla Espacio se aplicara la fuerzaSalto con la direccion Vector2.up
    /// </summary>
    private void ProcesarSalto()
    {
        if (Input.GetKeyDown(KeyCode.Space) && EstaEnSuelo())
        {
            SoundManager.SharedInstance.PlaySound(jumpSound);
            rigid.AddForce(Vector2.up * fuerzaSalto, ForceMode2D.Impulse);
        }
        animador.SetBool("isJumped", !EstaEnSuelo());
    }

    /// <summary>
    /// Procesa el movimiento, si el jugador no esta en el suelo la variable restaVelocidad sera mayor que 0, por lo que la velocidad horizontal disminuye
    /// </summary>
    private void ProcesarMovimiento()
    {
        float movHorizontal = Input.GetAxis("Horizontal");
        float restaVelocidad = 0;
        if (!EstaEnSuelo())
        {
            restaVelocidad = velocidad * 0.32f;
        }
        rigid.velocity = new Vector2(movHorizontal * (velocidad - restaVelocidad), rigid.velocity.y);
        animador.SetFloat("velocidad", Mathf.Abs(rigid.velocity.x));
        ProcesaFlip(movHorizontal);
    }

    /// <summary>
    /// Procesa el flip, si el movimiento es mayor que 0 el sentido es derecha, si es menor, es izquierda
    /// </summary>
    /// <param name="movHorizontal"></param>
    private void ProcesaFlip(float movHorizontal)
    {
        if (movHorizontal > 0)
        {
            spriteRenderer.flipX = true;
        }
        else if (movHorizontal < 0)
        {
            spriteRenderer.flipX = false;
        }
    }

    /// <summary>
    /// Se crea un raycast, si los pies del jugador estan tocando el suelo devolver� true, de lo contrario devolvera false
    /// </summary>
    /// <returns>Bool</returns>
    private bool EstaEnSuelo()
    {
        RaycastHit2D raycastHit = Physics2D.BoxCast(new Vector2(transform.position.x, transform.position.y - transform.localScale.y / 2),
            new Vector3(0.6f, 0.6f, 1), 0f, Vector2.down, 0.2f, capaSuelo);
        return raycastHit.collider != null;
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawCube(new Vector2(transform.position.x, transform.position.y - transform.localScale.y / 2), new Vector3(0.6f, 0.6f, 1));
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag(tagEnemigo))
        {
            SoundManager.SharedInstance.PlaySound(enemySound);
            gameController.haPerdido = true;
        }
        else if (collision.gameObject.CompareTag(tagHiedra))
        {
            DañoHiedra();
        }
        else if (!collision.gameObject.CompareTag(tagHiedra))
        {
            //Si no toca la hiedra vuelve a velocidad inicial
            velocidad = velocidadInicial;
        }
        if (collision.gameObject.CompareTag(tagSalida))
        {
            SoundManager.SharedInstance.PlaySound(exitSound);
            gameController.haGanado = true;
            menuFinal.Salida();
        }
    }

    private void DañoHiedra()
    {
        //se revisa si está tocando la hiedra para reducir la velocidad e ir disminuyendo la vida actual (Pendiente de valores)
        velocidad = velocidadInicial/2f;
        Debug.Log("a");
        //VidaActual = VidaActual - 1f;
    }

    public void TeleportTo(Vector2 position)
    {
        GunPoint gunPoint = gun.GetComponent<GunPoint>();
        animador.SetTrigger("Die");
        gunPoint.slingshot.GetComponent<SpriteRenderer>().enabled = false;
        animador.SetBool("spawn", true);
        StartCoroutine(AnimateTp(position));
        ableToMove = false;
        AnimateSpawn();
    }
    private IEnumerator AnimateTp(Vector2 TpPosition)
    {
        gun.GetComponent<GunPoint>().ableToShoot = false;
        yield return new WaitForSeconds(animador.runtimeAnimatorController.animationClips[3].length);
        transform.position = TpPosition;
        rigid.velocity = Vector2.zero;
        StartCoroutine(AnimateSpawn());
    }
    private IEnumerator AnimateSpawn()
    {
        yield return new WaitForSeconds(animador.runtimeAnimatorController.animationClips[4].length);
        ableToMove = true;
        gun.GetComponent<GunPoint>().ableToShoot = true;
        animador.SetBool("spawn", false);
        gun.GetComponent<GunPoint>().slingshot.GetComponent<SpriteRenderer>().enabled = true;
    }
    #endregion
}
