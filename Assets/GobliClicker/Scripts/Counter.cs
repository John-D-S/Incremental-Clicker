using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Resource
{
    Gobli,
    Red,
    Orange,
    Yellow,
    Green,
    Blue,
    Violet,
    None
}

public class Counter : MonoBehaviour
{
    public static Dictionary<Resource, int> counter = new Dictionary<Resource, int>();
    public static Dictionary<Resource, List<float>> resourcesInLastSecond = new Dictionary<Resource, List<float>>();

    int clickAmount = 1;
    //public static Dictionary<Resource, int> clickAmounts = new Dictionary<Resource, int>();

    //    private static List<float> clicksInLastSecond = new List<float>(); //stores a list of floats representing timers that represent the age of clicks made in the last second

    public static void AddResource(Resource _resource, int amount)// The amount can be negative
    {
        counter[_resource] += amount;
        if (amount > 0)
        {
            resourcesInLastSecond[_resource].Insert(0, 0);
        }
        else if (amount < 0 && resourcesInLastSecond[_resource].Count > 0)
        {
            resourcesInLastSecond[_resource].RemoveAt(-1);
        }
    }

    void Start()
    {
        for (int i = 0; (Resource)i != Resource.None; i++)
        {
            counter.Add((Resource)i, 0);
        }    
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        foreach (KeyValuePair<Resource, List<float>> resourceInLastSecond in resourcesInLastSecond)
        {
            for (int j = 0; j < resourceInLastSecond.Value.Count; j++) //for every timer in clicksInLastSecond, 
            {
                resourceInLastSecond.Value[j] += Time.fixedDeltaTime;
            }
            while (resourceInLastSecond.Value[-1] >= 1)
            {
                resourceInLastSecond.Value.RemoveAt(-1);
            }
        }

    }
    public void ButtonAddGobli()
    {
        AddResource(Resource.Gobli, clickAmount);
    }

    public void ButtonAddRed()
    {
        AddResource(Resource.Red, clickAmount);
    }

    public void ButtonAddOrange()
    {
        AddResource(Resource.Orange, clickAmount);
    }

    public void ButtonAddYellow()
    {
        AddResource(Resource.Yellow, clickAmount);
    }
    
    public void ButtonAddGreen()
    {
        AddResource(Resource.Green, clickAmount);
    }
    
    public void ButtonAddBlue()
    {
        AddResource(Resource.Blue, clickAmount);
    }

    public void ButtonAddViolet()
    {
        AddResource(Resource.Violet, clickAmount);
    }

}
