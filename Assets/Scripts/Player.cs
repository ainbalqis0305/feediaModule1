using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Networking;

public class Player : MonoBehaviour
{
    public int minLevel = 0;
    public int currentLevel;

    public GameObject farmerJumping;
    public LevelBar levelBar;
    public Image farmerSadImage;
    public TextMeshProUGUI resultText;
    public TextMeshProUGUI playerLevel;


    // Start is called before the first frame update
    /*void Start()
    {
        currentLevel = minLevel;
        levelBar.SetMinLevel(minLevel);

        int selectedID = PlayerPrefs.GetInt("SelectedId", -1);

        if (selectedID == 1)
        {
            resultText.text = "You just increase your food security level! It's time to level it up";
            IncreaseLevel(1);
        }
        else if (selectedID == 2 || selectedID == 3)
        {
            resultText.text = "Oh no! Your food security level is at risk! Replay to get a better food security level";
            LoadSadFarmerImage();
        }
    }*/

    void Start()
    {
        currentLevel = minLevel;
        levelBar.SetMinLevel(minLevel);


        //StartCoroutine(FetchSecurityLevel());
        StartCoroutine(FetchPlayerProgress());
    }

    IEnumerator FetchPlayerProgress()
    {
        UnityWebRequest playerIDRequest = UnityWebRequest.Get("http://localhost/feedia/get_player_id.php");
        yield return playerIDRequest.SendWebRequest();

        if (playerIDRequest.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("Failed to fetch playerID: " + playerIDRequest.error);
            yield break;
        }

        string playerID = playerIDRequest.downloadHandler.text;

        if (playerID == "Player not found")
        {
            Debug.LogError("No player found in the database.");
            yield break;
        }

        UnityWebRequest progressRequest = UnityWebRequest.Get($"http://localhost/feedia/get_player_progress.php?playerID={playerID}");
        yield return progressRequest.SendWebRequest();

        if (progressRequest.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("Failed to fetch player progress: " + progressRequest.error);
            yield break;
        }

        string progressData = progressRequest.downloadHandler.text;

        if (!string.IsNullOrEmpty(progressData) && progressData.Contains(","))
        {
            string[] data = progressData.Split(',');
            if (int.TryParse(data[0], out int badgeCount) && int.TryParse(data[1], out int securityLevel))
            {
                Debug.Log($"Fetched Player Progress - BadgeCount: {badgeCount}, SecurityLevel: {securityLevel}");

                EvaluateProgress(badgeCount, securityLevel);
            }
            else
            {
                Debug.LogError("Invalid progress data received from server.");
            }
        }
        else
        {
            Debug.LogError("Invalid or empty progress data received.");
        }
    }


    /*
    void EvaluateProgress(int badgeCount, int securityLevel) 
    {
        if (securityLevel == 1 && badgeCount == 1)
        {
            resultText.text = "You just increase your food security level! It's time to level it up";
            playerLevel.text = "1/4" + "This is wrong!";
            IncreaseLevel(1);
        }
        else if (securityLevel == 2 && badgeCount == 5)
        {
            resultText.text = "You just increase your food security level! It's time to level it up";
            playerLevel.text = "2/4";
            IncreaseLevel(2);
        }
        else if (securityLevel == 0 || badgeCount == 0)
        {
            resultText.text = "You just increase your food security level! It's time to level it up";
            playerLevel.text = "0/4";
            LoadSadFarmerImage();
        }
        else
        {
            resultText.text = "Oh no! Your food security level is at risk! Replay to get a better food security level";
            playerLevel.text = "0/4";
            LoadSadFarmerImage();
        }
    }*/


    void EvaluateProgress(int badgeCount, int securityLevel)
    {
        Debug.Log($"BadgeCount: {badgeCount}, SecurityLevel: {securityLevel}");

        if (securityLevel == 1 && badgeCount == 1)
        {
            resultText.text = "You just increase your food security level! It's time to level it up";
            playerLevel.text = "1/4";
            IncreaseLevel(1);
        }
        else if (securityLevel == 2 && badgeCount == 5)
        {
            resultText.text = "You just increase your food security level! It's time to level it up";
            playerLevel.text = "2/4";
            IncreaseLevel(2);
        }
        else if (securityLevel == 0 && badgeCount == 0)
        {
            resultText.text = "Oh no! Your food security level is at risk! Replay to get a better food security level";
            playerLevel.text = "0/4";
            LoadSadFarmerImage();
        }
        else
        {
            resultText.text = "Oh no! Your food security level is at risk! Replay to get a better food security level";
            playerLevel.text = "0/4";
            LoadSadFarmerImage();
        }
    }


    void IncreaseLevel(int increase)
    {
        currentLevel += increase;

        levelBar.SetLevel(currentLevel);
    }

    void LoadSadFarmerImage()
    {
        Texture2D texture = Resources.Load<Texture2D>("farmer_sad");

        if (texture != null)
        {
            farmerSadImage.sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
            farmerSadImage.enabled = true;
        }
        else
        {
            Debug.LogError("Image farmer_sad not found");
            farmerSadImage.enabled =false;
        }

        if (farmerJumping != null)
        {
            farmerJumping.SetActive(false);
        }
        else
        {
            Debug.LogError("farmerJumping gameobject is not assigned in the Inspector");
        }
    }

    public void continueButton()
    {
        //PlayerPrefs.SetInt("SelectedId", selectedID);

        SceneManager.LoadScene(SceneData.dashboard);
    }

    public void replayButton()
    {
        //PlayerPrefs.SetInt("SelectedId", selectedID);

        SceneManager.LoadScene(SceneData.scene1);
    }

    
}
