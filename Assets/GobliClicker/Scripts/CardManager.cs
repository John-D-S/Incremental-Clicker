using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

/// <summary>
/// This manages both the shop and the active cards.
/// </summary>
public class CardManager : MonoBehaviour
{
    // the prefab that will be instantiated in the shop when you buy a new card slot in the shop
    [SerializeField]
    private GameObject CardPrefab;

    // Can specify wether you want a prefab or an instance from the scene? How would you do that?

    // the content panel of the active card holder scroll rectangle.
    [SerializeField] 
    private RectTransform activeCardHolder;
    // the content panel of the shop card holder scroll rectangle.
    [SerializeField]
    private RectTransform shopCardHolder;

    // The cost in Gobli to add a new card to the shop
    private static int newShopCost = 10;
    // what level the next card that is added to the shop will be
    private static int newShopLevel = 1;
    // teh epic cost in Gobli to regenerate the cards in the shop.
    private static int regenerateShopCost = 50;

    // the TMPUGUI that displays the cost of adding a new card to the shop
    [SerializeField]
    private TextMeshProUGUI newShopCostText;
    // the TMPUGUI that displays the cost of regenerating all the cards in the shop
    [SerializeField]
    private TextMeshProUGUI regenerateShopCostText;

    // lists of all the shop/active cards
    private static List<CardComponent> shopCards = new List<CardComponent>();
    private static List<CardComponent> activeCards = new List<CardComponent>();

    // this exists so that I can reference serialized fields in static methods. This might be silly.
    private static CardManager mainCardManager; 

    private void Start()
    {
        // sets the mainCardManager (there is only one card manager)
        mainCardManager = gameObject.GetComponent<CardManager>();
        // setting the GUI on the shop buttons to display the cost of a new card and the cost of regenerating all the current cards.
        newShopCostText.text = $"{newShopCost} Gobli";
        regenerateShopCostText.text = $"{regenerateShopCost} Gobli";
    }

    /// <summary>
    /// If cardGameObjectToBuy is in the shop, check if the player can afford it. if the player can afford it, instantiate a copy of it in the active cards panel, set the copy to active and randomize the original with + 1 level
    /// </summary>
    public static void AttemptBuyCard(GameObject cardGameObjectToBuy)
    {

        //you can only buy a card if it is in the shop.
        if (cardGameObjectToBuy.transform.parent.tag == "Shop")
        {
            bool canBuyCard = true;
            //set the component of the card the player is trying to buy
            CardComponent cardToBuy = cardGameObjectToBuy.GetComponent<CardComponent>();
            //for each price in the card the player is trying to buy, check if you can afford it, if any one cost is too much, you can't afford the card at all  
            foreach (KeyValuePair<Resource, int> cost in cardToBuy.card.purchaseCost)
            {
                if (Counter.counter[cost.Key] < cost.Value)
                {
                    canBuyCard = false;
                }
            }
            //if you can afford the card
            if (canBuyCard)
            {   
                // subtract the cost from what you have
                foreach (KeyValuePair<Resource, int> cost in cardToBuy.card.purchaseCost)
                {
                    Counter.counter[cost.Key] -= cost.Value;
                }
                // saving the card's level to use later
                int cardLevel = cardToBuy.card.cardLevel;
                
                //instantiating the card int the activeCardHolder
                Instantiate(cardGameObjectToBuy, mainCardManager.activeCardHolder);
                //setting boughtCard to the card you just instantiated so that you can set some of its properties
                CardComponent boughtCard = mainCardManager.activeCardHolder.GetChild(mainCardManager.activeCardHolder.childCount - 1).GetComponent<CardComponent>();
                boughtCard.card = cardGameObjectToBuy.GetComponent<CardComponent>().card;
                boughtCard.card.activated = true;
                boughtCard.cardCost.enabled = false;
                //randomize the shop card you just bought, adding 1 to its level
                cardToBuy.RandomizeCard(cardLevel + 1);
            }
        }
    }

    /// <summary>
    /// Regenerates all the cards in the shop for the amount of Gobli in RegenerateShopCost
    /// </summary>
    public static void RefreshShopCards()
    {
        //if the player can afford it
        if (Counter.counter[Resource.Gobli] >= regenerateShopCost)
        {
            // Regenerate all the cards with cards of the same level.
            foreach (CardComponent cardComponent in shopCards)
            {
                int level = cardComponent.level;
                cardComponent.RandomizeCard(level);
            }
            // take away the cost of regenerating the cards
            Counter.counter[Resource.Gobli] -= regenerateShopCost;
            // calculate and set the new cost for regenerating the shop cards
            regenerateShopCost = Mathf.RoundToInt(regenerateShopCost * 1.5f);
            // update the text to display the correct regenerate cost.
            mainCardManager.regenerateShopCostText.text = $"{regenerateShopCost} Gobli";
        }

    }

    /// <summary>
    /// adds another card to the shopCardHolder if the player can afford it
    /// </summary>
    public static void AddNewCardForSale()
    {
        //check if the player can afford another card int hte shop card holder.
        if (Counter.counter[Resource.Gobli] >= newShopCost)
        {
            //add the card to the shopCardH
            Instantiate(mainCardManager.CardPrefab, mainCardManager.shopCardHolder);
            //sets the new card as the second last in the hierarchy to keep the new card and refresh cards buttons at the end of the shop.
            Transform newCard = mainCardManager.shopCardHolder.GetChild(mainCardManager.shopCardHolder.childCount - 1);
            //set the newCardComponent to the card component of the newCard
            CardComponent newCardComponent = newCard.GetComponentInChildren<CardComponent>();
            //add the newCardComponent to the list of CardComponents in shopCards
            shopCards.Add(newCardComponent);
            //randomize the card with the newShopLevel
            newCardComponent.RandomizeCard(newShopLevel);
            //add 1 to newShopLevel so the next time a AddNewCardForSale() (this function) is called, the new card will be 1 level higher.
            newShopLevel++;
            //take away the cost of newShopCost
            Counter.counter[Resource.Gobli] -= newShopCost;
            //multiply newShopCost by 2 so that it rises exponentially
            newShopCost *= 2;
            //refresh the text displaying the cost of adding a new card to the shop with correct information.
            mainCardManager.newShopCostText.text = $"{newShopCost} Gobli";
        }
    }
}
