using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class EndingController : MonoBehaviour
{
    public TextMeshProUGUI finalScoreText;

    void Start()
    {
        float finalScore = PlayerPrefs.GetFloat("FinalScore", 0); 
        finalScoreText.text = "Total Distance: " + Mathf.Floor(finalScore).ToString();
    }
   
    private void Update()
    {
        if (Keyboard.current.enterKey.wasPressedThisFrame)
        {
            RestartGame();
        }
    }

    public void RestartGame()
    {
        SceneManager.LoadScene("Gameplay");
    }
}
