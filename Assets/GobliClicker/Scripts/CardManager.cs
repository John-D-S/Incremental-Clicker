using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

//this manages both the shop and the active cards
public class CardManager : MonoBehaviour
{
    [SerializeField]
    private GameObject CardPrefab;

    [SerializeField] // ask if you can specify wether you want a prefab or an instance from the scene.
    private RectTransform activeCardHolder;
    [SerializeField]
    private RectTransform shopCardHolder;

    private static int newShopCost = 10;
    private static int newShopLevel = 1;
    private static int regenerateShopCost = 50;
    [SerializeField]
    private TextMeshProUGUI newShopCostText;
    [SerializeField]
    private TextMeshProUGUI regenerateShopCostText;

    private static List<CardComponent> shopCards = new List<CardComponent>();
    private static List<CardComponent> activeCards = new List<CardComponent>();

    private static CardManager mainCardManager; //this exists so that I can reference serialized fields in static methods

    private void Start()
    {
        mainCardManager = gameObject.GetComponent<CardManager>();
        newShopCostText.text = $"{newShopCost} Gobli";
        regenerateShopCostText.text = $"{regenerateShopCost} Gobli";
    }

    public static void AttemptBuyCard(GameObject cardGameObjectToBuy)
    {
        if (cardGameObjectToBuy.transform.parent.tag == "Shop")//you can only buy a card if it is in the shop.
        {
            bool canBuyCard = true;
            CardComponent cardToBuy = cardGameObjectToBuy.GetComponent<CardComponent>();
            foreach (KeyValuePair<Resource, int> cost in cardToBuy.card.purchaseCost)
            {
                if (Counter.counter[cost.Key] < cost.Value)
                {
                    canBuyCard = false;
                }
            }
            if (canBuyCard)
            {
                foreach (KeyValuePair<Resource, int> cost in cardToBuy.card.purchaseCost)
                {
                    Counter.counter[cost.Key] -= cost.Value;
                }
                int cardLevel = cardToBuy.card.cardLevel;
                
                Instantiate(cardGameObjectToBuy, mainCardManager.activeCardHolder);
                CardComponent boughtCard = mainCardManager.activeCardHolder.GetChild(mainCardManager.activeCardHolder.childCount - 1).GetComponent<CardComponent>();
                boughtCard.card = cardGameObjectToBuy.GetComponent<CardComponent>().card;
                boughtCard.card.activated = true;
                boughtCard.cardCost.enabled = false;
                cardToBuy.RandomizeCard(cardLevel + 1);//Setting this will randomize the card.
            }
        }
    }

    public static void RefreshShopCards()
    {
        if (Counter.counter[Resource.Gobli] >= regenerateShopCost)
        {
            foreach (CardComponent cardComponent in shopCards)
            {
                int level = cardComponent.level;
                cardComponent.RandomizeCard(level);
            }
            Counter.counter[Resource.Gobli] -= regenerateShopCost;
            regenerateShopCost = Mathf.RoundToInt(regenerateShopCost * 1.5f);
            mainCardManager.regenerateShopCostText.text = $"{regenerateShopCost} Gobli";
        }

    }

    public static void AddNewCardForSale()
    {
        if (Counter.counter[Resource.Gobli] >= newShopCost)
        {
            Instantiate(mainCardManager.CardPrefab, mainCardManager.shopCardHolder);
            //sets the new card as the second last in the hierarchy to keep the new card and refresh cards buttons at the end of the shop.
            Transform newCard = mainCardManager.shopCardHolder.GetChild(mainCardManager.shopCardHolder.childCount - 1);
            newCard.SetSiblingIndex(newCard.transform.parent.hierarchyCount - 2);
            CardComponent newCardComponent = newCard.GetComponentInChildren<CardComponent>();
            shopCards.Add(newCardComponent);
            newCardComponent.RandomizeCard(newShopLevel);
            newShopLevel++;
            Counter.counter[Resource.Gobli] -= newShopCost;
            newShopCost *= 2;
            mainCardManager.newShopCostText.text = $"{newShopCost} Gobli";
        }
    }
}
