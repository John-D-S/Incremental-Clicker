using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CardFunction
{
    AddResource,
    ConvertResources
}

/// <summary>
/// The class that contains all the data for a given card and performs the functions on the resources.
/// </summary>
public class Card
{
    //the card is inactive before being purchased
    public bool activated = false;
    
    //the resources and the number of them that it costs to purchase this card.
    public Dictionary<Resource, int> purchaseCost = new Dictionary<Resource, int>();
    //The cost of the card as it will be displayed
    public string PurchaseCostWords { get; }
    //the name of the card as it will be displayed.
    public string CardName { get; }
    //the card's description as it will be displayed.
    public string CardDescription { get; } = "";

    //this determines what this card does
    private CardFunction cardFunction;
    //the level of the card. Higher levels do more but also cost more.
    public int cardLevel = 1;

    //the variable that ticks up until it reaches TickTime
    public float timer;
    //time to complete the cardFunction
    public float TickTime { get; }

    //variables for addResources()
    private KeyValuePair<Resource, int> resourceAddOutput;

    //variables for ConvertResources()
    private Dictionary<Resource, int> resourceConvertInput;
    private KeyValuePair<Resource, int> resourceConvertOutput;

    /// <summary>
    /// The constructor randomizes the data with the given level when it is called.
    /// </summary>
    /// <param name="_cardLevel"></param>
    public Card(int _cardLevel)
    {
        
        //setting the card's cardLevel
        cardLevel = _cardLevel;
        
        //setting the purchaseCost of the card.
        //the minimum number of resources in the price of the card is 1, and the max is 3
        int noOfResourcesRequiredToPurchase = Random.Range(1, 4);
        for (int i = 0; i < noOfResourcesRequiredToPurchase; i++)
        {
            //gobli will always be part of the cost of the card.
            if (i == 0)
            {
                //this balances the price of the card with its level
                purchaseCost.Add((Resource)0, Mathf.RoundToInt(Power(2f, (float)cardLevel * 0.6f) * Random.Range(5, 10)));
            }
            else
            {
                //the 1 at the beginning of random.range is to exclude Gobli as a resource, since it has already been added and the - 1 at the end is to exclude None
                Resource randomResource = (Resource)Random.Range(1, Resource.GetNames(typeof(Resource)).Length - 1);
                //this if statement is to ensure that no more than one of a given resource is added to the cost
                if (!purchaseCost.ContainsKey(randomResource))
                {
                    purchaseCost.Add(randomResource, Mathf.RoundToInt(Power(2f, (float)cardLevel * 0.5f) * Random.Range(4, 9)));
                }
            }
        }

        //setting the card's function
        //the chance that the card function will be add goes down as the level goes up such that it is more likely to get a convert card past level 5 than an add card.
        float cardFunctionAddChance = Mathf.Pow(2f, -(float)cardLevel * 0.4f);
        if (Random.Range(0f, 1f) > cardFunctionAddChance)
        {
            cardFunction = CardFunction.ConvertResources;
        }
        else
        {
            cardFunction = CardFunction.AddResource;
        }

        //setting the time it takes to complete one tick of the card's function.
        TickTime = Random.Range(25f, 50f) / (cardLevel + 1) / 2f + 0.05f;

        //setting the values for the card's functions
        //setting the output resource for the addResource function
        resourceAddOutput = new KeyValuePair<Resource, int>((Resource)Random.Range(1, Resource.GetNames(typeof(Resource)).Length - 1), cardLevel + Random.Range(0, 3));//you cannot add Gobli; you can only convert to it
        
        //ResourceConvert function
        resourceConvertInput = new Dictionary<Resource, int>();
        int resourceConvertInputNumber = Random.Range(1, 4);//the number of resources going in
        for (int i = 0; i < resourceConvertInputNumber; i++)
        {
            KeyValuePair<Resource, int> resourceToAdd = new KeyValuePair<Resource, int>((Resource)Random.Range(1, Resource.GetNames(typeof(Resource)).Length - 1), cardLevel + Mathf.RoundToInt(Random.Range(0, cardLevel * 0.25f)));//gobli cannot be an input
            if (!resourceConvertInput.ContainsKey(resourceToAdd.Key))
            {
                resourceConvertInput.Add(resourceToAdd.Key, resourceToAdd.Value);
            }
        }
        //there is a 1 in 3 chance that the convert output will be gobli
        bool convertsToGobli = (Random.Range(0, 3) == 0);
        if (convertsToGobli)
        {
            resourceConvertOutput = new KeyValuePair<Resource, int>(Resource.Gobli, resourceConvertInputNumber * (cardLevel + Mathf.RoundToInt(Random.Range(0.25f, cardLevel))));
        }
        else
        {
            resourceConvertOutput = new KeyValuePair<Resource, int>((Resource)Random.Range(1, Resource.GetNames(typeof(Resource)).Length - 1), resourceConvertInputNumber * (cardLevel + Mathf.RoundToInt(Random.Range(0.25f, cardLevel))));
        }

        //setting the name
        switch (cardFunction)
        {
            case CardFunction.AddResource:
                CardName = $"Add {resourceAddOutput.Key}";
                break;
            case CardFunction.ConvertResources:
                CardName = $"Convert to {resourceConvertOutput.Key}";
                break;
            default:
                break;
        }

        //setting the description
        switch (cardFunction)
        {
            case CardFunction.AddResource:
                CardDescription = $"Adds {resourceAddOutput.Value} {resourceAddOutput.Key} every {Mathf.RoundToInt(TickTime * 10) * 0.1f} seconds.";
                break;
            case CardFunction.ConvertResources:
                int i = 0;
                CardDescription = "Converts ";
                foreach (KeyValuePair<Resource, int> InputItem in resourceConvertInput)
                {
                    if (i == 0)
                    {
                        CardDescription += $"{InputItem.Value} {InputItem.Key}";
                    }
                    else if (i < resourceConvertInput.Count - 1)
                    {
                        CardDescription += $", {InputItem.Value} {InputItem.Key}";
                    }
                    else if (i == resourceConvertInput.Count - 1)
                    {
                        CardDescription += $" and {InputItem.Value} {InputItem.Key}";
                    }
                    i++;
                }
                CardDescription += $" into {resourceConvertOutput.Value} {resourceConvertOutput.Key} every {Mathf.RoundToInt(TickTime * 10) * 0.1f} seconds.";
                break;
        }

        //setting the worded cost of the card to purchase
        int resourceNumber = 0;
        foreach (KeyValuePair<Resource, int> resourceCost in purchaseCost)
        {
            if (resourceNumber > 0)
            {
                PurchaseCostWords += ", ";
            }
            PurchaseCostWords += $"{resourceCost.Value} {resourceCost.Key}";
            resourceNumber++;
        }
    }

