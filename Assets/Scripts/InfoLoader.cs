using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEditor.Build.Content;

[System.Serializable]
public class FoodSecurityData
{
    public List<string> info;
}
public class InfoLoader : MonoBehaviour
{
    private FoodSecurityData data;

    private void Awake()
    {
        LoadInfo();
    }

    private void LoadInfo()
    {
        TextAsset jsonFile = Resources.Load<TextAsset>("FoodSecurityInfo");
        if (jsonFile != null)
        {
            data = JsonUtility.FromJson<FoodSecurityData>(jsonFile.text);
        }
        else
        {
            Debug.LogError("JSON file not found!");
        }
    }
    
    public string GetRandomInfo()
    {
        if (data != null && data.info.Count > 0)
        {
            int randomIndex = Random.Range(0, data.info.Count);
            return data.info[randomIndex];
        }
        return "No information available,";
    }
}
