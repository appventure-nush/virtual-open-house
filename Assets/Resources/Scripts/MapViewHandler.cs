using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MapViewHandler : MonoBehaviour
{
    public GameObject MapButtonPrefab;
    bool viewingMap = false;

    GameObject mapViewObj;
    // Start is called before the first frame update
    void Start()
    {
        if (!PlayerPrefs.GetString("Chapter").Equals("1"))
        {
            mapViewObj = GameObject.Find("MapViewerObj");

            TextAsset textData = Resources.Load<TextAsset>("Texts/Chapter" + PlayerPrefs.GetString("Chapter") + "MapView");
            string reader = textData.text;
            string[] maplocations = reader.Split('\n');
            foreach (string maplocation in maplocations)
            {
                string[] mapArray = maplocation.Split(';');

                //Dont mind the fact the landmarkPrefab... shush...
                GameObject mapButton = Instantiate(MapButtonPrefab, transform);

                mapButton.name = mapArray[0] + "Button";
                mapButton.GetComponentInChildren<TextMeshProUGUI>().text = mapArray[0];
                //landmarkButton.transform.parent = transform;
                mapButton.transform.SetParent(transform);
                mapButton.transform.localPosition = new Vector3(float.Parse(mapArray[1]), float.Parse(mapArray[2]));
                mapButton.transform.localScale = new Vector3(1f, 1f, 2f);

                GameObject mapIcon = new GameObject();
                RawImage mapIconTexture = mapIcon.AddComponent<RawImage>();
                mapIconTexture.texture = Resources.Load<Texture>("Sprites/Landmark");
                //landmarkIcon.transform.parent = transform;
                mapIcon.transform.SetParent(transform, false);
                mapIcon.transform.localPosition = new Vector3(float.Parse(mapArray[1]), float.Parse(mapArray[2]) + 150f);
                mapIcon.transform.localScale = new Vector3(1.5f, 1.5f, 2f);
                mapIcon.SetActive(true);

            }
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ToggleMapView()
    {
        if (!viewingMap)
        {
            StopAllCoroutines();
            StartCoroutine(LerpAnimations.instance.Move(mapViewObj.transform, GameObject.Find("Canvas").transform, 600 * Time.deltaTime, 0f));
            viewingMap = true;
        } else
        {
            StopAllCoroutines();
            //For some reason I cant get this to work >:(((
            StartCoroutine(LerpAnimations.instance.Shift(mapViewObj.transform, Vector3.down * 2400, 600 * Time.deltaTime, 0f));
            
            viewingMap = false;
        }

        
    }

    public void ViewMapOff()
    {
        if (!PlayerPrefs.GetString("Chapter").Equals("1"))
        {
            StopAllCoroutines();
            StartCoroutine(LerpAnimations.instance.Shift(mapViewObj.transform, Vector3.down * 2400, 600 * Time.deltaTime, 0f));

            viewingMap = false;
        }

    }
}
