using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [Header("Hearts UI")]
    public Image[] heartImages;
    public Sprite fullHeart;
    public Sprite emptyHeart;

    [Header("Coins UI")]
    public TMP_Text coinText;
    private int totalCoins = 0;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }


    public void UpdateHearts(int currentHealth)
{
    if (heartImages == null) return;

    for (int i = 0; i < heartImages.Length; i++)
    {
        if (heartImages[i] != null)
        {
            if (i < currentHealth)
            {
                heartImages[i].sprite = fullHeart;
            }
            else
            {
                heartImages[i].sprite = emptyHeart;
            }
        }
    }
}

    public void AddCoin(int amount)
    {
        totalCoins += amount;
        UpdateCoinUI();
    }

    private void UpdateCoinUI()
    {
        if (coinText != null)
        {
            coinText.text = "x " + totalCoins;
        }
    }
}