using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.Video;
using TMPro;
using System.Text.RegularExpressions;

public class Flag3Handler : MonoBehaviour, IPointerDownHandler
{
    string location;
    RawImage backdrop;

    RawImage dialogueBox;
    TextMeshProUGUI nameText;
    TextMeshProUGUI dialogueText;
    IEnumerator nextTextCoroutine;
    IEnumerator playVideoCoroutine;
    float timePerChar;
    float delay;

    RawImage hongMeng;

    TextAsset allText;
    string[] textArr;
    int currentTextIndex = -1;
    public bool scrolling { get; set; }
    public bool moving { get; set; }

    VideoPlayer videoPlayer;
    AudioSource audioSource;
    RawImage videoScreen;

    // Start is called before the first frame update
    void Start()
    {
        location = PlayerPrefs.GetString("Location");

        backdrop = GameObject.Find("Backdrop").GetComponent<RawImage>();
        dialogueBox = GameObject.Find("DialogueBox").GetComponent<RawImage>();
        hongMeng = GameObject.Find("HongMeng").GetComponent<RawImage>();

        nameText = GameObject.Find("NameText").GetComponent<TextMeshProUGUI>();
        dialogueText = GameObject.Find("DialogueText").GetComponent<TextMeshProUGUI>();

        timePerChar = 0.03f;
        delay = 1.5f;
        allText = Resources.Load<TextAsset>("Texts/" + location + "Entry");
        textArr = allText.text.Split('\n');
        nextTextCoroutine = Next(timePerChar, delay);

        videoPlayer = GameObject.Find("VideoPhase").GetComponent<VideoPlayer>();
        audioSource = GameObject.Find("VideoPhase").GetComponent<AudioSource>();
        audioSource.volume = 0.3f;
        videoScreen = GameObject.Find("VideoScreen").GetComponent<RawImage>();

        NextText();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        NextText();
    }

    public void NextText()
    {
        if (scrolling)
        {
            StopCoroutine(nextTextCoroutine);
            
        }
        else
        {
            currentTextIndex++;
            if (currentTextIndex < textArr.Length)
            {
                switch (textArr[currentTextIndex])
                {
                    case ("Entry"):
                        Entry();
                        currentTextIndex++;
                        StartCoroutine(nextTextCoroutine);
                        break;
                    default:
                        break;
                }
            }
        }
    }

    IEnumerator Next(float timePerChar, float delay)
    {
        string[] processedText = ProcessText(textArr[currentTextIndex]);
        string name = processedText[0];
        string texturePath = processedText[1];
        string text = processedText[2];

        SetName(name);
        SetHongMeng(texturePath);
        
        yield return new WaitForSeconds(delay);
        scrolling = true;
        for (int i = 0; i <= text.Length; i++)
        {
            dialogueText.text = text.Substring(0, i);
            if (i > 0 && i < text.Length)
            {
                if (text[i - 1].Equals(',')) yield return new WaitForSeconds(6f * timePerChar);
                else if (text[i - 1].Equals('.') || text[i - 1].Equals('!') || text[i - 1].Equals('?')) yield return new WaitForSeconds(15f * timePerChar);
                else yield return new WaitForSeconds(timePerChar);
            }
        }
        scrolling = false;
        yield return null;
    }

    IEnumerator PlayVideo()
    {
        videoScreen.texture = Resources.Load<Texture>("Sprites/CheckYourInternet");
        videoPlayer.url = textArr[currentTextIndex];
        videoPlayer.Prepare();
        while (!videoPlayer.isPrepared)
        {
            yield return new WaitForSeconds(0.0166667f);
        }
        videoScreen.color = new Color(1, 1, 1, 1);
        videoScreen.texture = videoPlayer.texture;
        videoPlayer.Play();
        audioSource.Play();
        yield return null;
    }

    void Entry()
    {
        StopAllCoroutines();
        ClearHongMeng();
        ClearName();
        SetBackdrop(location);
        NextText();
    }

    void Intro()
    {
        StopAllCoroutines();
    }

    void Video()
    {
        StopAllCoroutines();
        ClearHongMeng();
        ClearName();
        SetBackdrop(location);
    }

    void SetHongMeng(string path)
    {
        hongMeng.texture = Resources.Load<Texture>("Sprites/" + path);
        StartCoroutine(LerpAnimations.instance.Fade(hongMeng, 1f, 0.4f, 0f));
    }

    void ClearHongMeng()
    {
        nameText.text = "";
        StartCoroutine(LerpAnimations.instance.Fade(hongMeng, 1f, 0.4f, 0f));
    }

    void SetName(string name)
    {
        nameText.text = name;
    }

    void ClearName()
    {
        nameText.text = "";
    }

    void SetDialogueText(string text)
    {
        dialogueText.text = text;
    }

    void SetBackdrop(string path)
    {
        backdrop.texture = Resources.Load<Texture>("Sprites/" + path);
        StartCoroutine(LerpAnimations.instance.Fade(backdrop, 1f, 0.4f, 0f));
    }

    string[] ProcessText(string text)
    {
        string[] processedText = new string[3];
        for (int i = 0; i < text.Length; i++)
        {
            if (Regex.IsMatch(text.Substring(i, 1), @"[\d-]") && processedText[0] == null)
            {
                processedText[0] = text.Substring(0, i);
            }
            if (Regex.IsMatch(text.Substring(i, 1), @"[:]"))
            {
                processedText[1] = text.Substring(0, i);
                processedText[2] = text.Substring(i + 1, text.Length - i + 1);
                return processedText;
            }
        }
        return new string[] { "", "", text };
    }

}
