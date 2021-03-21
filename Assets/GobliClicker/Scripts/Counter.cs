using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UIElements;
using UnityEngine.UI;

public enum Resource
{
    Gobli,
    Choblex,
    Hoink,
    Yoswhal,
    Qumdo,
    Lombert,
    Vook,
    None
}

public class Counter : MonoBehaviour
{
    // this exists so that i can reference serialized variables from a static method
    private static Counter mainCounter;
    // a list of all resources for use in foreach statements
    private List<Resource> listOfAllResources = new List<Resource>();
    // a dictionary containing an estimate of how many resources per second are being added to a given resource
    public static Dictionary<Resource, int> resourcesPerSecond = new Dictionary<Resource, int>();
    // holds the change in resources in the last 10 seconds. used to find resources per second
    private Dictionary<Resource, int> resourceChange = new Dictionary<Resource, int>();
    // The main dictionary which holds the amount of all the resources. 
    public static Dictionary<Resource, int> counter = new Dictionary<Resource, int>();

    //a currently unchanging variable that determines how much each click is worth.
    int clickAmount = 1;

    //ideally this would be a dictionary but you can't serialize dictionaries. it works fine
    [SerializeField, Tooltip("These TMP Texts need to be put in here in the order that the resources have in the enum; Gobli, R, O, Y, G, B, V")]
    private List<TextMeshProUGUI> buttonTexts = new List<TextMeshProUGUI>(7);

    //Adds an amount of a resource such that the resourcesPerSecond is effected
    public static void AddResource(Resource _resource, int amount)// The amount can be negative
    {
        //add the specified amount to the specified resource in counter
        counter[_resource] += amount;
        //add the specified amount to the specified resource in resourceChange 
        mainCounter.resourceChange[_resource] += amount;
        //the above line will be undone in 10 seconds by the coroutine below
        mainCounter.StartCoroutine(mainCounter.UndoResourceChangeIn10Seconds(_resource, amount));
    }

    void Start()
    {
        //set the mainCounter to the counter attatched to this gameobject (there is only one counter)
        mainCounter = gameObject.GetComponent<Counter>();
        //setting all the dictionaries to 0 for each resource that there is
        for (int i = 0; (Resource)i != Resource.None; i++)
        {
            listOfAllResources.Add((Resource)i);
            counter.Add((Resource)i, 0);
            resourceChange.Add((Resource)i, 0);
            resourcesPerSecond.Add((Resource)i, 0);
        }
    }

    private void Update()
    {
        //set the text on on all the buttons to match the number of their respective resource
        foreach (Resource resource in listOfAllResources)
        {
            buttonTexts[(int)resource].text = $"{counter[resource]} {resource}, at {resourcesPerSecond[resource]}/s";
        }
    }

    void FixedUpdate()
    {
        //sets resourcesPerSecond to the change in resources in the last 10 seconds / 10
        foreach (Resource resource in listOfAllResources)
        {
            resourcesPerSecond[resource] = Mathf.RoundToInt(resourceChange[resource] / 10);
        }
    }

    IEnumerator UndoResourceChangeIn10Seconds(Resource resource, int amount)
    {
        //removes the specified amount from the specified resource in resource change
        yield return new WaitForSeconds(10);
        resourceChange[resource] -= amount;
    }

    //all these functions are called by the clicker buttons.
    public void ButtonAddGobli() => AddResource(Resource.Gobli, clickAmount);
    public void ButtonAddChoblex() => AddResource(Resource.Choblex, clickAmount);
    public void ButtonAddHoink() => AddResource(Resource.Hoink, clickAmount);
    public void ButtonAddYoswhal() => AddResource(Resource.Yoswhal, clickAmount);
    public void ButtonAddQumdo() => AddResource(Resource.Qumdo, clickAmount);
    public void ButtonAddLombert() => AddResource(Resource.Lombert, clickAmount);
    public void ButtonAddVook() => AddResource(Resource.Vook, clickAmount);
}
