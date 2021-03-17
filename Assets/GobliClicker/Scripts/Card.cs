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
    public Dictionary<Resource, int> purchaseCost; //the resources and the number of them that it costs to purchase this card.
    private int cardLevel;

    private string cardName;
    private string cardDescription = "";

    private CardFunction cardFunction;//this describes what this card does

    private float timer;
    private float tickTime;//time to complete the cardFunction

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
                purchaseCost.Add((Resource)0, Mathf.RoundToInt(Power(1.5f, (float)cardLevel * 0.25f) * Random.Range(20, 30)));//this should be changed to balance the price of the card with it's level
            }
            else
            {
                Resource randomResource = (Resource)Random.Range(1, Resource.GetNames(typeof(CardFunction)).Length - 1); //the 1 at the beginning of random.range is to exclude Click as a resource and the - 1 at the end is to exclude None
                purchaseCost.Add(randomResource, Mathf.RoundToInt(Power(1.5f, (float)cardLevel * 0.25f) * Random.Range(20, 30)));
            }
        }
        
        //setting the card's function
        cardFunction = (CardFunction)Random.Range(0, CardFunction.GetNames(typeof(CardFunction)).Length);//assigning a random enum from CardFunction to cardFunction

        //setting the time it takes to complete one tick of the card's function.
        tickTime = Random.Range(10, 50) / cardLevel;

        //setting the values for the card's functions (I'm setting even the ones I'm not using for reasons I can't remember)

        //addResource function
        resourceAddOutput = new KeyValuePair<Resource, int>((Resource)Random.Range(0, Resource.GetNames(typeof(CardFunction)).Length - 1), cardLevel + Random.Range(0, 3));
        
        //ResourceConvert function
        resourceConvertOutput = new KeyValuePair<Resource, int>((Resource)Random.Range(0, Resource.GetNames(typeof(CardFunction)).Length - 1), cardLevel + Mathf.RoundToInt(Random.Range(0, cardLevel * 0.75f)));
        resourceConvertInput = new Dictionary<Resource, int>();
        int resourceConvertInputNumber = Random.Range(1, 4);
        for (int i = 0; i < resourceConvertInputNumber; i++)
        {
            resourceConvertInput.Add((Resource)Random.Range(0, Resource.GetNames(typeof(CardFunction)).Length - 1), cardLevel + Mathf.RoundToInt(Random.Range(0, cardLevel * 0.25f)));
        }

        //setting the description
        switch (cardFunction)
        {
            case CardFunction.AddResource:
                cardDescription = "Adds " + resourceAddOutput.Value.ToString() + " " + resourceAddOutput.Key.ToString() + "s every " + tickTime.ToString() + " seconds.";
                break;
            case CardFunction.ConvertResources:
                int i = 0;
                cardDescription = "Converts ";
                foreach (KeyValuePair<Resource, int> InputItem in resourceConvertInput)
                {
                    if (i == 0)
                    {
                        cardDescription += $"{InputItem.Value} {InputItem.Key}";
                    }
                    else if (i < resourceConvertInput.Count - 1)
                    {
                        cardDescription += $", {InputItem.Value} {InputItem.Key}";
                    }
                    else if (i == resourceConvertInput.Count - 1)
                    {
                        cardDescription += $" and {InputItem.Value} {InputItem.Key}";
                    }
                }
                cardDescription += $" into {resourceConvertOutput.Value} {resourceConvertOutput.Key} every {tickTime} seconds.";

                break;
        }
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
            if (timer >= tickTime)
            {
                PerformFunciton();
                timer = 0;
            }
        }

    }
}
