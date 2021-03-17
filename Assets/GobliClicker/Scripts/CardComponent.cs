using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class CardComponent : MonoBehaviour
{
    [System.NonSerialized]
    public Card card;
    private int level;
    public int Level
    {
        get
        {
            return level;
        }
        set
        {
            card = new Card(value);
            level = value;
        }
    }

    [SerializeField]
    private TextMeshProUGUI cardName;
    [SerializeField]
    private TextMeshProUGUI cardDescription;
    public TextMeshProUGUI cardCost;


    // Update is called once per frame
    void Update()
    {
        card.UpdateCard();
    }
}
