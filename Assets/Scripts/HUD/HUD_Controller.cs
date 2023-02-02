using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;

public class HUD_Controller : MonoBehaviour
{
    #region Attributes
    [SerializeField] private GameObject timeBar, seed1, seed2;
    [SerializeField] private TextMeshProUGUI textTime, textContadorSeed1, textContadorSeed2;
    [SerializeField] private Sprite defaultImage, usedImage;
    private float time;
    private float health;
    public GunPoint playerController;
    public GameController gameController;
    public bool isHealing;
    public float HealthLoss;
    #endregion

    #region Unity methods

    void Update()
    {
        //actualizar texto tiempo como contador
        textTime.text = SetTime();
        //actualizar barra tiempo
        if(Time.timeScale == 1)
        {
            StartCoroutine(SetSmoothTimeBar());
        }
        //actualizar semillas
        SetPlayerSeeds();
    }
    #endregion

    #region Methods
    /// <summary>
    ///     <b>Cabecera: </b>private string SetTime
    ///     <b>Descripci�n: </b> Cambia el texto con el tiempo transcurrido en la partida
    /// </summary>
    /// <returns> Cadena con el tiempo en minutos y segundos</returns>
    private string SetTime()
    {
        //A�ado el intervalo transcurrido a la variable
        time += Time.deltaTime;
        health += Time.deltaTime;

        //Formateo minutos y segundos a dos d�gitos
        string minutos = Mathf.Floor(time / 60).ToString("00");
        string segundos = Mathf.Floor(time % 60).ToString("00");

        //Devuelvo el string formateado con : como separador
        return minutos + ":" + segundos;
    }

    /// <summary>
    ///     <b>Cabecera: </b>private IEnumerator SetSmoothTimeBar()
    ///     <b>Descripci�n: </b> Baja la barra de tiempo acorde con un tiempo m�ximo predeterminado
    /// </summary>
    /// <returns></returns>
    private IEnumerator SetSmoothTimeBar()
    {
        if (!isHealing)
        {
            timeBar.GetComponent<Image>().fillAmount -= HealthLoss;
            if (timeBar.GetComponent<Image>().fillAmount == 0)
            {
                gameController.haPerdido = true;
            }
        }
        else
        {
            timeBar.GetComponent<Image>().fillAmount += 0.0001f*2;
            if (timeBar.GetComponent<Image>().fillAmount >= 1)
            {
                timeBar.GetComponent<Image>().fillAmount = 1;
            }
        }
        yield return new WaitForSeconds(1);
    }

    /// <summary>
    ///     <b>Cabecera: </b>public void SetTimeBar(float normalizedValue)
    ///     <b>Descripci�n: </b> Cambia el tamna�o de la barra de vida con respecto al valor dado
    /// </summary>
    /// <param name="addedValue"> float con el tiempo normalizado</param>
    public void AddTime(float addedValue)
    {
        if ((health * 0.00001) + (addedValue * HealthLoss) >= 1)
        {
            health = 100000;
        }
        else if (addedValue != 0)
        {
            health += addedValue;
        }
        timeBar.GetComponent<Image>().fillAmount += (float) (health * 0.01);
    }
    
    /// <summary>
    ///     <b>Cabecera: </b>private void SetPlayerSeeds()
    ///     <b>Descripci�n: </b> Cambia la interfaz de las semillas restantes con respecto al numero de semillas
    /// </summary>
    public void SetPlayerSeeds()
    {
        switch (playerController.availableSeeds)
        {
            case 0:
                if (textContadorSeed1.gameObject.activeInHierarchy && !textContadorSeed2.gameObject.activeInHierarchy)
                {
                    ChangeSeedImage(seed2, usedImage);
                    HUD_CoolDown auxCooldown = new HUD_CoolDown();
                    StartCoroutine(auxCooldown.StartCountdown(textContadorSeed2));
                }
                else if (!textContadorSeed1.gameObject.activeInHierarchy && textContadorSeed2.gameObject.activeInHierarchy)
                {
                    ChangeSeedImage(seed1, usedImage);
                    HUD_CoolDown auxCooldown = new HUD_CoolDown();
                    StartCoroutine(auxCooldown.StartCountdown(textContadorSeed1));
                }
                break;

            case 1:
                if(!textContadorSeed1.gameObject.activeInHierarchy && !textContadorSeed2.gameObject.activeInHierarchy)
                {
                    ChangeSeedImage(seed1, defaultImage);
                    ChangeSeedImage(seed2, usedImage);
                    HUD_CoolDown auxCooldown = new HUD_CoolDown();
                    StartCoroutine(auxCooldown.StartCountdown(textContadorSeed2));
                }
                else if (textContadorSeed1.gameObject.activeInHierarchy && !textContadorSeed2.gameObject.activeInHierarchy)
                {
                    ChangeSeedImage(seed2, defaultImage);
                }
                else
                {
                    ChangeSeedImage(seed1, defaultImage);
                }
                break;

            case 2:
                ChangeSeedImage(seed1, defaultImage);
                ChangeSeedImage(seed2, defaultImage);
                break;
        }
    }

    /// <summary>
    ///     <b>Cabecera: </b>private void ChangeSeedImage(GameObject seed, Sprite sprite)
    ///     <b>Descripci�n: </b> Cambia el sprite de la imagen de las semillas
    /// </summary>
    private void ChangeSeedImage(GameObject seed, Sprite sprite)
    {
        seed.GetComponent<Image>().sprite = sprite;
    }
    #endregion

    public float getTime()
    {
        return time;
    }

}
