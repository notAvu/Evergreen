using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialController : MonoBehaviour
{

    private GameObject[] bocadillo;
    private float tiempoDialogoConfirmacion;
    private int indicetutorial;
    private bool haPulsadoA, haPulsadoD;
    float tiempoEsperaDialogo;

    // Start is called before the first frame update
    void Start()
    {
        indicetutorial = 1;
        tiempoDialogoConfirmacion = 1.25f;
        bocadillo = new GameObject[6];
        bocadillo[0] = transform.GetChild(0).gameObject;
        bocadillo[1] = transform.GetChild(1).gameObject;
        bocadillo[2] = transform.GetChild(2).gameObject;
        bocadillo[3] = transform.GetChild(3).gameObject;
        bocadillo[4] = transform.GetChild(4).gameObject;
        bocadillo[5] = transform.GetChild(5).gameObject;
        tiempoEsperaDialogo = 0.75f;
    }

    // Update is called once per frame
    void Update()
    {
        controllarTutorial();
    }

    /// <summary>
    /// 
    /// Se encarga de controlar los botones pulsados apra avanzar con el texto del tu
    /// 
    /// </summary>
    private void controllarTutorial()
    {
        switch (indicetutorial)
        {
            case 1:
                if (Input.GetKeyDown(KeyCode.A))
                    haPulsadoA = true;
                else if (Input.GetKeyDown(KeyCode.D))
                    haPulsadoD = true;

                if(haPulsadoA && haPulsadoD)
                {
                    StartCoroutine(indicadarBienHecho());
                    StartCoroutine(activarDialogoConfirmacion(2));

                    indicetutorial = 2 > indicetutorial ? 2 : indicetutorial; // si 2 > indiceTutorial setea indiceTutorial a 2, de lo contrario solo devolvemos indiceTutorial. 
                }
                break;

            case 2:
                if(Input.GetKeyDown(KeyCode.Space)){
                    StartCoroutine(indicadarBienHecho());
                    StartCoroutine(activarDialogoConfirmacion(3));

                    indicetutorial = 3 > indicetutorial ? 3 : indicetutorial;
                }
                break;
            case 3:
                if (Input.GetMouseButtonUp(0)) // Click izquierdo raton (al soltar)
                {
                    StartCoroutine(indicadarBienHecho());
                    StartCoroutine(activarDialogoConfirmacion(4));

                    indicetutorial = 4 > indicetutorial ? 4 : indicetutorial;
                }
                break;
            case 4:
                if (Input.GetMouseButtonUp(1)) // Click derecho raton (al soltar)
                {
                    StartCoroutine(indicadarBienHecho());
                    StartCoroutine(activarDialogoConfirmacion(5));
                    
                    indicetutorial = 5 > indicetutorial ? 5 : indicetutorial;
                }
                break;
        }
    }
    /// <summary>
    /// 
    /// Activa el dialogo pasado por parametro
    /// 
    /// </summary>
   private void activarDialogoTutorial(int indice)
    {
        desactivarTodosDialogoTutorial();
        bocadillo[indice].SetActive(true);
    }

    /// <summary>
    /// 
    /// Desactiva todos los dialogos
    /// 
    /// </summary>
    private void desactivarTodosDialogoTutorial()
    {
        for (int i = 0; i < 6; i++)
        {
            bocadillo[i].SetActive(false);
        }

    }

    /// <summary>
    /// 
    /// Se encarga de mostrar el texto de confirmacion durante 
    /// 
    /// </summary>
    /// <param name="particulas"></param>
    /// <returns></returns>
    IEnumerator finalizarDialogo()
    {
        yield return new WaitForSeconds(3.0f);
        transform.parent.gameObject.SetActive(false);
    }
    /// <summary>
    /// 
    /// Se encarga del retardo entre el tutorial actual y confirmacion
    /// 
    /// </summary>
    /// <returns></returns>
    IEnumerator indicadarBienHecho()
    {

        yield return new WaitForSeconds(tiempoEsperaDialogo);
        activarDialogoTutorial(0);
    }




    /// <summary>
    /// 
    /// Se encarga de mostrar el texto de confirmacion durante 
    /// 
    /// </summary>
    /// <param name="particulas"></param>
    /// <returns></returns>
    IEnumerator activarDialogoConfirmacion(int indice)
    {

        StartCoroutine(indicadarBienHecho());
        yield return new WaitForSeconds(tiempoEsperaDialogo + tiempoDialogoConfirmacion);
        activarDialogoTutorial(indice);
        if (indice == 5)
        {
            StartCoroutine(finalizarDialogo());
        }
    }
}
