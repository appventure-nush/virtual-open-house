using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LandmarksHandler : MonoBehaviour
{
    public GameObject landmarkPrefab;

    // Start is called before the first frame update
    void Start()
    {
        LoadLandmarks();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void LoadLandmarks()
    {
        TextAsset textData = Resources.Load<TextAsset>("Texts/" + PlayerPrefs.GetString("Location") + "Landmarks");
        string reader = textData.text;
        string[] landmarks = reader.Split('\n');
        foreach (string landmark in landmarks)
        {
            string[] landmarkArray = landmark.Split(',');
            GameObject landmarkButton = Instantiate(landmarkPrefab, transform);
            landmarkButton.name = landmarkArray[0] + "Button";
            landmarkButton.GetComponentInChildren<TextMeshProUGUI>().text = landmarkArray[0];
            //landmarkButton.transform.parent = transform;
            landmarkButton.transform.SetParent(transform);
            landmarkButton.transform.localPosition = new Vector3(float.Parse(landmarkArray[1]), float.Parse(landmarkArray[2]));

            GameObject landmarkIcon = new GameObject();
            RawImage landmarkIconTexture = landmarkIcon.AddComponent<RawImage>();
            landmarkIconTexture.texture = Resources.Load<Texture>("Sprites/Landmark");
            //landmarkIcon.transform.parent = transform;
            landmarkIcon.transform.SetParent(transform, false);
            landmarkIcon.transform.localPosition = new Vector3(float.Parse(landmarkArray[1]), float.Parse(landmarkArray[2]) + 200f);
            landmarkIcon.transform.localScale = new Vector3(2f, 2f, 2f);
            landmarkIcon.SetActive(true);
        }
    }
}
