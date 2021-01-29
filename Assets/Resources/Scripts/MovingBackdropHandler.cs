using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Events;

public class MovingBackdropHandler : MonoBehaviour
{
    public GameObject backdrop;
    public class PointerMovementEvent : UnityEvent<Vector3> { };
    public static PointerMovementEvent OnPointerMove = new PointerMovementEvent();
    Vector3 lastPointerPosition;

    // Use this for initialization
    void Start()
    {
        if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer) Destroy(GetComponent<MovingBackdropHandler>());
        OnPointerMove.AddListener((Vector3 delta) =>
        {
            backdrop.transform.position = Vector3.Lerp(backdrop.transform.position, backdrop.transform.position + delta * 0.025f, 50f * Time.deltaTime);
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
    }
}
