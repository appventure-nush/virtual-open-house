using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using TMPro;
using System;

public class SliderHandler : MonoBehaviour
    , IPointerEnterHandler
     , IPointerExitHandler
     , IPointerDownHandler
     , IPointerUpHandler
{
    RawImage surface;
    Vector3 initialSurfaceScale;
    RawImage bar;

    VideoPlayer videoPlayer;

    float width = 3060f;
    float initialX;

    bool draggable = false;
    bool resume = false;

    public class PointerMovementEvent : UnityEvent<Vector3> { };
    public static PointerMovementEvent OnPointerMove = new PointerMovementEvent();
    Vector3 lastPointerPosition;

    // Start is called before the first frame update
    void Start()
    {
        SetSlider();
        initialX = transform.localPosition.x;
        lastPointerPosition = Input.mousePosition;
        videoPlayer = GameObject.Find("VideoPhase").GetComponent<VideoPlayer>();

        OnPointerMove.AddListener((Vector3 delta) =>
        { 
            if (draggable && transform.localPosition.x + delta.x * 3840f / Screen.width >= initialX && transform.localPosition.x + delta.x * 3840f / Screen.width <= width + initialX + 10f)
            {
                transform.localPosition += new Vector3(delta.x * 3840f / Screen.width, 0f, 0f);
                videoPlayer.frame = Convert.ToInt64((transform.localPosition.x - initialX) / width * videoPlayer.frameCount);
            } 
        });
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 pointerDelta = Input.mousePosition - lastPointerPosition;
        lastPointerPosition = Input.mousePosition;

        if (pointerDelta.magnitude > 0f)
        {
            OnPointerMove.Invoke(pointerDelta);
        }

        if (videoPlayer.isPrepared && !draggable)
        {
            Vector3 videoDelta = new Vector3(width * (Convert.ToSingle(videoPlayer.frame + 1) / Convert.ToSingle(videoPlayer.frameCount)), transform.localPosition.y);
            transform.localPosition = videoDelta;
        }
        if (videoPlayer.frame + 2 >= Convert.ToInt64(videoPlayer.frameCount))
        {
            PlayerPrefs.SetString("ExitVideoStatus", "YouAreHere");
            GameObject.Find("ExitVideoButton").GetComponent<ButtonHandler>().SetButton();
        }

        else
        {
            PlayerPrefs.DeleteKey("ExitVideoStatus");
            GameObject.Find("ExitVideoButton").GetComponent<ButtonHandler>().SetButton();
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        StopAllCoroutines();
        surface.texture = Resources.Load<Texture>("Sprites/Slider" + PlayerPrefs.GetString("Chapter") + "Selected");
        StartCoroutine(LerpAnimations.instance.Fade(surface, 1f, 0.2f, 0f));
        StartCoroutine(LerpAnimations.instance.Scale(surface.transform, 2.1f, 10f, 0f));
        bar.color = new Color(bar.color.r, bar.color.g, bar.color.b, 0.5f);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        StopAllCoroutines();
        StartCoroutine(LerpAnimations.instance.Fade(surface, 1f, 0.2f, 0f));
        StartCoroutine(LerpAnimations.instance.Scale(surface.transform, 2.6f, 5f, 0f));
        bar.color = new Color(bar.color.r, bar.color.g, bar.color.b, 1);
        draggable = true;
        if (!videoPlayer.isPaused)
        {
            videoPlayer.Pause();
            resume = true;
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        StopAllCoroutines();
        StartCoroutine(LerpAnimations.instance.Fade(surface, 0.3f, 0.5f, 0f));
        StartCoroutine(LerpAnimations.instance.Scale(surface.transform, 1f, 10f, 0f));
        bar.color = new Color(bar.color.r, bar.color.g, bar.color.b, 0.2f);
        draggable = false;
        if (resume) videoPlayer.Play();
        resume = false;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (!draggable) { 
            StopAllCoroutines();
            surface.texture = Resources.Load<Texture>("Sprites/Slider" + PlayerPrefs.GetString("Chapter"));
            StartCoroutine(LerpAnimations.instance.Fade(surface, 0.5f, 0.2f, 0f));
            StartCoroutine(LerpAnimations.instance.Scale(surface.transform, 1f, 10f, 0f));
            bar.color = new Color(bar.color.r, bar.color.g, bar.color.b, 0.2f);
        }
    }

    void TransformAndImageReset()
    {
        surface.color = new Color(surface.color.r, surface.color.g, surface.color.b, 0.5f);
        surface.texture = Resources.Load<Texture>("Sprites/Slider" + PlayerPrefs.GetString("Chapter"));
        surface.transform.localScale = new Vector3(1, 1, 1);
        surface.transform.eulerAngles = new Vector3(0, 0, 0);
        StopAllCoroutines();
    }

    public void SetSlider()
    {
        bar = gameObject.transform.parent.GetChild(0).GetComponent<RawImage>();
        surface = gameObject.transform.parent.GetChild(1).GetComponent<RawImage>();
        initialSurfaceScale = surface.transform.localScale;
    }
}
