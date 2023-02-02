using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial : MonoBehaviour
{
    public float tiempo;
    private Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        animator=GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    /// <summary>
    /// voy restando el tiempo que le e asignado para que muestre el tutorial 
    /// </summary>
    private void FixedUpdate()
    {
        tiempo -= Time.deltaTime;
        Debug.Log(tiempo);
        ///Cuando se acaba se ejecuta una animacion para que no desaparezca de golpe y lo destruyo
        if (tiempo <= 0)
        {
            animator.SetTrigger("Destruir");
           
            Destroy(gameObject, 1);
        }
    }
}
