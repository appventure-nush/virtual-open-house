using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MoveButtonsHandler : MonoBehaviour
{
    LocationSceneHandler locationSceneHandler;
    private void Start()
    {
        locationSceneHandler = GameObject.Find("LocationSceneHandler").GetComponent<LocationSceneHandler>();
    }
    public void Move()
    {
        if (!locationSceneHandler.moving)
        {
            StopAllCoroutines();
            StartCoroutine(LerpAnimations.instance.Move(transform, GameObject.Find("Canvas").transform, 600 * Time.deltaTime, 0f));
            locationSceneHandler.moving = true;
        }
        else
        {
            StopAllCoroutines();
            StartCoroutine(LerpAnimations.instance.Shift(transform, Vector3.right * 4000, 600 * Time.deltaTime, 0f));
            locationSceneHandler.moving = false;
        }
    }
}
