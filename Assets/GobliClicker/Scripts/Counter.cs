using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UIElements;
using UnityEngine.UI;

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
    private List<Resource> listOfAllResources = new List<Resource>();

    public static Dictionary<Resource, int> counter = new Dictionary<Resource, int>();
    //public static Dictionary<Resource, List<float>> resourcesInLastSecond = new Dictionary<Resource, List<float>>();

    int clickAmount = 1;

    [SerializeField, Tooltip("These TMP Texts need to be put in here in the order that the resources have in the enum; Gobli, R, O, Y, G, B, V")]
    private List<TextMeshProUGUI> buttonTexts = new List<TextMeshProUGUI>(7);

    //private Dictionary<Resource, TextMeshProUGUI> buttonTextsDict = new Dictionary<Resource, TextMeshProUGUI>();
    //private Dictionary<Resource, string> resourceButtonDescriptions = new Dictionary<Resource, string>(7);

    public static void AddResource(Resource _resource, int amount)// The amount can be negative
    {
        counter[_resource] += amount;
        /*
        if (amount > 0)
        {
            resourcesInLastSecond[_resource].Insert(0, 0);
        }
        else if (amount < 0 && resourcesInLastSecond[_resource].Count > 0)
        {
            resourcesInLastSecond[_resource].RemoveAt(resourcesInLastSecond[_resource].Count - 1);
        }
        */
    }

    void Start()
    {
        for (int i = 0; (Resource)i != Resource.None; i++)
        {
            listOfAllResources.Add((Resource)i);
            counter.Add((Resource)i, 0);
            //resourcesInLastSecond.Add((Resource)i, new List<float>() { });
        }
    }

    private void Update()
    {
        
        foreach (Resource resource in listOfAllResources)
        {
            buttonTexts[(int)resource].text = $"{counter[resource]} {resource}";
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //this was for resources per second. it didn't work out.
        /*
        foreach (Resource resource in listOfAllResources)
        {
            for (int i = 0; i < resourcesInLastSecond[resource].Count; i++) //for every timer in clicksInLastSecond, 
            {
                Debug.Log(resourcesInLastSecond[resource][i]);
                resourcesInLastSecond[resource][i] += Time.fixedDeltaTime;
            }
            while (resourcesInLastSecond[resource][resourcesInLastSecond[resource].Count -1] >= 1)
            {
                resourcesInLastSecond[resource].RemoveAt(resourcesInLastSecond[resource].Count - 1);
            }
        }
        */
    }

    //a list of all the functions that the clickerButtons call
    public void ButtonAddGobli() => AddResource(Resource.Gobli, clickAmount);
    public void ButtonAddRed() => AddResource(Resource.Red, clickAmount);
    public void ButtonAddOrange() => AddResource(Resource.Orange, clickAmount);
    public void ButtonAddYellow() => AddResource(Resource.Yellow, clickAmount);
    public void ButtonAddGreen() => AddResource(Resource.Green, clickAmount);
    public void ButtonAddBlue() => AddResource(Resource.Blue, clickAmount);
    public void ButtonAddViolet() => AddResource(Resource.Violet, clickAmount);
}
