using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public bool haPerdido, haGanado;
    public AudioClip nivel1, nivel2, nivel3;

    // Start is called before the first frame update
    void Start()
    {
        switch (SceneManager.GetActiveScene().name)
        {
            case "Nivel_1":
                SoundManager.SharedInstance.PlayMusic(nivel1);
                break;
            case "Nivel_2":
                SoundManager.SharedInstance.PlayMusic(nivel2);
                break;
            case "Nivel_3":
                SoundManager.SharedInstance.PlayMusic(nivel3);
                break;
            case "Nivel_4":
                SoundManager.SharedInstance.PlayMusic(nivel3);
                break;
        }
    }

    private void Update()
    {
        ControlaReinicio();
    }

    public void ControlaReinicio()
    {
        if (Input.GetKeyDown(KeyCode.R) && !haGanado && !haPerdido)
        {
            ReiniciarNivel();
        }
        if (haPerdido)
        {
            ReiniciarNivel();
        }
    }

    /// <summary>
    /// Metodo para reiniciar la escena actual.
    /// </summary>
    public void ReiniciarNivel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void ReponerPlataforma(GameObject Platform, Vector3 localizacion) {

        Instantiate(Platform, localizacion, Quaternion.identity);

    }
}
