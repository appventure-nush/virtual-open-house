using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using TMPro;
using UnityEngine.EventSystems;
using System.Net;
using System.IO;

public class LocationSceneHandler : MonoBehaviour
     , IPointerDownHandler
{
    string location;
    RawImage backdrop;

    RawImage dialogueBox;
    TextMeshProUGUI nameText;
    TextMeshProUGUI dialogueText;
    IEnumerator scrollTextCoroutine;
    float timePerChar;
    float delay;

    RawImage backing;

    RawImage hongMeng;
    string[] hongMengSprites;

    string[] texts;
    int currentTextIndex = 0;
    public bool scrolling { get; set; }
    public bool moving { get; set; }

    int currentPhase; //0 for intro, 1 for intro, 2 for video, 3 for outro, 4 for waiting

    VideoPlayer videoPlayer;
    AudioSource audioSource;
    RawImage videoScreen;

    VideoPlayer videoPlayer360;
    RawImage videoScreen360;

    MoveButtonsHandler moveButtonsHandler;

    AudioSource sfx;

    Camera normal_cam;
    Camera cam360;

    ButtonHandler button360;
    RawImage button360bg;

    Material mat360;
    public bool active360;

    GameObject landmarks;

    // Start is called before the first frame update
    void Awake()
    {
        location = PlayerPrefs.GetString("Location");
        Debug.Log(location);
        active360 = false;

        backdrop = GameObject.Find("Backdrop").GetComponent<RawImage>();
        hongMeng = GameObject.Find("HongMeng").GetComponent<RawImage>();
        dialogueBox = GameObject.Find("DialogueBox").GetComponent<RawImage>();
        nameText = GameObject.Find("NameText").GetComponent<TextMeshProUGUI>();
        
        dialogueText = GameObject.Find("DialogueText").GetComponent<TextMeshProUGUI>();
        timePerChar = 0.03f;
        delay = 1.5f;
        scrollTextCoroutine = ScrollText(timePerChar, delay);
        
        videoPlayer = GameObject.Find("VideoPhase").GetComponent<VideoPlayer>();
        audioSource = GameObject.Find("VideoPhase").GetComponent<AudioSource>();
        audioSource.volume = 0.3f;
        videoScreen = GameObject.Find("VideoScreen").GetComponent<RawImage>();

        moveButtonsHandler = GameObject.Find("MoveButtons" + PlayerPrefs.GetString("Chapter")).GetComponent<MoveButtonsHandler>();
        
        sfx = GameObject.Find("SFX").GetComponent<AudioSource>();
        sfx.volume = 0.3f;

        backing = GameObject.Find("Backing").GetComponent<RawImage>();
        normal_cam = GameObject.Find("Main Camera").GetComponent<Camera>();
        cam360 = GameObject.Find("Camera360").GetComponent<Camera>();
        button360 = GameObject.Find("View360").GetComponent<ButtonHandler>();   
        button360bg = button360.GetComponentInChildren<RawImage>();

        mat360 = Resources.Load<Material>("Materials/360 Mat");


        //Check if 360Video exists onsite
        /*if (!RemoteFileExists("https://nush-open-house.sgp1.cdn.digitaloceanspaces.com/" + location + "360vid.mp4"))
        {
            button360.enabled = false;
            button360bg.texture = Resources.Load<Texture>("Sprites/BigButton1Visited");
        }*/

        backdrop.texture = Resources.Load<Texture>("Sprites/" + location);
        if (PlayerPrefs.HasKey(location + "Phase"))
        {
            currentPhase = PlayerPrefs.GetInt(location + "Phase");
            ChangePhase(currentPhase);
        }
        else
        {
            currentPhase = 0;
            ChangePhase(0);
        }

        //Check if 360Picture exists
        if (Resources.Load<Texture>("Sprites/360Pictures/" + location + "360") != null)
        {
            button360.enabled = true;
            mat360.mainTexture = Resources.Load<Texture>("Sprites/360Pictures/" + location + "360");

        } else
        {
            button360.enabled = false;
            button360bg.texture = Resources.Load<Texture>("Sprites/BigButton1Visited");
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (!active360)
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

    }

    void ChangePhase(int phase)
    {
        print(phase);
        switch (phase)
        {
            case (0):
                LocationEntry(location); break;
            case (1):
                LocationIntro(location); break;
            case (2):
                LocationVideo(location); break;
            case (3):
                LocationOutro(location); break;
            case (4):
                LocationWaiting(location); break;
            case (5):
                LocationLandmarkIntro(PlayerPrefs.GetString("Landmark")); break;
            case (6):
                LocationLandmarkVideo(PlayerPrefs.GetString("Landmark")); break;
            default:
                LocationWaiting(location);
                currentPhase = 4; break;
        }
    }

    public void NextText()
    {
        currentTextIndex++;
        if (currentTextIndex >= texts.Length)
        {
            currentPhase++;
            ChangePhase(currentPhase);
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

    public void LocationEntry(string location)
    {
        StopAllCoroutines();
        StartCoroutine(LerpAnimations.instance.Fade(backdrop, 1f, 0.4f, 1.3f));
        StartCoroutine(LerpAnimations.instance.Fade(dialogueBox, 1f, 0.2f, 1.5f));
        
        TextAsset textData = Resources.Load<TextAsset>("Texts/" + location + "Entry");
        string reader = textData.text;
        texts = reader.Split('\n');
        currentTextIndex = 0;

        StartCoroutine(scrollTextCoroutine);
        PlayerPrefs.SetInt(location + "Phase", 0);
        Debug.Log(location);
        Debug.Log(texts);
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


    public void LocationVideo(string location)
    {
        print(location);
        if (PlayerPrefs.GetString("Chapter").Equals("1")) {
            StopAllCoroutines();
            StartCoroutine(LerpAnimations.instance.Fade(backdrop, 0f, 0.4f, 0f));
            StartCoroutine(LerpAnimations.instance.Fade(dialogueBox, 0f, 0.2f, 0f));
            StartCoroutine(LerpAnimations.instance.Fade(hongMeng, 0f, 0.2f, 0f));
            StartCoroutine(LerpAnimations.instance.Shift(GameObject.Find("Buttons").transform, Vector3.down * 2200, 400 * Time.deltaTime, 0f));
            nameText.text = "";
            dialogueText.text = "";
            StartCoroutine(LerpAnimations.instance.Move(GameObject.Find("VideoPhase").transform, GameObject.Find("Canvas").transform, 600 * Time.deltaTime, 0f));

            texts = new string[0];
            currentTextIndex = 0;
            StartCoroutine(PlayVideo("https://nush-open-house.sgp1.cdn.digitaloceanspaces.com/" + location + ".mp4"));
        }
        else NextText();
    }

    public void LocationView360(string location)
    {
        active360 = true;
        StopAllCoroutines();
        StartCoroutine(LerpAnimations.instance.Fade(backdrop, 0f, 0.4f, 0f));
        StartCoroutine(LerpAnimations.instance.Fade(backing, 0f, 0.2f, 0f));
        StartCoroutine(LerpAnimations.instance.Fade(dialogueBox, 0f, 0.2f, 0f));
        StartCoroutine(LerpAnimations.instance.Fade(hongMeng, 0f, 0.2f, 0f));


        nameText.alpha = 0;
        dialogueText.alpha = 0;

        normal_cam.enabled = false;
        cam360.enabled = true;

    }
    public void LocationExitView360()
    {
        active360 = false;

        StopAllCoroutines();
        StartCoroutine(LerpAnimations.instance.Fade(backdrop, 1f, 0.4f, 0f));
        StartCoroutine(LerpAnimations.instance.Fade(backing, 1f, 0.2f, 0f));

        if(PlayerPrefs.GetInt(location + "Phase") != 4)
        {
            StartCoroutine(LerpAnimations.instance.Fade(dialogueBox, 1f, 0.2f, 0f));

            if (PlayerPrefs.GetInt(location + "Phase") != 0)
            {
                StartCoroutine(LerpAnimations.instance.Fade(hongMeng, 1f, 0.2f, 0f));
            }

            nameText.alpha = 1;
            dialogueText.alpha = 1;
        }


        normal_cam.enabled = true;
        cam360.enabled = false;

    }


    public void LocationOutro(string location)
    {
        StopAllCoroutines();
        videoPlayer.Stop();
        StartCoroutine(LerpAnimations.instance.Shift(GameObject.Find("VideoPhase").transform, Vector3.down * 2200, 400 * Time.deltaTime, 0f));
        StartCoroutine(LerpAnimations.instance.Fade(hongMeng, 1f, 0.4f, 0f));
        StartCoroutine(LerpAnimations.instance.Fade(backdrop, 1f, 0.4f, 0f));
        StartCoroutine(LerpAnimations.instance.Fade(dialogueBox, 1f, 0.2f, 0f));
        if (GameObject.Find("Buttons") != null) StartCoroutine(LerpAnimations.instance.Move(GameObject.Find("Buttons").transform, GameObject.Find("Canvas").transform, 400 * Time.deltaTime, 0f));

        TextAsset textData = Resources.Load<TextAsset>("Texts/" + location + "Outro");
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
        PlayerPrefs.SetInt(location + "Phase", 3);
    }

    public void LocationWaiting(string location)
    {
        StopAllCoroutines();
        videoPlayer.Stop();
        StartCoroutine(LerpAnimations.instance.Fade(backdrop, 1f, 0.4f, 0f));
        StartCoroutine(LerpAnimations.instance.Fade(dialogueBox, 0f, 0.2f, 0f));
        StartCoroutine(LerpAnimations.instance.Fade(hongMeng, 0f, 0.2f, 0f));
        StartCoroutine(LerpAnimations.instance.Move(GameObject.Find("Landmarks").transform, GameObject.Find("Canvas").transform, 400 * Time.deltaTime, 0f));
        StartCoroutine(LerpAnimations.instance.Shift(GameObject.Find("VideoPhase").transform, Vector3.down * 2200, 400 * Time.deltaTime, 0f));
        
        //Not sure if this is important, if not just delete
        //if (GameObject.Find("Buttons") != null) StartCoroutine(LerpAnimations.instance.Move(GameObject.Find("Buttons").transform, GameObject.Find("Canvas").transform, 400 * Time.deltaTime, 0f));

        nameText.text = "";
        dialogueText.text = "";

        texts = new string[0];
        currentTextIndex = 0;
        if (GameObject.Find("RevisitButton") != null) ((ButtonHandler)GameObject.Find("RevisitButton").GetComponent("ButtonHandler")).StartBlink();
        PlayerPrefs.SetInt(location + "Phase", 4);
    }

    public void LocationLandmarkIntro(string landmark)
    {
        active360 = false;
        nameText.alpha = 1;
        dialogueText.alpha = 1;
        StopAllCoroutines();
        delay = 0f;
        StartCoroutine(LerpAnimations.instance.Fade(backdrop, 1f, 0.4f, 0f));
        StartCoroutine(LerpAnimations.instance.Fade(dialogueBox, 1f, 0.2f, 0f));
        StartCoroutine(LerpAnimations.instance.Fade(hongMeng, 1f, 0.2f, 0f));
        StartCoroutine(LerpAnimations.instance.Shift(GameObject.Find("Landmarks").transform, Vector3.left * 4400, 400 * Time.deltaTime, 0f));

        TextAsset textData = Resources.Load<TextAsset>("Texts/" + landmark);
        string reader = textData.text;
        texts = reader.Split('\n');
        currentTextIndex = 0;

        hongMengSprites = new string[texts.Length];
        for (int i = 0; i < texts.Length; i++)
        {
            hongMengSprites[i] = texts[i].Split(';')[0];
            texts[i] = texts[i].Split(';')[1];
        }

        currentTextIndex = 0;
        hongMeng.texture = Resources.Load<Texture>("Sprites/" + hongMengSprites[currentTextIndex]);
        nameText.text = hongMengSprites[currentTextIndex].Substring(0, hongMengSprites[currentTextIndex].Length - 1);

        scrollTextCoroutine = ScrollText(timePerChar, delay);
        StartCoroutine(scrollTextCoroutine);
        PlayerPrefs.SetInt(location + "Phase", 5);
    }

    public void LocationLandmarkVideo(string landmark)
    {
        if (!PlayerPrefs.GetString("Chapter").Equals("1")) {
            StopAllCoroutines();
            StartCoroutine(LerpAnimations.instance.Fade(backdrop, 1f, 0.4f, 0f));
            StartCoroutine(LerpAnimations.instance.Fade(dialogueBox, 0f, 0.2f, 0f));
            StartCoroutine(LerpAnimations.instance.Fade(hongMeng, 0f, 0.2f, 0f));
            StartCoroutine(LerpAnimations.instance.Shift(GameObject.Find("Buttons").transform, Vector3.down * 2200, 400 * Time.deltaTime, 0f));

            nameText.text = "";
            dialogueText.text = "";
            StartCoroutine(LerpAnimations.instance.Move(GameObject.Find("VideoPhase").transform, GameObject.Find("Canvas").transform, 400 * Time.deltaTime, 0f));

            texts = new string[0];
            currentTextIndex = 0;

            StartCoroutine(PlayVideo("https://nush-open-house.sgp1.cdn.digitaloceanspaces.com/" + landmark + ".mp4"));
        } else NextText();
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

    IEnumerator PlayVideo(string url)
    {
        videoScreen.texture = Resources.Load<Texture>("Sprites/CheckYourInternet");
        videoPlayer.url = url;
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
/*
    IEnumerator Play360Video(string url)
    {
        videoScreen360.texture = Resources.Load<Texture>("Sprites/CheckYourInternet");
        videoPlayer360.url = url;
        videoPlayer360.Prepare();
        while (!videoPlayer360.isPrepared)
        {
            yield return new WaitForSeconds(0.0166667f);
        }
        videoScreen360.color = new Color(1, 1, 1, 1);
        videoScreen360.texture = videoPlayer360.texture;
        videoPlayer360.Play();
        yield return null;
    }*/

    public void PausePlayVideo()
    {
        if (videoPlayer.isPaused)
        {
            videoPlayer.Play();
            audioSource.Play();
        }
        else
        {
            videoPlayer.Pause();
            audioSource.Pause();
        }
    }

    public void ResetVideo()
    {
        videoPlayer.Stop();
        StartCoroutine(PlayVideo("insert url here"));
    }



}
