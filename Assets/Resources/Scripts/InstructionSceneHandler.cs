using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class InstructionSceneHandler : MonoBehaviour
     , IPointerDownHandler
{
    string location="Introduction";
    RawImage backdrop;

    RawImage dialogueBox;
    TextMeshProUGUI nameText;
    TextMeshProUGUI dialogueText;
    IEnumerator scrollTextCoroutine;
    float timePerChar;
    float delay;

    RawImage hongMeng;
    string[] hongMengSprites;

    string[] texts;
    int currentTextIndex = 0;
    public bool scrolling { get; set; }
    public bool moving { get; set; }

    int currentPhase; //0 for intro, 1 for intro, 2 for video, 3 for outro, 4 for waiting

    MoveButtonsHandler moveButtonsHandler;

    AudioSource sfx;

    // Start is called before the first frame update
    void Awake()
    {
        backdrop = GameObject.Find("Backdrop").GetComponent<RawImage>();
        hongMeng = GameObject.Find("HongMeng").GetComponent<RawImage>();
        dialogueBox = GameObject.Find("DialogueBox").GetComponent<RawImage>();
        nameText = GameObject.Find("NameText").GetComponent<TextMeshProUGUI>();
        dialogueText = GameObject.Find("DialogueText").GetComponent<TextMeshProUGUI>();
        timePerChar = 0.03f;
        delay = 1.5f;
        scrollTextCoroutine = ScrollText(timePerChar, delay);
        sfx = GameObject.Find("SFX").GetComponent<AudioSource>();
        sfx.volume = 0.3f;

        backdrop.texture = Resources.Load<Texture>("Sprites/NUSH2");
    }

    void Start() 
    {
        LocationIntro(location);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (currentPhase != 2 && currentPhase != 4)
        {
            if (scrolling)
            {
                StopCoroutine(scrollTextCoroutine);
                dialogueText.text = texts[currentTextIndex];
                scrolling = false;
            }
            else
            {
                StopCoroutine(scrollTextCoroutine);
                NextText();
            }
        }
    }

    public void NextText()
    {
        currentTextIndex++;
        if (currentTextIndex >= texts.Length)
        {
            StartCoroutine(SceneChange("ChapterSelectScene"));
            PlayerPrefs.SetInt("TutorialFinish", 1);
        }
        else
        {
            scrollTextCoroutine = ScrollText(timePerChar, delay);
            StartCoroutine(scrollTextCoroutine);
            if (hongMengSprites != null)
            {
                hongMeng.texture = Resources.Load<Texture>("Sprites/" + hongMengSprites[currentTextIndex]);
                nameText.text = hongMengSprites[currentTextIndex].Substring(0, hongMengSprites[currentTextIndex].Length - 1);
            }
        }
    }

    public void LocationIntro(string location)
    {
        StopAllCoroutines();
        delay = 0f;

        StartCoroutine(LerpAnimations.instance.Fade(backdrop, 1f, 0.4f, 0f));
        StartCoroutine(LerpAnimations.instance.Fade(dialogueBox, 1f, 0.2f, 0f));
        StartCoroutine(LerpAnimations.instance.Fade(hongMeng, 1f, 0.4f, 0f));

        TextAsset textData = Resources.Load<TextAsset>("Texts/" + location + "Intro");
        string reader = textData.text;
        texts = reader.Split('\n');
        currentTextIndex = 0;

        hongMengSprites = new string[texts.Length];
        for (int i = 0; i < texts.Length; i++)
        {
            hongMengSprites[i] = texts[i].Split(';')[0];
            texts[i] = texts[i].Split(';')[1];
        }
        
        hongMeng.texture = Resources.Load<Texture>("Sprites/" + hongMengSprites[currentTextIndex]);
        nameText.text = hongMengSprites[currentTextIndex].Substring(0, hongMengSprites[currentTextIndex].Length - 1);

        scrollTextCoroutine = ScrollText(timePerChar, delay);
        StartCoroutine(scrollTextCoroutine);
        PlayerPrefs.SetInt(location + "Phase", 1);
    }

    IEnumerator ScrollText(float timePerChar, float delay)
    {
        yield return new WaitForSeconds(delay);
        scrolling = true;
        string text = texts[currentTextIndex];
        for (int i = 0; i <= text.Length; i++)
        {
            dialogueText.text = text.Substring(0, i);
            if (i > 0 && i < text.Length)
            {
                if (text[i-1].Equals(',')) yield return new WaitForSeconds(6f * timePerChar);
                else if (text[i-1].Equals('.') || text[i-1].Equals('!') || text[i - 1].Equals('?')) yield return new WaitForSeconds(15f * timePerChar);
                else yield return new WaitForSeconds(timePerChar);
            }
        }
        scrolling = false;
        yield return null;
    }

    IEnumerator SceneChange(string scene)
    {
        yield return new WaitForSeconds(0.5f);
        SceneManager.LoadScene(scene);
    }
}
