using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Flag3 : MonoBehaviour
{
    string s = "";

    RawImage notTheFlag;
    TextMeshProUGUI flag;
    AudioSource audioSource;
    bool found;
    // Start is called before the first frame update
    void Start()
    {
        found = false;
        if (PlayerPrefs.GetString("Chapter").Equals("5"))
        { 
            PlayerPrefs.DeleteAll();
            Destroy(GameObject.Find("Buttons"));
            PlayerPrefs.SetString("IsFailure", "WillBe");
        }
        notTheFlag = GameObject.Find("NotTheFlag").GetComponent<RawImage>();
        flag = GameObject.Find("Flag").GetComponent<TextMeshProUGUI>();
        audioSource = GameObject.Find("SFX").GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        s += Input.inputString;
        if (s.Length == 10 && !found)
        {
            if (s.Equals("t4nrU1f3nG"))
            {
                PrintFlag();
                found = true;
            }
            else s = "";
        }
    }

    void PrintFlag()
    {
        notTheFlag.texture = Resources.Load<Texture>("Sprites/NotTheFlag3");
        notTheFlag.color = new Color(1, 1, 1, 1);
        StartCoroutine(LerpAnimations.instance.Fade(notTheFlag, 0f, 0.2f, 0.3f));
        flag.text = "NUSHCTF{st4T15t1cal_4n0Maly}";
        audioSource.PlayOneShot(Resources.Load<AudioClip>("SFX/d34th"));
        PlayerPrefs.DeleteKey("IsFailure");
    }
}
