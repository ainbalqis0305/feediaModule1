using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BadgeManagerL2_4 : MonoBehaviour
{

    public TextMeshProUGUI badgeText;
    public Image bannerImage;
    public Image xImage;
    public Button replayButton;

    public string congratulation = "congratulations";
    public string ohno = "ohno";
    public string x = "ex";

    private int badgeCount = 0;
    private int securityLevel = 0;
    private int playerID;
    // Start is called before the first frame update
    IEnumerator Start()
    {
        // Wait until playerID is fetched
        yield return FetchPlayerID();

        yield return FetchPlayerProgress();

        if (badgeCount == 4)
        {
            badgeCount++;
            if (badgeCount == 5)
            {
                securityLevel++;
            }
            UpdateDatabase(playerID, badgeCount, securityLevel);

            badgeText.text = "YOU RECEIVED:";
            LoadImage(bannerImage, congratulation);
            Debug.Log($"badgeCount is now {badgeCount}, securityLevel is {securityLevel}");


        } 
        else if (badgeCount == 5)
        {
            badgeText.text = "YOU RECEIVED:";
            LoadImage(bannerImage, congratulation);
            Debug.Log($"badgeCount is now {badgeCount}, securityLevel is {securityLevel}");
        }
        else
        {
            badgeText.text = "YOU ARE LOSING A BADGE:";
            LoadImage(bannerImage, ohno);
            LoadImage(xImage, x);
            replayButton.gameObject.SetActive(true);
        }

        /*if (selectedID == 7)
        {
            badgeText.text = "YOU RECEIVED:";
            LoadImage(bannerImage, congratulation);

            if (badgeCount < 3)
            {
                badgeCount++;

                PlayerPrefs.SetInt("BadgeCount", badgeCount);
                PlayerPrefs.SetInt("SecurityLevel", securityLevel);

                // Ensure playerID is valid before updating the database
                if (playerID > 0)
                {
                    UpdateDatabase(playerID, badgeCount, securityLevel);
                }
                else
                {
                    Debug.LogError("Player ID is not valid");
                }
            }
            else
            {
                badgeText.text = "YOU RECEIVED:";
                LoadImage(bannerImage, congratulation);
                Debug.Log("BadgeCount is already 3, no increment performed.");
            }
        }
        else
        {
            badgeText.text = "YOU ARE LOSING A BADGE:";
            LoadImage(bannerImage, ohno);
            LoadImage(xImage, x);
            replayButton.gameObject.SetActive(true);
        }*/


    }

    private void LoadImage(Image imageComponent, string image)
    {
        Texture2D texture = Resources.Load<Texture2D>(image);
        if (texture != null)
        {
            imageComponent.sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
            imageComponent.enabled = true;
        }
        else
        {
            Debug.LogError("Image not found at path: " + image);
            imageComponent.enabled = false;
        }
    }

    public void replayButton4()
    {
        SceneManager.LoadScene(SceneData.level2scene1);
    }

    public void gotoNextScene4()
    {
        SceneManager.LoadScene(SceneData.level2result);
    }

    private IEnumerator FetchPlayerID()
    {
        using (UnityWebRequest www = UnityWebRequest.Get("http://localhost/feedia/get_player_id.php"))
        {
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.Success)
            {
                //playerID = int.Parse(www.downloadHandler.text);
                //Debug.Log("Player ID fetched: " + playerID);
                string responseText = www.downloadHandler.text.Trim();
                Debug.Log("Server response: " + responseText);

                if (int.TryParse(responseText, out playerID))
                {
                    Debug.Log("Player ID fetched successfully: " + playerID);
                }
                else
                {
                    Debug.LogError($"Failed to parse playerID. Response: {responseText}");
                }
            }
            else
            {
                Debug.LogError("Error fetching playerID: " + www.error);
            }
        }
    }

    private IEnumerator FetchPlayerProgress()
    {
        using (UnityWebRequest www = UnityWebRequest.Get($"http://localhost/feedia/get_player_progress.php?playerID={playerID}"))
        {
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.Success)
            {
                string[] progressData = www.downloadHandler.text.Split(',');
                if (progressData.Length == 2 &&
                    int.TryParse(progressData[0], out badgeCount) &&
                    int.TryParse(progressData[1], out securityLevel))
                {
                    if (badgeCount > 0 && securityLevel > 0)
                    {
                        Debug.Log($"Player Progress: badgeCount={badgeCount}, securityLevel={securityLevel}");
                    }
                    else
                    {
                        Debug.LogError("Fetched badgeCount or securityLevel is zero, invalid data.");
                    }
                }
                else
                {
                    Debug.LogError("Failed to parse player progress data.");
                }
            }
            else
            {
                Debug.LogError("Error fetching player progress: " + www.error);
            }
        }
    }

    private void UpdateDatabase(int playerID, int badgeCount, int securityLevel)
    {
        if (badgeCount > 0 && securityLevel > 0)
        {
            Debug.Log($"Updating database with playerID: {playerID}, badgeCount: {badgeCount}, securityLevel: {securityLevel}");
            StartCoroutine(SendDataToDatabase(playerID, badgeCount, securityLevel));
        }
        else
        {
            Debug.LogError("Cannot update database with zero values for badgeCount or securityLevel.");
        }
    }

    //Send data to PHP script
    private IEnumerator SendDataToDatabase(int playerID, int badgeCount, int securityLevel)
    {
        WWWForm form = new WWWForm();
        form.AddField("playerID", playerID);
        form.AddField("badgeCount", badgeCount);
        form.AddField("securityLevel", securityLevel);

        using (UnityWebRequest www = UnityWebRequest.Post("http://localhost/feedia/update_progress.php", form))
        {
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.Success)
            {
                Debug.Log("Database update successful");
            }
            else
            {
                Debug.LogError("Error updating database: " + www.error);
            }

        }
    }
}
