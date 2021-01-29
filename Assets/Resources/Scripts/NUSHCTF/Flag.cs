using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class Flag : MonoBehaviour
{
    int secret;
    RawImage notTheFlag;
    TextMeshProUGUI flag;

    private void Start()
    {
        secret = 0;
        notTheFlag = GameObject.Find("NotTheFlag").GetComponent<RawImage>();
        flag = GameObject.Find("Flag").GetComponent<TextMeshProUGUI>();
    }

    private void Update()
    {
        if (Input.GetKeyDown("a"))
        {
            if (secret == 0) secret++;
            else secret = 0;
        }
        if (Input.GetKeyDown("p"))
        {
            if (secret == 1 || secret == 2) secret++;
            else secret = 0;
        }
        if (Input.GetKeyDown("v"))
        {
            if (secret == 3) secret++;
            else secret = 0;
        }
        if (Input.GetKeyDown("e"))
        {
            if (secret == 9)
            {
                secret = 0;
                PrintFlag();
            }
            else if (secret == 4) secret++;
            else secret = 0;
        }
        if (Input.GetKeyDown("n"))
        {
            if (secret == 5) secret++;
            else secret = 0;
        }
        if (Input.GetKeyDown("t"))
        {
            if (secret == 6) secret++;
            else secret = 0;
        }
        if (Input.GetKeyDown("u"))
        {
            if (secret == 7) secret++;
            else secret = 0;
        }
        if (Input.GetKeyDown("r"))
        {
            if (secret == 8) secret++;
            else secret = 0;
        }
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            notTheFlag.color = new Color(1, 1, 1, 0);
            flag.text = "";
        }
    }

    void PrintFlag()
    {
        notTheFlag.texture = Resources.Load<Texture>("Sprites/NotTheFlag");
        notTheFlag.color = new Color(1, 1, 1, 1);
        int[] arr1 = new int[] { 33, 59, 48, 45, 54, 36, 41, 21, 15, 69, 39, 7, 81, 54, 7, 0, 86, 5, 4, 0, 48, 83, 2, 30, 92, 69, 11, 15 };
        int[] arr2 = new int[] { 111, 110, 99, 101, 117, 112, 111, 110, 97, 116, 105, 109, 101, 105, 100, 114, 101, 97, 109, 116, 111, 102, 97, 108, 111, 118, 101, 114 };

        byte[] result = new byte[arr1.Length];
        for (int i = 0; i < arr1.Length; i++)
        {
            result[i] = (byte)(arr1[i] ^ arr2[i]);
        }
        string hex = BitConverter.ToString(result).Replace("-", "");

        string flagStr = "";
        for (int i = 0; i < hex.Length; i += 2)
        {
            flagStr += (Convert.ToChar(Convert.ToUInt32(hex.Substring(i, 2), 16)));
        }

        flag.text = flagStr;
    }

}