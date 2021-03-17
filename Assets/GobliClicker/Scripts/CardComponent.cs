using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardComponent : MonoBehaviour
{
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

    private Card card;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
