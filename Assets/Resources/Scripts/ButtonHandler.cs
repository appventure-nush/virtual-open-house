using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using TMPro;

public class ButtonHandler : MonoBehaviour
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

    LocationSceneHandler locationSceneHandler;
    MoveButtonsHandler moveButtonsHandler;
    
    // Start is called before the first frame update
          
    void Start()
    {
        cover = GameObject.Find("Cover").transform;
        coverTarget = GameObject.Find("Canvas").transform;

        StartCoroutine(StillTransiting());
        SetButton();

        locationSceneHandler = GameObject.Find("LocationSceneHandler").GetComponent<LocationSceneHandler>();
        moveButtonsHandler = GameObject.Find("MoveButtons" + PlayerPrefs.GetString("Chapter")).GetComponent<MoveButtonsHandler>();
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
                case ("LetsGoButton"):
                    Transit();
                    LoadScene("ChapterSelectScene"); break;
                case ("Pause/PlayButton"):
                    locationSceneHandler.PausePlayVideo(); break;
                case ("ResetButton"):
                    locationSceneHandler.ResetVideo(); break;
                case ("ExitVideoButton"):
                    locationSceneHandler.NextText();
                    PlayerPrefs.DeleteKey("ExitVideoStatus"); break;
                case ("MoveButton"):
                    moveButtonsHandler.Move(); break;
                case ("ConcourseButton"):
                    MoveTo("Concourse"); break;
                case ("AuditoriumButton"):
                    MoveTo("Auditorium"); break;
                case ("BoardingButton"):
                    MoveTo("Boarding"); break;
                case ("LibraryButton"):
                    MoveTo("Library"); break;
                case ("Year 1 ClassroomsButton"):
                    MoveTo("Year 1 Classrooms"); break;
                case ("CanteenButton"):
                    MoveTo("Canteen"); break;
                case ("Art RoomButton"):
                    MoveTo("Art Room"); break;
                case ("D&E LabButton"):
                    MoveTo("D&E Lab"); break;
                case ("Level 3 LabsButton"):
                    MoveTo("Level 3 Labs"); break;
                case ("Computer LabsButton"):
                    MoveTo("Computer Labs"); break;
                case ("Synthetic Chemistry LabButton"):
                    MoveTo("Synthetic Chemistry Lab"); break;
                case ("Life Sciences LabButton"):
                    MoveTo("Life Sciences Lab"); break;
                case ("Clean Energy LabButton"):
                    MoveTo("Clean Energy Lab"); break;
                case ("Analytical Chemistry LabButton"):
                    MoveTo("Analytical Chemistry Lab"); break;
                case ("Applied Tech LabButton"):
                    MoveTo("Applied Tech Lab"); break;
                case ("Math ICT LabButton"):
                    MoveTo("Math ICT Lab"); break;
                case ("Cookery LabButton"):
                    MoveTo("Cookery Lab"); break;
                case ("HallButton"):
                    MoveTo("Hall"); break;
                case ("TheatretteButton"):
                    MoveTo("Theatrette"); break;
                case ("LoungeButton"):
                    MoveTo("Lounge"); break;
                case ("Seminar RoomsButton"):
                    MoveTo("Seminar Rooms"); break;
                case ("Netball CourtButton"):
                    MoveTo("Netball Court"); break;
                case ("Basketball CourtButton"):
                    MoveTo("Basketball Court"); break;
                case ("T&F, Tennis CourtsButton"):
                    MoveTo("T&F, Tennis Courts"); break;
                case ("GymButton"):
                    MoveTo("Gym"); break;
                case ("ISHButton"):
                    MoveTo("ISH"); break;
                case ("Music RoomButton"):
                    MoveTo("Music Room"); break;
                case ("EcopondButton"):
                    MoveTo("Ecopond"); break;
                case ("ChapterSelectButton"):
                    LoadScene("ChapterSelectScene");
                    Transit(); break;
                case ("RevisitButton"):
                    PlayerPrefs.SetInt(PlayerPrefs.GetString("Location") + "Phase", 0);
                    MoveTo(PlayerPrefs.GetString("Location"));
                    Transit(); break;
                default:
                    PlayerPrefs.SetString("Landmark", name.Substring(0, name.Length - 6));
                    PlayerPrefs.SetString(PlayerPrefs.GetString("Landmark") + "Status", "Visited");
                    SetButton();
                    locationSceneHandler.NextText(); break;
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
        surface.transform.eulerAngles = new Vector3(0, 0, 0);
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
        if (!PlayerPrefs.HasKey("Chapter"))
        {
            surfaceTexture = "Sprites/BigButton1" + PlayerPrefs.GetString(name.Substring(0, name.Length - 6) + "Status");
        }
        else
        {
            surfaceTexture = "Sprites/BigButton" + PlayerPrefs.GetString("Chapter") + PlayerPrefs.GetString(name.Substring(0, name.Length - 6) + "Status");
        }
        surface.texture = Resources.Load<Texture>(surfaceTexture);
        text = this.gameObject.transform.GetChild(1).GetComponent<TextMeshProUGUI>();
        initialTextScale = text.transform.localScale;
    }

    public void Click()
    {
        if (!transiting) OnPointerUp(null);
    }
}


