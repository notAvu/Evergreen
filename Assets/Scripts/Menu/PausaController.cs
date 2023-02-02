using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PausaController : MonoBehaviour
{
    #region variables
    /// <summary>
    /// Panel de pausa
    /// </summary>
    [SerializeField] private GameObject pausaPanel;
    /// <summary>
    /// Panel con el ranking
    /// </summary>
    [SerializeField] private GameObject rankingTab;
    /// <summary>
    /// Texto en el que se muestra el ranking
    /// </summary>
    [SerializeField] private TextMeshProUGUI rankingText;
    /// <summary>
    /// Escena del menu
    /// </summary>
    public string menuSceneName;
    /// <summary>
    /// Si el juego esta pausado o no
    /// </summary>
    public bool gamePaused;
    /// <summary>
    /// Objeto que contiene el control del juego
    /// </summary>
    private GameController gameController;
    /// <summary>
    /// objeto RankingSaver
    /// </summary>
    public RankingSaver rankingSaver;
    /// <summary>
    /// Si se esta mostrando el ranking o no
    /// </summary>
    private bool rankingShow;

    public AudioClip bottomSound;
    #endregion


    /// <summary>
    /// Se busca el objeto gamecontroler el ranking saver y se establece la pausa y el mostrado de ranking a false
    /// se ocultan los paneles de pausa y de ranking
    /// </summary>
    void Start()
    {
        gameController = GameObject.Find("GameController").GetComponent<GameController>();
        gamePaused = false;
        pausaPanel.SetActive(false);
        rankingTab.SetActive(false);
        rankingShow = false;
        rankingSaver = GameObject.Find("Ranking_Saver").GetComponent<RankingSaver>();
    }

    /// <summary>
    /// Si se pulsa ESC y no esta pausado se pausa y si no se continua
    /// Si se pulsa tab y no esta pausado ni mostrado el ranking muestra el ranking,
    /// en caso contrario oculta el ranking
    /// </summary>
    void Update()
    {
        if (!gameController.haGanado)
        {
            //si pulsas escape
            if (Input.GetButtonDown("Cancel"))
            {
                //si el juego esta pausado
                if (gamePaused)
                    ResumeGame();
                else
                    PauseGame();
            }

            if (Input.GetKeyDown(KeyCode.Tab))
            {
                if (!gamePaused && !rankingShow)
                {
                    ShowRanking();
                    rankingShow = true;
                }
                else
                {
                    rankingTab.SetActive(false);
                    rankingShow = false;
                }
            }
        }
    }
    
    /// <summary>
    /// Pausa el juego. Pone el tiempo a 0 y muestra el menu de pausa
    /// </summary>
    private void PauseGame()
    {
        Time.timeScale = 0;
        pausaPanel.SetActive(true);
        gamePaused = true;
    }

    /// <summary>
    /// Reanuda el juego
    /// pone el tiempo a 1 y oculta el menu de pausa
    /// </summary>
    private void ResumeGame()
    {
        Time.timeScale = 1;
        pausaPanel.SetActive(false);
        gamePaused = false;
    }

    /// <summary>
    /// Caundo se pulse el menu de continuar se llama al método de reanudar
    /// </summary>
    public void OnButtonResume()
    {
        SoundManager.SharedInstance.PlaySound(bottomSound);
        ResumeGame();
    }

    /// <summary>
    /// Llama al método de reinicio
    /// </summary>
    public void OnButtonReset()
    {
        SoundManager.SharedInstance.PlaySound(bottomSound);
        ResumeGame();
        gameController.ReiniciarNivel();
    }

    /// <summary>
    /// Al pulsar el botón de salir se llama al menú del juego (Escena inicial)
    /// </summary>
    public void OnButtonMenu()
    {
        SoundManager.SharedInstance.PlaySound(bottomSound);
        SceneManager.LoadScene(menuSceneName);
        ResumeGame();
    }

    /// <summary>
    /// Carga el ranking desde el fichero y lo muestra
    /// </summary>
    public void ShowRanking()
    {
        rankingText.SetText(rankingSaver.ShowLevelRank(SceneManager.GetActiveScene().name, 15));
        rankingTab.SetActive(true);
        
    }

}
