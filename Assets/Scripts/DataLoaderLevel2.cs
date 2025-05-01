using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager.Requests;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SocialPlatforms;
using UnityEngine.UI;
using TMPro;

public class DataLoaderLevel2 : MonoBehaviour
{
    public TextMeshProUGUI desc;
    public Image displayImage;
    public GameObject dataPanel;
    //Store fetched data
    private Dictionary<int, string> dataDictionary = new Dictionary<int, string>(); // Store fetched data
    // -1 indicates no selection
    private int selectedId = -1;


    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(GetRequest("http://localhost/feedia/ItemData.php"));
        displayImage.enabled = false; //Hide the image initially
        dataPanel.SetActive(false);
    }

    IEnumerator GetRequest(string uri)
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(uri))
        {

            // Send the request and wait for a response
            yield return webRequest.SendWebRequest();

            string[] pages = uri.Split('/');
            int page = pages.Length - 1;

            switch (webRequest.result)
            {
                case UnityWebRequest.Result.ConnectionError:
                case UnityWebRequest.Result.DataProcessingError:
                    Debug.LogError(pages[page] + ": Error: " + webRequest.error);
                    break;
                case UnityWebRequest.Result.ProtocolError:
                    Debug.LogError(pages[page] + ": HTTP Error: " + webRequest.error);
                    break;
                case UnityWebRequest.Result.Success:

                    string rawresponse = webRequest.downloadHandler.text;
                    string[] decisions = rawresponse.Split('*');

                    dataDictionary.Clear(); // Clear old data
                    for (int i = 0; i < decisions.Length; i++)
                    {
                        if (!string.IsNullOrEmpty(decisions[i]))
                        {
                            string[] decisionInfo = decisions[i].Split('|');
                            int id = int.Parse(decisionInfo[0]);
                            string description = decisionInfo[1];
                            dataDictionary[id] = description;
                        }
                    }
                    Debug.Log("Data loaded successfully!");
                    break;
                default:
                    Debug.LogError("Error fetching data: " + webRequest.error);
                    break;
            }
        }
    }

    public void DisplayData(int id)
    {
        if (dataDictionary.ContainsKey(id))
        {
            selectedId = id;
            desc.text = dataDictionary[id];

            //Map id to the corresponsing image name
            string imageName = "";
            switch (id)
            {
                case 4:
                    imageName = "pickup_truck";
                    break;
                case 5:
                    imageName = "car";
                    break;
                case 6:
                    imageName = "lorry";
                    break;
                case 7:
                    imageName = "wholesale_market";
                    break;
                case 8:
                    imageName = "pasar_tani";
                    break;
                case 9:
                    imageName = "putra_mart";
                    break;
                case 10:
                    imageName = "western";
                    break;
                case 11:
                    imageName = "mixed_rice";
                    break;
                case 12:
                    imageName = "snacks";
                    break;

                default:
                    Debug.LogError("No image mapped for id: " + id);
                    displayImage.enabled = false;
                    dataPanel.SetActive(false);
                    return;
            }

            ShowImage(imageName);
            dataPanel.SetActive(true);
        }
        else
        {
            desc.text = "No data found";
            displayImage.enabled = false;
            dataPanel.SetActive(false);
        }
    }

    private void ShowImage(string imageName)
    {
        //Load the image from the Resources folder
        Sprite sprite = Resources.Load<Sprite>(imageName);

        if (sprite != null)
        {
            displayImage.sprite = sprite;
            displayImage.enabled = true;
        }
        else
        {
            Debug.LogError("Image not found in Resources folder: " + imageName);
            displayImage.enabled = false;
        }
    }

    public void ConfirmSelection()
    {
        if (selectedId != -1)
        {
            PlayerPrefs.SetInt("SelectedId", selectedId);
            UnityEngine.SceneManagement.SceneManager.LoadScene("level2feedback");
        }
        else
        {
            Debug.LogError("No selection made. Please choose an option before confirming.");
        }
    }


}
