using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartupSequence : MonoBehaviour
{
    public GameObject title;
    public GameObject cover;
    public Transform target;

    static bool introDone = false;

    void Awake()
    {
        if (introDone)
        {
            GameObject appVenture = GameObject.Find("AppVenture");
            {
                if (appVenture)
                {
                    cover.transform.position = appVenture.transform.position;
                    Destroy(appVenture);
                }
            }
        }
    }

    // Use this for initialization
    void Start()
    {
        PlayerPrefs.SetString("43046721 y0u 4re th3 0ne", "password is t4nrU1f3nG");
        StartCoroutine("Lerp");
        if (title != null) StartCoroutine(LerpAnimations.instance.ScaleLoop(title.transform, 1.1f, 30 * Time.deltaTime, 0f));
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnDestroy()
    {
        introDone = true;
    }

    IEnumerator Lerp()
    {
        yield return new WaitForSeconds(1f);
        while (Vector3.Distance(transform.position, target.position) > 0.05f)
        {
            transform.position = Vector3.Lerp(transform.position, target.position, 10 * Time.deltaTime);
            yield return null;
        }

    }
}
