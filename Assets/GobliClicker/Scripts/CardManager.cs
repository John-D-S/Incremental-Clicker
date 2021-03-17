using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//this manages both the shop and the active cards
public class CardManager : MonoBehaviour
{
    [SerializeField]
    private GameObject Shop;
    [SerializeField]
    private GameObject ActiveCards;

    private List<CardComponent> shopCards = new List<CardComponent>();
    private List<CardComponent> activeCards = new List<CardComponent>();

    // Start is called before the first frame update
    void Start()
    {
            
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
