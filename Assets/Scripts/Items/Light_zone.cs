using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Light_zone : MonoBehaviour
{
    private bool DentroArea;
    public AudioClip lightSound;
    private HUD_Controller hud;

    private void Start()
    {
        hud = GameObject.FindGameObjectWithTag("UI").gameObject.GetComponent<HUD_Controller>();
    }

    private void Update()
    {
        hud.isHealing = DentroArea;
    }
    
    /// <summary>
    /// Comprueba que el jugador  a entrado en  la zona de luz y si esta dentro  se  habilita DentroArea
    /// </summary>
    /// <param name="collision"></param>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            SoundManager.SharedInstance.PlaySound(lightSound);
            DentroArea = true;
        }
    }
    
    /// <summary>
    /// Comprueba que el jugador se a salido de la zona de luz y su es asi desabilita DentroArea
    /// </summary>
    /// <param name="collision"></param>
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            DentroArea = false;
        }
    }
}