    /// <summary>
    /// I didn't know that Mathf.Pow existed.
    /// </summary>
    private float Power(float value, float power)
    {
        if (power <= 1)
        {
            return value;
        }
        else
        {
            return Power(value, power - 1, value * value);
        }
    }
    private float Power(float originalValue, float power, float currentValue)
    {
        if (power <= 1)
        {
            return currentValue;
        }
        else
        {
            return Power(originalValue, power - 1, currentValue * originalValue);
        }
    }

    /// <summary>
    /// The function for if the cardFunction is CardFunction.AddResources
    /// </summary>
    private void AddResources()
    {
        Counter.AddResource(resourceAddOutput.Key, resourceAddOutput.Value);
    }
    
    /// <summary>
    /// The function for if the cardFunction is CardFunction.ConvertResources
    /// </summary>
    private void ConvertResources()
    {
        bool canConvert = true;
        foreach (KeyValuePair<Resource, int> resourceInput in resourceConvertInput)
        {
            if (resourceInput.Value > Counter.counter[resourceInput.Key])//if the resources required to convert is greater than the amount of that resource owned, you cannot convert.
            {
                canConvert = false;
            }
        }
        if (canConvert)
        {
            foreach (KeyValuePair<Resource, int> resourceInput in resourceConvertInput)
            {
                Counter.AddResource(resourceInput.Key, -resourceInput.Value);
            }
            Counter.AddResource(resourceConvertOutput.Key, resourceConvertOutput.Value);
        }
        
    }

    /// <summary>
    /// Tells the card to do it's thing.
    /// </summary>
    private void PerformFunciton()
    {
        switch (cardFunction)
        {
            case CardFunction.AddResource:
                AddResources();
                break;
            case CardFunction.ConvertResources:
                ConvertResources();
                break;
        }
    }

    // Update is called once per frame
    public void UpdateCard()
    {
        //if the card has been purchased, the timer ticks up and the card performs its function each time the timer reaches TickTime.
        if (activated)
        {
            timer += Time.fixedDeltaTime;
            if (timer >= TickTime)
            {
                PerformFunciton();
                timer = 0;
            }
        }

    }
}
