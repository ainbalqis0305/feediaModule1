using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Level1Dialogue : MonoBehaviour
{
    public TextMeshProUGUI textComponent;
    public string[] lines;
    public float textSpeed;

    public AudioSource audioSource;
    public AudioClip[] audioClips;


    private int index;
    void Start()
    {
        textComponent.text = string.Empty;
        StartDialogue();
    }
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            NextLine();
        } /*else
        {
            StopAllCoroutines();
            textComponent.text = lines[index];
        }*/
    }

    

    void StartDialogue()
    {
        index = 0;
        StartCoroutine(TypeLine());
        PlayAudioForLine(index);
    }

    IEnumerator TypeLine()
    {
        foreach (char c in lines[index].ToCharArray())
        {
            textComponent.text += c;
            yield return new WaitForSeconds(textSpeed);
        }
        Debug.Log("Finished typing: " + lines[index]);
    }

    void NextLine()
    {
        if (index < lines.Length - 1)
        {
            index++;
            textComponent.text = string.Empty;
            StartCoroutine(TypeLine());
        }
        else
        {
            gameObject.SetActive(false);
        }
    }

    void PlayAudioForLine(int lineIndex)
    {
        if (audioClips != null && lineIndex < audioClips.Length)
        {


        }
        else
        {
            Debug.LogWarning("No audio clip found for line index: + " + lineIndex);
        }
    }
}
