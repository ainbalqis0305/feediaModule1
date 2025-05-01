using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Pause : MonoBehaviour
{
    public GameObject pausePanel;
    // Start is called before the first frame update
    void Start()
    {
        pausePanel.SetActive(false);
    }

    public void OnPauseButtonClick()
    {
        pausePanel.SetActive(true);
    }

    public void OnExitButtonClick()
    {
        SceneManager.LoadScene(SceneData.dashboard);
    }
    
    public void OnContinueButtonClick()
    {
        pausePanel.SetActive(false);
    }


  
}
