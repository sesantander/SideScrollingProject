using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void OnRestartClick()
    {
        SceneManager.LoadScene("SampleScene");
    }

    public void QuitGameClick()
    {
        UnityEditor.EditorApplication.isPlaying = false;
    }
}
