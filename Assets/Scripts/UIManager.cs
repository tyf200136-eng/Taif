using UnityEngine;
using TMPro; // ضروري للتعامل مع نصوص TextMeshPro

public class UIManager : MonoBehaviour
{
    public TextMeshProUGUI scoreText; // خانة لنسحب فيها نص النتيجة
    public PlayerScore player;       // خانة لنسحب فيها اللاعب

    void Update()
    {
        if (player != null && scoreText != null)
        {
            // تحديث النص الموجود على الشاشة ليطابق نتيجة اللاعب
            scoreText.text = "Score: " + player.score;
        }
    }
}