using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using TMPro;

public class ChapterButtonHandler : MonoBehaviour
     , IPointerEnterHandler
     , IPointerExitHandler
     , IPointerDownHandler
     , IPointerUpHandler
{
    RawImage surface;
    string surfaceTexture;
    Vector3 initialMoleculeScale;
    TextMeshProUGUI text;
    Vector3 initialTextScale;
    static bool transiting = false;

    Transform cover;
    Transform coverTarget;

    TextMeshProUGUI desc;
    
    // Start is called before the first frame update
          
    void Start()
    {
        cover = GameObject.Find("Cover").transform;
        coverTarget = GameObject.Find("Canvas").transform;
        desc = GameObject.Find("Desc").GetComponent<TextMeshProUGUI>();

        StartCoroutine(StillTransiting());
        SetButton();
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!transiting)
        {
            TransformAndImageReset();
            surface.texture = Resources.Load<Texture>(surfaceTexture + "Selected");
            text.color = new Color32(0, 0, 0, 255);
            StartCoroutine(LerpAnimations.instance.Scale(surface.transform, 1.2f, 500f * Time.deltaTime, 0f));
            StartCoroutine(LerpAnimations.instance.Scale(text.transform, 1.2f, 500f * Time.deltaTime, 0f));
            switch (name)
            {
                case ("Chapter1Button"):
                    desc.text = "Look around the iconic locations in NUS High!"; break;
                case ("Chapter2Button"):
                    desc.text = "From high-tech labs to cozy lounges, find out more about what happens behind the scenes."; break;
                default:
                    break;
            }
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (!transiting)
        {
            StopAllCoroutines();
            surface.texture = Resources.Load<Texture>(surfaceTexture);
            text.color = new Color32(255, 255, 255, 255);
            StartCoroutine(LerpAnimations.instance.Scale(surface.transform, 1f, 100f * Time.deltaTime, 0f));
            StartCoroutine(LerpAnimations.instance.Scale(text.transform, 1f, 100f * Time.deltaTime, 0f));
            desc.text = "";
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (!transiting)
        {
            StopAllCoroutines();
            StartCoroutine(LerpAnimations.instance.Scale(surface.transform, 1.6f, 100f * Time.deltaTime, 0f));
            StartCoroutine(LerpAnimations.instance.Scale(text.transform, 1.6f, 100f * Time.deltaTime, 0f));
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (!transiting)
        {
            StopAllCoroutines();
            switch (name)
            {
                case ("Chapter1Button"):
                    PlayerPrefs.SetString("Chapter", "1");
                    MoveTo("Concourse"); break;
                case ("Chapter2Button"):
                    PlayerPrefs.SetString("Chapter", "2");
                    MoveTo("Clean Energy Lab"); break;
                case ("Chapter3Button"):
                    if (PlayerPrefs.HasKey("IsFailure"))
                    {
                        cover.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "f4ilur3f4ilur3f4ilur3f4ilur3f4ilur3f4ilur3f4ilur3f4ilur3f4ilur3f4ilur3f4ilur3f4ilur3f4ilur3f4ilur3f4ilur3f4ilur3f4ilur3f4ilur3f4ilur3f4ilur3f4ilur3f4ilur3f4ilur3f4ilur3f4ilur3f4ilur3f4ilur3f4ilur3f4ilur3f4ilur3f4ilur3f4ilur3f4ilur3f4ilur3f4ilur3f4ilur3f4ilur3f4ilur3f4ilur3f4ilur3f4ilur3f4ilur3f4ilur3f4ilur3f4ilur3f4ilur3f4ilur3f4ilur3f4ilur3f4ilur3f4ilur3f4ilur3f4ilur3f4ilur3f4ilur3f4ilur3f4ilur3f4ilur3f4ilur3f4ilur3f4ilur3f4ilur3f4ilur3f4ilur3f4ilur3f4ilur3f4ilur3f4ilur3f4ilur3f4ilur3f4ilur3f4ilur3f4ilur3f4ilur3f4ilur3f4ilur3f4ilur3f4ilur3f4ilur3f4ilur3f4ilur3f4ilur3f4ilur3f4ilur3f4ilur3f4ilur3f4ilur3f4ilur3f4ilur3f4ilur3f4ilur3f4ilur3f4ilur3f4ilur3f4ilur3f4ilur3f4ilur3f4ilur3f4ilur3f4ilur3f4ilur3f4ilur3f4ilur3f4ilur3f4ilur3f4ilur3f4ilur3f4ilur3f4ilur3f4ilur3f4ilur3f4ilur3f4ilur3f4ilur3f4ilur3f4ilur3f4ilur3f4ilur3f4ilur3f4ilur3f4ilur3f4ilur3f4ilur3f4ilur3f4ilur3f4ilur3f4ilur3f4ilur3f4ilur3f4ilur3f4ilur3f4ilur3f4ilur3f4ilur3f4ilur3f4ilur3f4ilur3f4ilur3f4ilur3f4ilur3f4ilur3f4ilur3f4ilur3f4ilur3f4ilur3f4ilur3f4ilur3f4ilur3f4ilur3f4ilur3f4ilur3f4ilur3f4ilur3f4ilur3f4ilur3f4ilur3f4ilur3f4ilur3f4ilur3f4ilur3f4ilur3f4ilur3f4ilur3f4ilur3f4ilur3f4ilur3f4ilur3f4ilur3f4ilur3f4ilur3f4ilur3f4ilur3f4ilur3f4ilur3f4ilur3f4ilur3f4ilur3f4ilur3f4ilur3f4ilur3f4ilur3f4ilur3f4ilur3f4ilur3f4ilur3f4ilur3f4ilur3f4ilur3f4ilur3f4ilur3f4ilur3f4ilur3f4ilur3f4ilur3f4ilur3f4ilur3f4ilur3f4ilur3f4ilur3f4ilur3f4ilur3f4ilur3f4ilur3f4ilur3f4ilur3f4ilur3f4ilur3f4ilur3f4ilur3f4ilur3f4ilur3f4ilur3f4ilur3f4ilur3f4ilur3f4ilur3f4ilur3f4ilur3f4ilur3f4ilur3f4ilur3f4ilur3f4ilur3f4ilur3f4ilur3f4ilur3f4ilur3f4ilur3f4ilur3f4ilur3f4ilur3f4ilur3f4ilur3f4ilur3f4ilur3f4ilur3f4ilur3f4ilur3f4ilur3f4ilur3f4ilur3f4ilur3f4ilur3f4ilur3f4ilur3f4ilur3f4ilur3f4ilur3f4ilur3f4ilur3f4ilur3f4ilur3f4ilur3f4ilur3f4ilur3f4ilur3f4ilur3f4ilur3f4ilur3f4ilur3f4ilur3f4ilur3f4ilur3f4ilur3f4ilur3f4ilur3f4ilur3f4ilur3f4ilur3f4ilur3f4ilur3f4ilur3f4ilur3f4ilur3f4ilur3f4ilur3f4ilur3f4ilur3f4ilur3f4ilur3f4ilur3f4ilur3f4ilur3f4ilur3f4ilur3f4ilur3f4ilur3f4ilur3f4ilur3f4ilur3f4ilur3f4ilur3f4ilur3f4ilur3f4ilur3f4ilur3f4ilur3f4ilur3f4ilur3f4ilur3f4ilur3f4ilur3f4ilur3f4ilur3f4ilur3f4ilur3f4ilur3f4ilur3f4ilur3f4ilur3f4ilur3f4ilur3f4ilur3f4ilur3f4ilur3f4ilur3f4ilur3f4ilur3f4ilur3f4ilur3f4ilur3f4ilur3f4ilur3f4ilur3f4ilur3f4ilur3f4ilur3f4ilur3f4ilur3f4ilur3f4ilur3f4ilur3f4ilur3f4ilur3f4ilur3f4ilur3f4ilur3f4ilur3f4ilur3f4ilur3f4ilur3f4ilur3f4ilur3f4ilur3f4ilur3f4ilur3f4ilur3f4ilur3f4ilur3f4ilur3f4ilur3f4ilur3f4ilur3f4ilur3f4ilur3f4ilur3f4ilur3f4ilur3f4ilur3f4ilur3f4ilur3f4ilur3f4ilur3f4ilur3f4ilur3f4ilur3f4ilur3f4ilur3f4ilur3f4ilur3f4ilur3f4ilur3f4ilur3f4ilur3f4ilur3f4ilur3f4ilur3f4ilur3f4ilur3f4ilur3f4ilur3f4ilur3f4ilur3f4ilur3f4ilur3f4ilur3f4ilur3f4ilur3f4ilur3f4ilur3f4ilur3f4ilur3f4ilur3f4ilur3f4ilur3f4ilur3f4ilur3f4ilur3f4ilur3f4ilur3f4ilur3f4ilur3f4ilur3f4ilur3f4ilur3f4ilur3f4ilur3f4ilur3";
                        LoadScene("ChapterSelectScene");
                    }
                    else
                    {
                        PlayerPrefs.SetString("Chapter", "3");
                        cover.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "w3lc0m3_T0_mY_d0m41n";
                        MoveTo("Flag3Room"); 
                    }
                    break;
                default:
                    break;
            }
        }
    }

    void Transit()
    {
        transiting = true;
        StartCoroutine(LerpAnimations.instance.Move(cover, coverTarget, 15f, 0f));
    }

    void MoveTo(string location)
    {
        PlayerPrefs.SetString(PlayerPrefs.GetString("Location") + "Status", "Visited");
        PlayerPrefs.SetString("Location", location);
        PlayerPrefs.SetString(location + "Status", "YouAreHere");
        LoadScene("LocationScene");
        Transit();
    }

    void LoadScene(string scene)
    {
        StartCoroutine(SceneChange(scene));
    }
    
    IEnumerator SceneChange(string scene)
    {
        yield return new WaitForSeconds(0.5f);
        SceneManager.LoadScene(scene);
    }

    IEnumerator StillTransiting()
    {
        transiting = true;
        yield return new WaitForSeconds(1f);
        transiting = false;
        yield break;
    }

    void TransformAndImageReset()
    {
        surface.transform.localScale = new Vector3(1, 1, 1);
        text.transform.localScale = new Vector3(1, 1, 1);
        StopAllCoroutines();
    }

    public void DeactivateAllButtons()
    {
        GameObject buttons = GameObject.FindWithTag("Button");
        for (int i = 0; i < buttons.transform.childCount; ++i)
        { 
            buttons.transform.GetChild(0).GetComponent<Button>().interactable = false;
        }
    }

    public void SetButton()
    {
        surface = this.gameObject.transform.GetChild(0).GetComponent<RawImage>();
        surfaceTexture = "Sprites/BigButton" + name.Substring(7, 1);
        text = this.gameObject.transform.GetChild(1).GetComponent<TextMeshProUGUI>();
        initialTextScale = text.transform.localScale;
    }

    public void Click()
    {
        if (!transiting) OnPointerUp(null);
    }
}


