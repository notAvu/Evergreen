using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;

public class FloatingPlatform: MonoBehaviour
{
    private Vector3 PosicionOriginal;
    private Rigidbody2D rb;
    private BoxCollider2D BoxC2D;
    public float DuracionPlataforma;
    private float ContadorPlatforma;
    private bool SobrePlataforma;
    public  float TiempoDestruir;
    private GameController gameControler;

    // Start is called before the first frame update
    void Start()
    {
        //Obtengo del objeto su Rigidbody2D , BoxCollider2D y  guardo la posicion de intanciado 
        PosicionOriginal = gameObject.GetComponent<Transform>().position;
        rb = GetComponent<Rigidbody2D>();
        BoxC2D=GetComponent<BoxCollider2D>();
        SobrePlataforma = false;
        gameControler = GameObject.Find("GameController").GetComponent<GameController>();
        ContadorPlatforma = 0;
    }

    // Update is called once per frame
    private void FixedUpdate()
    {///Cuando SobrePlataforma es verdadero va restando tiempo  a la duracion de la plataforma
        if (SobrePlataforma)
        {
            ContadorPlatforma += Time.deltaTime;
            if (ContadorPlatforma >= DuracionPlataforma)
            {
                Activarcaida();
                BoxC2D.isTrigger = true;
            }

        }

    }
    private void OnCollisionStay2D(Collision2D collision)
    {
        /// Al colisionar con el jugador activa un bool y activa un temporizador cuando llega a cero  
        /// cambia el  tipo de cuerpo de la plataforma  a Dynamic aumento su gravedad 
        /// y le digo que es un trigger para que atraviese el suelo y lo destruyo a los 5s
        if (collision.gameObject.CompareTag("Player"))
        {
            SobrePlataforma = true;
            
        }
       
    }
  
 

    public void Activarcaida() {

        rb.bodyType = RigidbodyType2D.Dynamic;
        rb.gravityScale = 2;
    }
    public void Desactivarcaida()
    {
        rb.bodyType = RigidbodyType2D.Static;
        rb.gravityScale = 0;
        BoxC2D.isTrigger = false;
        ContadorPlatforma=0;
        SobrePlataforma = false;

    }
    private void OnBecameInvisible()
    {
        Invoke("reponerPlataforma", TiempoDestruir);
    }
    public void reponerPlataforma() {
        Desactivarcaida();
        transform.position = PosicionOriginal;
    }

}
