using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using JetBrains.Annotations;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;


/*public class FeedbackManager : MonoBehaviour
{
    public TextMeshProUGUI feedbackText;
    public TextMeshProUGUI choiceText;
    public UnityEngine.UI.Image farmerImage;

    public string happyFarmer = "farmer";
    public string sadFarmer = "farmer_sad"; // ganti farmer sedih
    // Start is called before the first frame update
    void Start()
    {
        int selectedID = PlayerPrefs.GetInt("SelectedId", -1);

        if (selectedID == 1)
        {
            choiceText.text = "GREAT CHOICE!";
            feedbackText.text = "Paddy can survive the transitional monsoon in Malaysia. The seasonal rains provide sufficient water, but proper management is crucial to handle fluctuation in rainfall.";
            LoadImage(happyFarmer);
        }
        else if (selectedID == 2) {
            choiceText.text = "BAD CHOICE!";
            feedbackText.text = "Maize does not need as much water as paddy. The transitional monsoon season may provide too much rainfall, which can lead to waterlogging, affecting maize growth.";
            LoadImage(sadFarmer);
        }
        else if (selectedID == 3)
        {
            choiceText.text = "BAD CHOICE!";
            feedbackText.text = "Wheat is not well-suited for the tropical monsoon climate. Also,it typically does not grow well in the humid, hot conditions of Malaysia.";
            LoadImage(happyFarmer);
        }
    }

    private void LoadImage(string image)
    {
        Texture2D texture = Resources.Load<Texture2D>(image);
        if (texture != null) {
            farmerImage.sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
            farmerImage.enabled = true;
        } else
        {
            Debug.LogError("Image not found at path: " + image);
            farmerImage.enabled=false;
        }
    }
}*/

public class FeedbackManager : MonoBehaviour
{
    public TextMeshProUGUI feedbackText;
    public TextMeshProUGUI choiceText;
    public UnityEngine.UI.Image farmerImage;

    public string happyFarmer = "farmer";
    public string sadFarmer = "farmer_sad";

    private Dictionary<int, Rule> knowledgeBase;

    //int selectedID = PlayerPrefs.GetInt("SelectedId", -1);

    void Start()
    {
        // Initialize the knowledge base
        knowledgeBase = new Dictionary<int, Rule>
        {
            { 1, new Rule("GREAT CHOICE!", "Paddy can survive the transitional monsoon in Malaysia. The seasonal rains provide sufficient water, but proper management is crucial to handle fluctuation in rainfall.", happyFarmer) },
            { 2, new Rule("BAD CHOICE!", "Maize does not need as much water as paddy. The transitional monsoon season may provide too much rainfall, which can lead to waterlogging, affecting maize growth.", sadFarmer) },
            { 3, new Rule("BAD CHOICE!", "Wheat is not well-suited for the tropical monsoon climate. Also, it typically does not grow well in the humid, hot conditions of Malaysia.", sadFarmer) }
        };

        int selectedID = PlayerPrefs.GetInt("SelectedId", -1);

        if (knowledgeBase.TryGetValue(selectedID, out Rule rule))
        {
            choiceText.text = rule.ChoiceText;
            feedbackText.text = rule.FeedbackText;
            LoadImage(rule.ImageName);
        }
        else
        {
            Debug.LogError("Invalid selection ID: " + selectedID);
        }
    }

    public void OnContinueButtonClick()
    {
        int selectedID = PlayerPrefs.GetInt("SelectedId", -1);
        // Load different scenes based on the selected ID
        if (selectedID == 1)
        {
            // If the user chooses Paddy, go to the match-3 game scene
            SceneManager.LoadScene("match3game");
        }
        else
        {
            // For other choices, go to the badge scene
            SceneManager.LoadScene("level1badge");
        }
    }

    private void LoadImage(string image)
    {
        Texture2D texture = Resources.Load<Texture2D>(image);
        if (texture != null)
        {
            farmerImage.sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
            farmerImage.enabled = true;
        }
        else
        {
            Debug.LogError("Image not found at path: " + image);
            farmerImage.enabled = false;
        }
    }
}



public class Rule
{
    public string ChoiceText { get; set; }
    public string FeedbackText { get; set; }
    public string ImageName { get; set; }

    public Rule(string choiceText, string feedbackText, string imageName)
    {
        ChoiceText = choiceText;
        FeedbackText = feedbackText;
        ImageName = imageName;
    }

    
}



