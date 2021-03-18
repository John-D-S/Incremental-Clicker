using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//this manages both the shop and the active cards
public class CardManager : MonoBehaviour
{
    [SerializeField] // ask if you can specify wether you want a prefab or an instance from the scene.
    private Transform activeCardHolder;
    [SerializeField]
    private Transform shopCardHolder;

    private List<CardComponent> shopCards = new List<CardComponent>();
    private List<CardComponent> activeCards = new List<CardComponent>();

    //
    public void AttemptBuyCard(GameObject cardGameObjectToBuy)
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
                int cardLevel = cardToBuy.Level;
                GameObject boughtCard = Instantiate<GameObject>(cardGameObjectToBuy, activeCardHolder);
                boughtCard.GetComponent<CardComponent>().card.activated = true;
                boughtCard.GetComponent<CardComponent>().cardCost.enabled = false;
                cardToBuy.Level = cardLevel;//Setting this will randomize the card.
            }
        }
    }

    public void AddNewCardForSale()
    {

    }

    public void RegenerateCardsForSale()
    {

    }

    // Start is called before the first frame update
    void Start()
    {
            
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
