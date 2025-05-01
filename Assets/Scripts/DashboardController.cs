using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DashboardController : MonoBehaviour
{
    // Start is called before the first frame update

    public GameObject selectLevelPanel;
    public GameObject arrow;
    public TextMeshProUGUI infoText;

    private void Start()
    {
        if (selectLevelPanel != null)
        {
            selectLevelPanel.SetActive(false);
        }

        InfoLoader infoloader = FindObjectOfType<InfoLoader>();
        if (infoloader != null && infoText != null)
        {
            infoText.text = infoloader.GetRandomInfo();
        }
        else
        {
            Debug.LogError("InfoLoader or infoText is not set up properly!");
        }
    }

    public void ShowSelectLevelPanel()
    {
        if (selectLevelPanel != null)
        {
            selectLevelPanel.SetActive(true);
        }

        if (arrow != null)
        {
            arrow.SetActive(false);
        }
    }

    public void CloseSelectLevelPanel()
    {
        if (selectLevelPanel != null)
        {
            selectLevelPanel.SetActive(false);
        }

        if (arrow != null)
        {
            arrow.SetActive(true);
        }
    }
    
}
