using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cobj1 : MonoBehaviour
{
    public GameObject controles;
    
    // Start is called before the first frame update
 
 
    private void OnTriggerEnter2D(Collider2D collision)
        ///Cuando el player pasa por el colider activo el cambas con los controles
    {
        if (collision.gameObject.CompareTag("Player"))
        {


            controles.SetActive(true);


        }
    }
}
