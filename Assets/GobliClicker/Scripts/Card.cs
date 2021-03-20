using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CardFunction
{
    AddResource,
    ConvertResources
}

public class Card
{    
    public bool activated = false;//the card is inactive before being purchased
    public Dictionary<Resource, int> purchaseCost = new Dictionary<Resource, int>(); //the resources and the number of them that it costs to purchase this card.
    public string PurchaseCostWords { get; }
    
    public int cardLevel = 1;

    public string CardName { get; }
    public string CardDescription { get; } = "";

    private CardFunction cardFunction;//this describes what this card does

    public float timer;
    public float TickTime { get; }//time to complete the cardFunction

    //variables for addResources()
    private KeyValuePair<Resource, int> resourceAddOutput;

    //variables for ConvertResources()
    private Dictionary<Resource, int> resourceConvertInput;
    private KeyValuePair<Resource, int> resourceConvertOutput;

    public Card(int _cardLevel)
    {
        
        //setting the card's cardLevel
        cardLevel = _cardLevel;
        
        //setting the purchaseCost of the card.
        int noOfResourcesRequiredToPurchase = Random.Range(1, 4);
        for (int i = 0; i < noOfResourcesRequiredToPurchase; i++)
        {
            if (i == 0)
            {
                purchaseCost.Add((Resource)0, Mathf.RoundToInt(Power(2f, (float)cardLevel * 0.6f) * Random.Range(5, 10)));//this should be changed to balance the price of the card with it's level
            }
            else
            {
                Resource randomResource = (Resource)Random.Range(1, Resource.GetNames(typeof(Resource)).Length - 1); //the 1 at the beginning of random.range is to exclude Click as a resource and the - 1 at the end is to exclude None
                if (!purchaseCost.ContainsKey(randomResource))
                {
                    purchaseCost.Add(randomResource, Mathf.RoundToInt(Power(2f, (float)cardLevel * 0.5f) * Random.Range(4, 9)));
                }
            }
        }
        
        //setting the card's function
        cardFunction = (CardFunction)Random.Range(0, CardFunction.GetNames(typeof(CardFunction)).Length);//assigning a random enum from CardFunction to cardFunction

        //setting the time it takes to complete one tick of the card's function.
        TickTime = Random.Range(25f, 50f) / (cardLevel + 1) / 2f + 1;

        //setting the values for the card's functions

        //addResource function
        resourceAddOutput = new KeyValuePair<Resource, int>((Resource)Random.Range(1, Resource.GetNames(typeof(Resource)).Length - 1), cardLevel + Random.Range(0, 3));//you cannot add Gobli
        
        //ResourceConvert function
        resourceConvertInput = new Dictionary<Resource, int>();
        int resourceConvertInputNumber = Random.Range(1, 4);//the number of resources going in
        for (int i = 0; i < resourceConvertInputNumber; i++)
        {
            KeyValuePair<Resource, int> resourceToAdd = new KeyValuePair<Resource, int>((Resource)Random.Range(0, Resource.GetNames(typeof(Resource)).Length - 1), cardLevel + Mathf.RoundToInt(Random.Range(0, cardLevel * 0.25f)));
            if (!resourceConvertInput.ContainsKey(resourceToAdd.Key))
            {
                resourceConvertInput.Add(resourceToAdd.Key, resourceToAdd.Value);
            }
        }
        //there is a 1 in 3 chance that the convert output will be gobli
        bool convertsToGobli = (Random.Range(0, 3) == 0);
        if (convertsToGobli)
        {
            resourceConvertOutput = new KeyValuePair<Resource, int>(Resource.Gobli, resourceConvertInputNumber * (cardLevel + Mathf.RoundToInt(Random.Range(0, cardLevel * 0.75f))));
        }
        else
        {
            resourceConvertOutput = new KeyValuePair<Resource, int>((Resource)Random.Range(1, Resource.GetNames(typeof(Resource)).Length - 1), resourceConvertInputNumber * (cardLevel + Mathf.RoundToInt(Random.Range(0, cardLevel * 0.75f))));
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
        Debug.Log("randomized card");
    }

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

    private void AddResources()
    {
        Counter.AddResource(resourceAddOutput.Key, resourceAddOutput.Value);
    }

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
        if (activated)
        {
            timer += Time.fixedDeltaTime;
            Debug.Log(TickTime);
            if (timer >= TickTime)
            {
                PerformFunciton();
                timer = 0;
            }
        }

    }
}
