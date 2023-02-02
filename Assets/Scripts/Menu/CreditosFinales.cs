using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CreditosFinales : MonoBehaviour
{
    public string nextScene;
    public AudioClip bottomSound;

    public void LoadScene()
    {
        SoundManager.SharedInstance.PlaySound(bottomSound);
        
        SceneManager.LoadScene(nextScene);
    }

}
