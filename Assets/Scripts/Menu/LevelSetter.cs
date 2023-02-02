using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelSetter : MonoBehaviour
{
    #region variables
    /// <summary>
    /// objeto que contiene el controlador del menu
    /// </summary>
    [SerializeField] private MenuController menu;
    /// <summary>
    /// nombre de la escena a la que llama el botón
    /// </summary>
    public string levelSceneName;
    /// <summary>
    /// referencia al objeto rankingSaver
    /// </summary>
    public RankingSaver rankingSaver;
    /// <summary>
    /// Texto en el que mostrar el ranking
    /// </summary>
    [SerializeField] private TextMeshProUGUI rankingText;
    [SerializeField] private TextMeshProUGUI levelNameText;
    #endregion

    /// <summary>
    /// Buscamos el objeto rankigSaver
    /// </summary>
    void Start()
    {
        rankingSaver = GameObject.Find("Ranking_Saver").GetComponent<RankingSaver>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// Cuando se le haga click llamará al metodo de seleccion de nivel del menu
    /// Mostrará el ranking en el campo de texto
    /// </summary>
    public void OnBotonPulsado()
    {
        menu.OnButtonLevel(levelSceneName);
        ShowRanking(levelSceneName);
        levelNameText.text = levelSceneName;
    }

    /// <summary>
    /// Muestra los 15 primeros registros del ranking de la escena seleccionada
    /// </summary>
    /// <param name="levelName">nombre de la escena seleccionada</param>
    public void ShowRanking(string levelName)
    {
        rankingText.SetText(rankingSaver.ShowLevelRank(levelName,15));
    }


}
