using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;

public class UI_InputWindow : MonoBehaviour
{

    public TextMeshProUGUI output;
    public TMP_InputField userName;

    public void OKButton()
    {
        string playerName = userName.text;
        output.text = "Hello there, " + playerName;

        StartCoroutine(SendNameToDatabase(playerName));

        StartCoroutine(WaitAndLoadScene(3)); // Start coroutine to wait and load scene after 3 seconds
    }

    private IEnumerator WaitAndLoadScene(float waitTime)
    {
        yield return new WaitForSeconds(waitTime); // Wait for the specified time
        SceneManager.LoadScene(SceneData.dashboard); // Replace "YourSceneName" with the name of your next scene
    }
    public void CancelButton()
    {
        SceneManager.LoadScene(SceneData.landingpage);
    }

    private IEnumerator SendNameToDatabase(string playerName)
    {
        string url = "http://localhost/feedia/Player.php";

        WWWForm form = new WWWForm();
        form.AddField("playerName", playerName);

        UnityWebRequest www = UnityWebRequest.Post(url, form);
        
        yield return www.SendWebRequest();

        if (www.result == UnityWebRequest.Result.Success)
        {
            Debug.Log("Player name sent to database successfully!");
        }
        else
        {
            Debug.LogError("Error sending player name to database: " + www.error);
        }

    }
}
