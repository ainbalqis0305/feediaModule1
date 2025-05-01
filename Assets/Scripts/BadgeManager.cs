using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BadgeManager : MonoBehaviour
{
    public TextMeshProUGUI badgeText;
    public Image bannerImage;
    public Image xImage;
   

    public string congratulation = "congratulations";
    public string ohno = "ohno";
    public string x = "ex";

    private int badgeCount = 0;
    private int securityLevel = 0;
    private int playerID;

    // Start is called before the first frame update
    IEnumerator Start()
    {
        // Retrieve the player ID from the database
        yield return FetchPlayerID();

        // Fetch badgeCount and securityLevel for the playerID from the database
        yield return FetchPlayerProgress();

        // Log the fetched data
        Debug.Log($"After Fetch: PlayerID = {playerID}, BadgeCount = {badgeCount}, SecurityLevel = {securityLevel}");

        // Get selectedID from the previous page
        int selectedID = PlayerPrefs.GetInt("SelectedId", -1);
        Debug.Log($"SelectedID from previous page: {selectedID}");

        if (selectedID == 1) // If player received a badge
        {
            badgeText.text = "YOU RECEIVED:";
            LoadImage(bannerImage, congratulation);

            if (securityLevel < 1)
            {
                badgeCount++;
                securityLevel++;

                if (playerID > 0)
                {
                    // Log before updating the database
                    Debug.Log($"Before Update: PlayerID = {playerID}, BadgeCount = {badgeCount}, SecurityLevel = {securityLevel}");

                    // Update the updated badgeCount and securityLevel to the database
                    yield return UpdatePlayerProgress();

                    // Log after database update
                    Debug.Log($"After Update: PlayerID = {playerID}, BadgeCount = {badgeCount}, SecurityLevel = {securityLevel}");
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
                Debug.Log("SecurityLevel is already >= 1, no increment performed.");

            }

        }
        else // If player is losing a badge
        {
            badgeText.text = "YOU ARE LOSING A BADGE:";
            LoadImage(bannerImage, ohno);
            LoadImage(xImage, x);


            // Log when no update is made
            Debug.Log($"No Update: PlayerID = {playerID}, BadgeCount = {badgeCount}, SecurityLevel = {securityLevel}");
        }
    }

    private IEnumerator FetchPlayerID()
    {
        using (UnityWebRequest request = UnityWebRequest.Get("http://localhost/feedia/get_player_id.php"))
        {
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                if (int.TryParse(request.downloadHandler.text.Trim(), out playerID))
                {
                    Debug.Log($"Fetched PlayerID: {playerID}");
                }
                else
                {
                    Debug.LogError($"Failed to parse PlayerID. Response: {request.downloadHandler.text}");
                }
            }
            else
            {
                Debug.LogError($"Error fetching PlayerID: {request.error}");
            }
        }
    }

    private IEnumerator FetchPlayerProgress()
    {
        string url = $"http://localhost/feedia/get_player_progress.php?playerID={playerID}";
        using (UnityWebRequest request = UnityWebRequest.Get(url))
        {
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                string[] progressData = request.downloadHandler.text.Split(',');

                if (progressData.Length == 2 &&
                    int.TryParse(progressData[0], out badgeCount) &&
                    int.TryParse(progressData[1], out securityLevel))
                {
                    Debug.Log($"Fetched Progress: BadgeCount = {badgeCount}, SecurityLevel = {securityLevel}");
                }
                else
                {
                    Debug.LogError("Invalid progress data received, resetting badgeCount and securityLevel.");
                    badgeCount = 0;
                    securityLevel = 0;
                }
            }
            else
            {
                Debug.LogError($"Failed to fetch player progress: {request.error}");
            }
        }
    }

    private IEnumerator UpdatePlayerProgress()
    {
        WWWForm form = new WWWForm();
        form.AddField("playerID", playerID);
        form.AddField("badgeCount", badgeCount);
        form.AddField("securityLevel", securityLevel);

        using (UnityWebRequest request = UnityWebRequest.Post("http://localhost/feedia/update_progress.php", form))
        {
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                Debug.Log($"Updated Progress in Database: BadgeCount = {badgeCount}, SecurityLevel = {securityLevel}");
            }
            else
            {
                Debug.LogError($"Error updating player progress: {request.error}");
            }
        }
    }

    private void LoadImage(Image imageComponent, string imageName)
    {
        Texture2D texture = Resources.Load<Texture2D>(imageName);
        if (texture != null)
        {
            imageComponent.sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
            imageComponent.enabled = true;
        }
        else
        {
            Debug.LogError($"Image not found: {imageName}");
            imageComponent.enabled = false;
        }
    }

    public void GotoResult()
    {
        SceneManager.LoadScene("level1result");
    }
}
