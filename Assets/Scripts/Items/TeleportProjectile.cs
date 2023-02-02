using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TeleportProjectile : MonoBehaviour
{
    [SerializeField]
    private string playerTag;
    [SerializeField]
    private string groundTag;
    [SerializeField]
    private string enemyTag;
    private GameObject player;
    float playerYSizeOffset;

    // Start is called before the first frame update
    void Awake()
    {
        player = GameObject.FindGameObjectWithTag(playerTag);
        playerYSizeOffset = (player.transform.localScale.y / 2) - 0.15f;

        Physics2D.IgnoreCollision(player.GetComponent<Collider2D>(), gameObject.GetComponent<Collider2D>());
        SetIgnoreTag("Vines");
    }
    /// <summary>
    /// Hace que el collider de este objeto ignore colisiones con todos los objetos de la escena con el tag <paramref name="tagToIgnore"/>
    /// </summary>
    /// <param name="tagToIgnore">el tag que llevan los colliders a evitar</param>
    private void SetIgnoreTag(string tagToIgnore)
    {
        GameObject[] vines = GameObject.FindGameObjectsWithTag(tagToIgnore);
        foreach (GameObject vine in vines)
        {
            Physics2D.IgnoreCollision(vine.GetComponent<TilemapCollider2D>(), this.gameObject.GetComponent<Collider2D>());
            Physics2D.IgnoreCollision(vine.GetComponent<CompositeCollider2D>(), this.gameObject.GetComponent<Collider2D>());
        }
    }
    private void FixedUpdate()
    {
        PredictCollisionPoint(gameObject.transform.position, 0.2f);
    }
    /// <summary>
    /// Reconoce las colisiones con las que puede impactar el objeto en la posicion indicada en un radio determinado
    /// </summary>
    /// <remarks>
    /// Utilizo este metodo en lugar del on collision porque me permite comprobar los datos de la colision antes de que ocurra.
    /// De lo contrario la colision podria o no haber cambiado la trayectoria del proyectil y el calculo del angulo y la direccion no funcionarian
    /// </remarks>
    /// <param name="position">la posicion actual del objeto</param>
    /// <param name="stepSize">el radio de deteccion de colisiones</param>


    private void PredictCollisionPoint(Vector3 position, float stepSize)
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(position, stepSize);
        if (hits.Length > 1)
        {
            bool canTp = true;
            canTp = !SurroundedByVines(position);
            foreach (Collider2D hit in hits)
            {
                if (hit != null)
                {
                    Vector3 collisionPoint = hit.ClosestPoint(position);
                    Vector3 collisionVector = transform.position - collisionPoint;
                    int angle = (int)Mathf.Abs(Vector3.Angle(position - collisionPoint, Vector2.right));

                    bool horizontalCollision = (angle == 90 || angle == 89) && collisionVector.magnitude > 0.0001;
                    if (hit.CompareTag("Vines"))
                    {
                        Physics2D.IgnoreCollision(gameObject.GetComponent<Collider2D>(), hit);
                    }
                    if (hit.CompareTag(groundTag) && canTp)
                    {
                        if (horizontalCollision && gameObject.GetComponent<Rigidbody2D>().velocity.y < 0)
                        {
                            player.GetComponent<PlayerController>().TeleportTo(new Vector2(transform.position.x, transform.position.y + playerYSizeOffset));
                        }
                        Destroy(gameObject);
                    }
                    else if (!hit.CompareTag(playerTag) && !hit.CompareTag("Untagged") && !hit.CompareTag("Vines"))
                    {
                        Destroy(gameObject);
                    }
                }
            }
        }
    }

    private bool SurroundedByVines(Vector3 position)
    {
        bool rightHitTrue = false;
        bool leftHitTrue = false;
        RaycastHit2D[] rightHit = Physics2D.RaycastAll(position, Vector2.right, 2);
        RaycastHit2D[] leftHit = Physics2D.RaycastAll(position, Vector2.left, 2);
        foreach (RaycastHit2D dhit in leftHit)
        {
            if (dhit.collider.CompareTag("Vines"))
            {
                leftHitTrue = true;
            }
        }
        foreach (RaycastHit2D dhit in rightHit)
        {
            if (dhit.collider.CompareTag("Vines"))
            {
                rightHitTrue = true;
            }
        }
        return (leftHitTrue && rightHitTrue);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, (transform.position + Vector3.right * 1.5f));
        Gizmos.DrawLine(transform.position, (transform.position + Vector3.left * 1.5f));

    }

}