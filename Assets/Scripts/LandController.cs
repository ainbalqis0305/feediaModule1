using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LandController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Script land controller has just started");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void gotoInsertName()
    {
        SceneManager.LoadScene(SceneData.insertName);
    }
}
