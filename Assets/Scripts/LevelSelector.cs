using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelSelector : MonoBehaviour
{
    public Button level1Button;
    public Button level2Button;
    public Button level3Button;
    public Button level4Button;

    public Image level2Lock;
    public Image level3Lock;
    public Image level4Lock;

    public GameObject arrow1;
    public GameObject arrow2;
    public GameObject arrow3;

    private int securityLevel = 0;
    private int playerID = 0;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(FetchID_SecurityLevel());
    }

    IEnumerator FetchID_SecurityLevel()
    {
        string playerIDurl = "http://localhost/feedia/get_player_id.php";

        using (UnityWebRequest playerIDrequest = UnityWebRequest.Get(playerIDurl))
        {
            yield return playerIDrequest.SendWebRequest();

            if (playerIDrequest.result == UnityWebRequest.Result.Success)
            {
                if (int.TryParse(playerIDrequest.downloadHandler.text, out int id))
                {
                    playerID = id;
                }
                else
                {
                    Debug.LogError("Failed to parse playerID:" + playerIDrequest.downloadHandler.text);
                    yield break;
                }
            }
            else
            {
                Debug.LogError("Failed to fetch playerID: " + playerIDrequest.error);
                yield break;
            }
        }

            string securitylevelurl = "http://localhost/feedia/get_security_level.php?playerID={playerID}";

            using (UnityWebRequest securitylevelrequest = UnityWebRequest.Get(securitylevelurl))
            {
                yield return securitylevelrequest.SendWebRequest();

                if (securitylevelrequest.result == UnityWebRequest.Result.Success)
                {
                    if (int.TryParse(securitylevelrequest.downloadHandler.text, out int level))
                    {
                        securityLevel = level;
                        UpdateButtonStates();
                    }
                    else
                    {
                        Debug.LogError("Failed to parse security level: " + securitylevelrequest.downloadHandler.text);
                    }
                }
                else
                {
                    Debug.LogError("Failed to fetch security level: " + securitylevelrequest.error);
                }
            }
        }

    void UpdateButtonStates()
    {
        // Activate Level 1 button
        level1Button.interactable = true;
        if (arrow1 != null)
        {
            arrow1.SetActive(true); // Ensure arrow1 is active at Level 1
        }

        // Update Level 2 button
        if (securityLevel >= 1)
        {
            level2Button.interactable = true;
            level2Lock.gameObject.SetActive(false);

            if (arrow1 != null)
            {
                arrow1.SetActive(false); // Deactivate arrow1 at Level 2
            }

            /*if (arrow2 != null)
            {
                arrow2.SetActive(false); // Ensure arrow2 stays inactive
            }*/
        }
        else
        {
            level2Button.interactable = false;
            level2Lock.gameObject.SetActive(true);

            if (arrow1 != null)
            {
                arrow1.SetActive(true); // Keep arrow1 active only at Level 1
            }

            /*if (arrow2 != null)
            {
                arrow2.SetActive(false); // Ensure arrow2 remains inactive
            }*/
        }


        //Update level 3 button
        if (securityLevel >= 2)
        {
            level3Button.interactable = true;
            level3Lock.gameObject.SetActive(false);

            if (arrow2 != null)
            {
                arrow2.SetActive(false);
            }

            if (arrow3 != null)
            {
                arrow3.SetActive(true);
            }
        }
        else
        {
            level3Button.interactable = false;
            level3Lock.gameObject.SetActive(true);

            if (arrow2 != null)
            {
                arrow2.SetActive(true); // arrow1 is active if level 2 is locked
            }

            if (arrow3 != null)
            {
                arrow3.SetActive(false); // arrow2 is off if level 2 is locked
            }
        }

        //Update level 4 button
        if (securityLevel >= 3)
        {
            level4Button.interactable = true;
            level4Lock.gameObject.SetActive(false);
        }
        else
        {
            level4Button.interactable = false;
            level4Lock.gameObject.SetActive(true);
        }
    }
    public void gotoLevel1()
    {
        SceneManager.LoadScene(SceneData.scene1);
    }

    public void gotoLevel2() { 
         SceneManager.LoadScene (SceneData.level2scene1);
    }

}
