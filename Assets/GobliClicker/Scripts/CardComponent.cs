using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class CardComponent : MonoBehaviour
{
    [System.NonSerialized]
    public Card card;

    public int level = 1;

    [SerializeField]
    private TextMeshProUGUI cardName;
    [SerializeField]
    private TextMeshProUGUI cardDescription;
    public TextMeshProUGUI cardCost;
    public Image ProgressBar;

    public void BuySelf()
    {
        CardManager.AttemptBuyCard(gameObject);
    }

    public void RandomizeCard(int _level)
    {
        Debug.Log("this card should be randomized");
        level = _level;
        card = new Card(level);
        ResetGui();
    }

    void ResetGui()
    {
        cardName.text = card.CardName;
        cardDescription.text = card.CardDescription;
        if (cardCost.IsActive())
        {
            cardCost.text = card.PurchaseCostWords;
        }
    }

    private void Start()
    {
        if (card == null)
        {
            card = new Card(level);
        }
        ResetGui();
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        card.UpdateCard();
    }

    void Update()
    {
        if (ProgressBar.IsActive())
        {
            ProgressBar.fillAmount = card.timer / card.TickTime;
        }
    }
}
