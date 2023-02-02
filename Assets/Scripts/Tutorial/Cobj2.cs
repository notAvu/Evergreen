using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cobj2 : MonoBehaviour
{
    
    private Animator animator;
    public GameObject controles;
    // Start is called before the first frame update
    void Start()
    {
        animator=GetComponent<Animator>();
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        ///Cuando el player pasa por el colaider activo una animacion y destruyo el canvas
        if (collision.gameObject.CompareTag("Player"))
        {
            
            controles.GetComponent<Animator>().SetTrigger("Destruir");

            Destroy(controles,1);


        }
    }
}
