using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonScript : MonoBehaviour
{
    public void AddNewShopCard()
    {
        CardManager.AddNewCardForSale();
    }

    public void RefreshShopCards()
    {
        CardManager.RefreshShopCards();
    }
}
