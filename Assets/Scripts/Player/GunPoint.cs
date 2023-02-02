using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunPoint : MonoBehaviour
{
    #region referencias de escena
    
    private Camera mainCamera;
    [SerializeField]
    private Transform gunHolder;
    [SerializeField]
    private Transform gunPivot;
    #endregion
    #region input disparo
    [HideInInspector]
    public bool ableToShoot;
    private float projectileOneAxisVal;//Valor del trigger o click del raton asociado al primer proyectil
    private float projectileTwoAxisVal;//Valor del trigger o click del raton asociado al segundo proyectil
    #endregion
    
    #region variables prefabs proyectiles
    [SerializeField]
    private GameObject[] availablePrefabs;
    private int selectedPrefabIndex;

    public int availableSeeds;

    private GameObject activeProjectile;

    private bool chargingProjectile;

    private LineRenderer lineRenderer;

    [SerializeField]
    List<string> tagsToIgnore;

    [SerializeField]
    private float launchForce; 
    private Vector3 distanceVector; //vector de la distancia entre el jugador y la posicion del raton
    #endregion
    private Animator slingAnimator;
    public GameObject slingshot;
    #region eventos de Unity
    private void Awake()
    {
        slingshot = transform.GetChild(0).gameObject;
        lineRenderer = slingshot.GetComponent<LineRenderer>();
        slingAnimator = slingshot.GetComponent<Animator>();
        mainCamera = Camera.allCameras[0];
        ableToShoot = true;
    }
    private void Update()
    {
        Vector2 mousePosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);

        projectileOneAxisVal = Input.GetAxisRaw("Fire1");
        projectileTwoAxisVal = Input.GetAxisRaw("Fire2");
        RotateGun(mousePosition);
    }
    private void FixedUpdate()
    {
        if (!slingshot.GetComponent<SlingshotScript>().touchingGrass)
        {
            if (projectileOneAxisVal > 0 && activeProjectile == null && availableSeeds > 0 && ableToShoot)
            {
                ChargeProjectile(0);
            }
            else if (projectileTwoAxisVal > 0 && activeProjectile == null && ableToShoot)
            {
                ChargeProjectile(1);
            }
            else if ((projectileOneAxisVal <= 0 || projectileTwoAxisVal <= 0) && chargingProjectile && ableToShoot)
            {
                LaunchProjectile();
            }
        }
        lineRenderer.enabled = chargingProjectile;
    }


    #endregion
    #region Metodos de lanzamiento
    /// <summary>
    /// Rota el arma/canon/tirachinas para que la punta mire al puntero del raton
    /// </summary>
    /// <param name="targetPoint">el vector posicion del raton</param>
    private void RotateGun(Vector3 targetPoint)
    {
        //if (mainCamera == null)
        //{
        //    mainCamera = Camera.current;
        //}
        distanceVector = targetPoint - gunPivot.position;
        float angleDeg = Mathf.Atan2(distanceVector.y, distanceVector.x) * Mathf.Rad2Deg;
        //gunHolder.GetComponent<SpriteRenderer>().flipX = angleDeg < 90f;
        slingshot.GetComponent<SpriteRenderer>().flipY = Mathf.Abs(angleDeg) >= 90 ;
        gunPivot.rotation = Quaternion.AngleAxis(angleDeg, Vector3.forward);
    }
    /// <summary>
    /// Instancia un proyectiil desde la posicion del eje de rotacion del lanzador y lo dispara aplicandole una fuerza igual a <see cref="launchForce"/>
    /// </summary>
    private void LaunchProjectile()
    {
        slingAnimator.SetBool("Charging", false);
        activeProjectile = Instantiate(availablePrefabs[selectedPrefabIndex], slingshot.transform.position, Quaternion.identity);
        if (selectedPrefabIndex == 0)
        {
            availableSeeds--;
        }
        Vector2 direction = distanceVector.normalized;
        activeProjectile.GetComponent<Rigidbody2D>().AddForce(direction * launchForce);
        chargingProjectile = false;
    }
    #endregion
    #region Dibujar trayectoria
    /// <summary>
    /// Simula la trayectoria del proyectil mostrando la trayectoria que va a seguir con el line renderer
    /// </summary>
    private void DrawProjectileTrajectory()
    {
        float simulationDuration = 3f;
        float stepInterval = 0.015f;
        int steps = (int)(simulationDuration / stepInterval);
        lineRenderer.positionCount = steps;
        float projectileMass = availablePrefabs[selectedPrefabIndex].gameObject.GetComponent<Rigidbody2D>().mass;
        float velocity = (launchForce / projectileMass) * Time.fixedDeltaTime;
        Vector3 nextposition;
        for (int i = 0; i < steps; i++)
        {
            nextposition = slingshot.transform.position + distanceVector.normalized * velocity * stepInterval * i;
            float gravity = Physics2D.gravity.y / 2 * Mathf.Pow(i * stepInterval, 2);
            nextposition.y += gravity;
            lineRenderer.SetPosition(i, nextposition);
            if (LineCollided(nextposition, stepInterval))
            {
                steps = i;
                lineRenderer.positionCount = steps;
            }
        }
    }
    /// <summary>
    /// Metodo que calcula si en el siguiente paso de la linea de trayectoria colisionaria ignorando las <see cref="tagsToIgnore"/>
    /// </summary>
    /// <param name="position">la posicion del siguiente punto</param>
    /// <param name="stepSize">el radio del area que detecta colisiones</param>
    /// <returns>true si la linea tocaria un collider en el siguiente punto de la trayectoria</returns>
    private bool LineCollided(Vector3 position, float stepSize)
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(position, stepSize);
        int numberOfHits = hits.Length;
        foreach (Collider2D hit in hits)
        {
            if (tagsToIgnore.Contains(hit.tag))
            {
                Physics2D.IgnoreCollision(hit, availablePrefabs[selectedPrefabIndex].GetComponent<Collider2D>());
                numberOfHits--;
            }
        }
        Physics2D.queriesHitTriggers = false;
        return numberOfHits > 0;
    }
    #endregion
    #region seleccionar proyectil
    /// <summary>
    /// Cambia el proyectil seleccionado en funcion del indice indicado, lo carga y dibuja la trayectoria que va a seguir
    /// </summary>
    /// <param name="index"></param>
    private void ChargeProjectile(int index)
    {
        if (index < availablePrefabs.Length)
        {
            slingAnimator.SetBool("Charging", true);
            selectedPrefabIndex = index;
            chargingProjectile = true;
            DrawProjectileTrajectory();
        }
    }
    #endregion
}
