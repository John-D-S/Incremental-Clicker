using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonScript : MonoBehaviour
{
    /// <summary>
    /// Called by the button in the card shop which adds a new purchasable card.
    /// </summary>
    public void AddNewShopCard()
    {
        CardManager.AddNewCardForSale();
    }

    /// <summary>
    /// called by the button in the card shop which randomizes all the shop cards.
    /// </summary>
    public void RefreshShopCards()
    {
        CardManager.RefreshShopCards();
    }
}
