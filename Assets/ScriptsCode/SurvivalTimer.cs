using UnityEngine;
using TMPro;

public class SurvivalTimer : MonoBehaviour
{
    public float goalTime = 60f;
    public TextMeshProUGUI timerText;
    public GameObject gateObject;

    private float timer;
    private bool won = false;

    void Update()
    {
        if (won) return;

        timer += Time.deltaTime;

        float remainingTime = Mathf.Max(0, goalTime - timer);

        if (timerText != null)
        {
            timerText.text = "Time: " + Mathf.CeilToInt(remainingTime) + "s";
        }

        if (timer >= goalTime)
        {
            won = true;
            if (timerText != null)
            {
                timerText.text = "Gate Opened!";
            }

            if (gateObject != null)
            {
                gateObject.SetActive(true);
            }
        }
    }
}