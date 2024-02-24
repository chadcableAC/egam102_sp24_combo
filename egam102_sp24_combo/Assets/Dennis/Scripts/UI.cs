using UnityEngine;
using TMPro;

public class UIController : MonoBehaviour
{
    public TMP_Text attemptsText;
    public TMP_Text sumText;
    public TMP_Text randomNumbersText;

    private LotteryGame lotteryGame;

    private void Awake()
    {
        lotteryGame = FindObjectOfType<LotteryGame>();
    }

    private void Update()
    {
        attemptsText.text = "Chances: " + lotteryGame.AttemptsLeft.ToString();
        sumText.text = "Current Sum: " + lotteryGame.CurrentSum.ToString();
        UpdateRandomNumbersText();
    }

    private void UpdateRandomNumbersText()
    {
        string numbersText = "Numbers you got: ";

        foreach (int number in lotteryGame.GetNumbers())
        {
            numbersText += number.ToString() + " ";
        }

        randomNumbersText.text = numbersText;
    }
}