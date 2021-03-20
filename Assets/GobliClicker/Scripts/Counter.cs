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
    private static Counter mainCounter;
    private List<Resource> listOfAllResources = new List<Resource>();
    public static Dictionary<Resource, int> resourcesPerSecond = new Dictionary<Resource, int>();
    private Dictionary<Resource, int> resourceChange = new Dictionary<Resource, int>();
    public static Dictionary<Resource, int> counter = new Dictionary<Resource, int>();

    int clickAmount = 1;

    [SerializeField, Tooltip("These TMP Texts need to be put in here in the order that the resources have in the enum; Gobli, R, O, Y, G, B, V")]
    private List<TextMeshProUGUI> buttonTexts = new List<TextMeshProUGUI>(7);

    public static void AddResource(Resource _resource, int amount)// The amount can be negative
    {
        counter[_resource] += amount;
        mainCounter.resourceChange[_resource] += amount;
        mainCounter.StartCoroutine(mainCounter.UndoResourceChangeIn20Seconds(_resource, amount));
    }

    void Start()
    {
        mainCounter = gameObject.GetComponent<Counter>();
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
        foreach (Resource resource in listOfAllResources)
        {
            buttonTexts[(int)resource].text = $"{counter[resource]} {resource}, at {resourcesPerSecond[resource]}/s";
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        foreach (Resource resource in listOfAllResources)
        {
            resourcesPerSecond[resource] = Mathf.RoundToInt(resourceChange[resource] / 10);
        }
    }

    IEnumerator UndoResourceChangeIn20Seconds(Resource resource, int amount)
    {
        yield return new WaitForSeconds(10);
        resourceChange[resource] -= amount;
    }

    //a list of all the functions that the clickerButtons call
    public void ButtonAddGobli() => AddResource(Resource.Gobli, clickAmount);
    public void ButtonAddChoblex() => AddResource(Resource.Choblex, clickAmount);
    public void ButtonAddHoink() => AddResource(Resource.Hoink, clickAmount);
    public void ButtonAddYoswhal() => AddResource(Resource.Yoswhal, clickAmount);
    public void ButtonAddQumdo() => AddResource(Resource.Qumdo, clickAmount);
    public void ButtonAddLombert() => AddResource(Resource.Lombert, clickAmount);
    public void ButtonAddVook() => AddResource(Resource.Vook, clickAmount);
}
