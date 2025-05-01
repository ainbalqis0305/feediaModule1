using DG.Tweening.Core.Easing;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static Unity.VisualScripting.Member;
using UnityEngine.Profiling;
using UnityEngine.SocialPlatforms;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

/*public class FeedbackManagerLevel2 : MonoBehaviour
{
    public TextMeshProUGUI feedbackText;
    public TextMeshProUGUI choiceText;
    public UnityEngine.UI.Image farmerImage;

    public string happyFarmer = "farmer_smart";
    public string sadFarmer = "farmer_sad"; //ganti farmer smart sedih
    // Start is called before the first frame update
    void Start()
    {
        int selectedID = PlayerPrefs.GetInt("SelectedId", -1);

        if (selectedID == 4)
        {
            choiceText.text = "GREAT CHOICE!";
            feedbackText.text = "Pickup trucks are ideal for small-scale farmers in Malaysia due to their moderate cargo capacity, ability to handle rural road, affordability, and accessibility on narrow farm paths and local markets.";
            LoadImage(happyFarmer);
        }
        else if (selectedID == 5)
        {
            choiceText.text = "BAD CHOICE!";
            feedbackText.text = "While a car is suitable for urban driving, it lacks the cargo capacity to handle a medium-scale harvest. A car would likely require multiple trips, making it inefficient for transporting crops.";
            LoadImage(sadFarmer);
        }
        else if (selectedID == 6)
        {
            choiceText.text = "BAD CHOICE!";
            feedbackText.text = "A lorry has the capacity for larger loads but may be excessive for a medium-scale harvest, particularly over short distances. However, lorries would be more efficient for long-distance transport or very large-scale operations.";
            LoadImage(sadFarmer);
        }
        else if (selectedID == 7)
        {
            choiceText.text = "GREAT CHOICE!";
            feedbackText.text = "Wholesale markets not only offer lower prices to consumers but also provide a wider variety of food supplies in larger quantities.";
            LoadImage(happyFarmer);
        }
        else if (selectedID == 8)
        {
            choiceText.text = "BAD CHOICE!";
            feedbackText.text = "While conveniently located near UPM, Putra Mart is a smaller-scale market catering, with limited stock available for bulk purchases.";
            LoadImage(sadFarmer);
        }
        else if (selectedID == 9)
        {
            choiceText.text = "BAD CHOICE!";
            feedbackText.text = "Pasar Tani offers fresh, locally sourced crops. However, the availability of large quantities may be limited and it is better for smaller-scale or regular purchases.";
            LoadImage(sadFarmer);
        }
        else if (selectedID == 10)
        {
            choiceText.text = "BAD CHOICE!";
            feedbackText.text = "Western meals can be more expensive, often approaching the upper limit of the budget or exceeding it. While they can be filling, they might offer less variety in nutrients.";
            LoadImage(sadFarmer);
        }
        else if (selectedID == 11)
        {
            choiceText.text = "GREAT CHOICE!";
            feedbackText.text = "Mixed rice is beneficial as it offers affordability, variety, and nutritional balance while allowing for portion control and customization to personal preferences, making it a convenient and cost-effective meal option.";
            LoadImage(happyFarmer);
        }
        else if (selectedID == 12)
        {
            choiceText.text = "BAD CHOICE!";
            feedbackText.text = "Snacks might be affordable but lack the substance of a full meal. Though snacks can be filling in the short term, they are lacking in nutrients and may leave students hungry later.";
            LoadImage(sadFarmer);
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
}*/

public class FeedbackManagerLevel2 : MonoBehaviour
{
    public TextMeshProUGUI feedbackText;
    public TextMeshProUGUI choiceText;
    public UnityEngine.UI.Image farmerImage;

    public string happyFarmer = "farmer_smart";
    public string sadFarmer = "farmer_sad";

    private Dictionary<int, Rule> knowledgeBase;

    void Start()
    {
        // Initialize the knowledge base
        knowledgeBase = new Dictionary<int, Rule>
        {
            { 4, new Rule("GREAT CHOICE!", "Pickup trucks are ideal for small-scale farmers in Malaysia due to their moderate cargo capacity, ability to handle rural roads, affordability, and accessibility on narrow farm paths and local markets.", happyFarmer) },
            { 5, new Rule("BAD CHOICE!", "While a car is suitable for urban driving, it lacks the cargo capacity to handle a medium-scale harvest. A car would likely require multiple trips, making it inefficient for transporting crops.", sadFarmer) },
            { 6, new Rule("BAD CHOICE!", "A lorry has the capacity for larger loads but may be excessive for a medium-scale harvest, particularly over short distances. However, lorries would be more efficient for long-distance transport or very large-scale operations.", sadFarmer) },
            { 7, new Rule("GREAT CHOICE!", "Wholesale markets not only offer lower prices to consumers but also provide a wider variety of food supplies in larger quantities.", happyFarmer) },
            { 8, new Rule("BAD CHOICE!", "While conveniently located near UPM, Putra Mart is a smaller-scale market catering, with limited stock available for bulk purchases.", sadFarmer) },
            { 9, new Rule("BAD CHOICE!", "Pasar Tani offers fresh, locally sourced crops. However, the availability of large quantities may be limited and it is better for smaller - scale or regular purchases.", sadFarmer)},
            { 10, new Rule("BAD CHOICE", "Western meals can be more expensive, often approaching the upper limit of the budget or exceeding it. While they can be filling, they might offer less variety in nutrients.", sadFarmer)},
            { 11, new Rule ("GREAT CHOICE", "Mixed rice is beneficial as it offers affordability, variety, and nutritional balance while allowing for portion control and customization to personal preferences, making it a convenient and cost-effective meal option.", happyFarmer) },
            { 12, new Rule("BAD CHOICE", "Snacks might be affordable but lack the substance of a full meal. Though snacks can be filling in the short term, they are lacking in nutrients and may leave students hungry later.", sadFarmer) }
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

    public void OnContinueButton()
    {
        int selectedID = PlayerPrefs.GetInt("SelectedId", -1);

        if (selectedID == 4 || selectedID == 5 || selectedID == 6)
        {
            SceneManager.LoadScene("level2badge1");
        }
        else if (selectedID == 7 || selectedID == 8 || selectedID == 9)
        {
            SceneManager.LoadScene("level2badge2");
        }
        else if (selectedID == 10 || selectedID == 11 || selectedID == 12)
        {
            SceneManager.LoadScene("level2badge3");
        }
        else
        {
            Debug.LogError("Unexpected SelectedId: " + selectedID);
        }
    }

}



