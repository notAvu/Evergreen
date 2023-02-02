using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class water_drop: MonoBehaviour
{
    public AudioClip waterSound;
    public float health;
    
    /// <summary>
    /// Al entrar la gota en colision con el jugador se le agrega al jugador vida y la gota se destruye 
    /// </summary>
    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.CompareTag("Player"))
        {
            collider.gameObject.GetComponent<PlayerController>().AnyadirVida(health);
            SoundManager.SharedInstance.PlaySound(waterSound);
            Destroy(gameObject);
        }
    }


}
