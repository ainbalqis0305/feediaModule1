using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public sealed class ScoreCounter : MonoBehaviour
{
    public static ScoreCounter Instance { get; private set; }

    private int _score;

    public GameObject popupPanel;

    private void Start()
    {
        StartCoroutine(ShowPopupPanel());
    }

    IEnumerator ShowPopupPanel()
    {
        if (popupPanel != null)
        {
            popupPanel.SetActive(true);
            yield return new WaitForSeconds(2f);
        }
    }

    private void Update()
    {
        if (popupPanel != null && popupPanel.activeSelf && Input.GetMouseButtonDown(0))
        {
            popupPanel.SetActive(false);
        }
    }

    public int Score
    {
        get => _score;

        set
        {
            if (_score == value) return; 

            _score = value;

            scoreText.SetText($"Score = {_score}");

            if (_score > 100)
            {
                MoveToNextScene();
            }
           
        }
    }

    [SerializeField] private TextMeshProUGUI scoreText;

    private void Awake() => Instance = this;

    private void MoveToNextScene()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("powerups");
    }

}
