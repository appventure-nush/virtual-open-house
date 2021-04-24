using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Viwer360 : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.RotateAround(transform.position, -Vector3.up, Input.GetAxis("Mouse X"));
        transform.RotateAround(transform.position, transform.right, Input.GetAxis("Mouse Y"));

    }
}
