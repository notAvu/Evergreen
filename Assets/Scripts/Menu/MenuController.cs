using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{
    #region variables
    [Header("Panles")]
    //menu principal
    [SerializeField] private GameObject principalPanel;
    //seleccion de nombre
    [SerializeField] private GameObject nameSelectorPanel;
    //seleccion de nivel
    [SerializeField] private GameObject levelSelectorPanel;
    //creditos
    [SerializeField] private GameObject creditsPanel;
    [Header("Texts")]
    //texto del nombre del jugador
    [SerializeField] private Text txtPlayerName;
    /// <summary>
    /// Referencia al objeto de rankigSacer
    /// </summary>
    public RankingSaver rankingSaver;
    /// <summary>
    /// nombre del jugador
    /// </summary>
    private string playerName;
    /// <summary>
    /// escena seleccionada
    /// </summary>
    private string selectedScene;
    public AudioClip bottomSound;
    
    #endregion

    /// <summary>
    /// Inicializamos las variables a null de modo que sea mas facil comprobar que no estan rellenas
    /// </summary>
    void Start()
    {
        playerName = null;
        selectedScene = null;
        ActivatePanel(principalPanel);
        rankingSaver = GameObject.Find("Ranking_Saver").GetComponent<RankingSaver>();
    }

    
    void Update()
    {
        
    }

    /// <summary>
    /// Este m�todo activa el menu necesario en cada momento
    /// Primero desactiva todos los paneles y despues activa el necesario
    /// </summary>
    /// <param name="panel"> panel a activar </param>
    private void ActivatePanel(GameObject panel)
    {
        //se desactivan todos los paneles
        principalPanel.SetActive(false);
        nameSelectorPanel.SetActive(false);
        levelSelectorPanel.SetActive(false);

        //se activa el panel necesario
        panel.SetActive(true);
    }


    /// <summary>
    /// M�todo para activar el panel de seleccion de nombre desde el panel principal
    /// </summary>
    public void OnButtonPlay()
    {
        SoundManager.SharedInstance.PlaySound(bottomSound);
        playerName = null;
        
        ActivatePanel(nameSelectorPanel);
    }

    /// <summary>
    /// M�todo para activar el panel de cr�ditos desde el panel principal
    /// </summary>
    public void OnButtonCredits()
    {
        SoundManager.SharedInstance.PlaySound(bottomSound);
        SceneManager.LoadScene("Creditos");
    }

    /// <summary>
    /// Bot�n de salir en el men� principal
    /// Este bot�n cierra la aplicaci�n.
    /// </summary>
    public void OnButtonExit()
    {
        SoundManager.SharedInstance.PlaySound(bottomSound);
        Application.Quit();
    }

    /// <summary>
    /// En este m�todo se establece el nombre del jugador y se accede a seleccion de nivel
    /// </summary>
    public void OnButtonSelectLevel()
    {
        if (!string.IsNullOrEmpty(txtPlayerName.text)|| !string.IsNullOrWhiteSpace(txtPlayerName.text))
        {
            playerName = txtPlayerName.text;
            rankingSaver.setPlayerName(playerName);
        }

        if (!string.IsNullOrEmpty(txtPlayerName.text))
        {
            SoundManager.SharedInstance.PlaySound(bottomSound);
            ActivatePanel(levelSelectorPanel);
        }
    }

    /// <summary>
    /// M�todo para volver al panel principal
    /// </summary>
    public void OnButtonMenu()
    {
        SoundManager.SharedInstance.PlaySound(bottomSound);
        ActivatePanel(principalPanel);
        selectedScene = null;
        playerName = null;
        
    }

    /// <summary>
    /// Establece que nivel estamos seleccionando
    /// Se llamar� desde el propio bot�n del nivel
    /// Desde aqui se llamar� al m�todo para mostrar el ranking del nivel
    /// </summary>
    /// <param name="level"></param>
    public void OnButtonLevel(string level)
    {
        if (!string.IsNullOrEmpty(level))
        {
            SoundManager.SharedInstance.PlaySound(bottomSound);
            selectedScene = level;
        }
    }

    /// <summary>
    /// Cargar� la escena seleccionada, si no hay ninguna escena seleccionada no hace nada
    /// </summary>
    public void OnButtonPlayLevel()
    {
        if (!string.IsNullOrEmpty(selectedScene))
        {
            SoundManager.SharedInstance.PlaySound(bottomSound);
            SceneManager.LoadScene(selectedScene);
        }
    }




}
