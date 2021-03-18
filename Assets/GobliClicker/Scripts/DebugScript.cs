using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DebugScript : MonoBehaviour
{
    private List<Resource> listOfAllResources = new List<Resource>();

    [SerializeField, Tooltip("These TMP Texts need to be put in here in the order that the resources have in the enum; Gobli, R, O, Y, G, B, V")]
    private List<TextMeshProUGUI> buttonTexts;

    void Start()
    {
        for (int i = 0; (Resource)i != Resource.None; i++)
        {
            listOfAllResources.Add((Resource)i);
        }
    }

    private void Update()
    {

        foreach (Resource resource in listOfAllResources)
        {
            Debug.Log(buttonTexts[(int)resource].text);
            buttonTexts[(int)resource].text = $"resource: {resource}";
        }
    }
}
