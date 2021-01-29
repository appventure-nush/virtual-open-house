using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Flag2 : MonoBehaviour
{
    float countdown = 227.6f;
    string thisIsIt = "Type the no. of seconds you waited here:";
    bool active = false;
    bool shown = false;
    string error = "";
    int secret = 0;
    string[] nope = new string[]{
        "Nope",
        "Nuh-uh",
        "Not even close",
        "Wrong place",
        "Different castle",
        "Ain't nothin' here",
        "Empty",
        "insert_epicfail",
        "NULL",
        "It's not here",
        "Nah",
        "blank",
        "j3b41t3d",
        "Bamboozled",
        "This ain't it chief",
        "outplayedLMAO",
        "Goneded",
        "Nothing",
        "3mp+y v01d",
        "0",
        "Check elsewhere",
        "Maybe not",
        "Try again",
        "It's not this place"
    };

    RawImage notTheFlag;
    TextMeshProUGUI flag;
    System.Random random;
    // Start is called before the first frame update
    void Start()
    {
        secret = 0;
        notTheFlag = GameObject.Find("NotTheFlag").GetComponent<RawImage>();
        flag = GameObject.Find("Flag").GetComponent<TextMeshProUGUI>();
        random = new System.Random();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            notTheFlag.color = new Color(1, 1, 1, 0);
            flag.text = "";
            Debug.Log("huh?");
            countdown = 227.6f;
        }

        if (!shown)
        {
            if (countdown >= 0) countdown -= Time.deltaTime;
            else
            {
                if (PlayerPrefs.GetString("Location").Equals("Life Sciences Lab"))
                {
                    GameObject.Find("NameText").GetComponent<TextMeshProUGUI>().text = Base64Encode(thisIsIt);
                    active = true;
                }
                else GameObject.Find("NameText").GetComponent<TextMeshProUGUI>().text = Base64Encode(nope[random.Next(nope.Length)]);
                shown = true;
            }
        }

        if (active)
        {
            if (Input.GetKeyDown("2"))
            {
                if (secret == 0 || secret == 1) secret++;
                else secret = 0;
            }
            if (Input.GetKeyDown("6"))
            {
                if (secret == 2)
                {
                    secret++;
                    error = "6";
                }
                else secret = 0;
            }
            if (Input.GetKeyDown("7"))
            {
                if (secret == 2)
                {
                    secret++;
                    error = "7";
                }
                else secret = 0;
            }
            if (Input.GetKeyDown("8"))
            {
                if (secret == 2)
                {
                    secret++;
                    error = "8";
                }
                else secret = 0;
            }
        }

        if (secret >= 3)
        {
            secret = 0;
            PrintFlag();
        }
    }

    public static string Base64Encode(string plainText)
    {
        var plainTextBytes = System.Text.Encoding.ASCII.GetBytes(plainText);
        return System.Convert.ToBase64String(plainTextBytes);
    }

    void PrintFlag()
    {
        notTheFlag.texture = Resources.Load<Texture>("Sprites/NotTheFlag2");
        notTheFlag.color = new Color(1, 1, 1, 1);
        flag.text = "Sorry\t\nbut 3 minutes and 4" + error + " seconds is\nNUSHCTF{w4y_T00_l0n9}";
    }
}
