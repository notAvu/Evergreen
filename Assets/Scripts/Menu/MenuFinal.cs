using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class MenuFinal : MonoBehaviour
{
    public GameObject finPanel;
    public GameObject textPuntos;
    public GameObject textRanking;
    public HUD_Controller hud;
    public RankingSaver rankingSaver;
    public string nextScene;
    public float maxPoints;
    public AudioClip bottomSound;

    private string puntos;
    [SerializeField] 
    private PausaController pausaController;


    // Start is called before the first frame update
    private void Start()
    {
        rankingSaver = GameObject.Find("Ranking_Saver").GetComponent<RankingSaver>();
        hud = GameObject.Find("HUD").GetComponent<HUD_Controller>();
        finPanel = this.gameObject;
        finPanel.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    /// <summary>
    /// Se llama desde Player Controller cuando el personaje llega a la salida.
    /// Guarda la puntuacion del personaje y muestra el panel de fin del nivel
    /// </summary>
    public void Salida()
    {
        Time.timeScale = 0; //Al finalizar nivel se pausa el juego (Se debe volver a activar el juego tras cambiar de nivel)
        rankingSaver.setPoints(hud.getTime(), maxPoints);
        puntos = ""+rankingSaver.getPoints();
        rankingSaver.addRankingLine(SceneManager.GetActiveScene().name + "|" + puntos + "|" + rankingSaver.getPlayerName());
        rankingSaver.Save();
        textPuntos.GetComponent<TextMeshProUGUI>().text = puntos;
        textRanking.GetComponent<TextMeshProUGUI>().text = rankingSaver.ShowLevelRank(SceneManager.GetActiveScene().name);
        finPanel.SetActive(true);
        pausaController.gamePaused = true;
    }

    public void OnButtonNextLevel()
    {
        SoundManager.SharedInstance.PlaySound(bottomSound);
        Time.timeScale = 1;
        SceneManager.LoadScene(nextScene);
    }
}
