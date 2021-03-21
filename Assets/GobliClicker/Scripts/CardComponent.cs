using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// The class which is attatched to the card prefab. it deals with all the UI stuff with the card.
/// </summary>
public class CardComponent : MonoBehaviour
{
    //the card that holds and deals with the counter for this card component
    [System.NonSerialized]
    public Card card;

    //the level of this card
    public int level = 1;

    //the TMPUGUI displaying the card's name
    [SerializeField]
    private TextMeshProUGUI cardName;
    //the TMPUGUI displaying the card's description
    [SerializeField]
    private TextMeshProUGUI cardDescription;
    //the TMPUGUI displaying how much the card costs when it is in the shop
    public TextMeshProUGUI cardCost;
    //the Image Displaying the tick progress of the card when it is in the active cards bar
    public Image ProgressBar;

    /// <summary>
    /// is called by the card's buy button when it is in the card shop.
    /// </summary>
    public void BuySelf()
    {
        CardManager.AttemptBuyCard(gameObject);
    }

    /// <summary>
    /// randomizes the card's function and data by calling its constructor with the given level. 
    /// </summary>
    public void RandomizeCard(int _level)
    {
        level = _level;
        card = new Card(level);
        ResetGui();
    }

    /// <summary>
    /// Resets the GUI on the card.
    /// </summary>
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
        //Sets card to a new card if it doesn't already exist and then resets the GUI
        if (card == null)
        {
            card = new Card(level);
        }
        ResetGui();
    }

    private void FixedUpdate()
    {
        card.UpdateCard();
    }

    void Update()
    {
        // if the card is in the active column, set the progressbar's fill amount to be card.timer / card.TickTime so that it fills up like a little clock :)
        if (ProgressBar.IsActive())
        {
            ProgressBar.fillAmount = card.timer / card.TickTime;
        }
    }
}
