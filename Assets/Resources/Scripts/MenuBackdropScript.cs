using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuBackdropScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        System.Random random = new System.Random();
        GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/NUSH" + (random.Next(1000) % 4 + 1).ToString());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
