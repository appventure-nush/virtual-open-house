using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LerpAnimations : MonoBehaviour
{

    public static LerpAnimations instance;

    void Awake()
    {
        instance = this;
    }

    internal IEnumerator Scale(Transform transform, float endScale, float smoothing, float delay)
    {
        string i = transform.localScale.ToString();
        yield return new WaitForSeconds(delay);
        Vector3 finalScale = new Vector3(endScale, endScale, endScale);
        while (Vector3.Distance(transform.localScale, finalScale) > 0.05f)
        {
            transform.localScale = Vector3.Lerp(transform.localScale, finalScale, Time.deltaTime * smoothing);
            yield return null;
        }
    }

    internal IEnumerator Rotate(Transform transform, float endAngle, float smoothing, float delay)
    {
        yield return new WaitForSeconds(delay);
        Vector3 finalAngle = new Vector3(0, 0, endAngle);
        while (Math.Abs(transform.eulerAngles.z - endAngle) > 0.05f)
        {
            transform.eulerAngles = Vector3.Lerp(transform.eulerAngles, finalAngle, Time.deltaTime * smoothing);
            yield return null;
        }
    }

    internal IEnumerator Fade(RawImage rawImage, float endAlpha, float time, float delay)
    {
        yield return new WaitForSeconds(delay);
        float startTime = Time.time;
        float startAlpha = rawImage.color.a;
        while (Time.time - startTime < time)
        {
            rawImage.color = new Color(rawImage.color.r, rawImage.color.g, rawImage.color.b, Mathf.SmoothStep(startAlpha, endAlpha, (Time.time - startTime) / time));
            yield return null;
        }

    }

    internal IEnumerator ScaleLoop(Transform transform, float endScale, float smoothing, float delay)
    {
        yield return new WaitForSeconds(delay);
        Vector3 originalScale = transform.localScale;
        Vector3 finalScale = new Vector3(endScale, endScale, endScale);
        bool initDir = true;
        while (transform != null)
        {
            if (initDir)
            {
                transform.localScale = Vector3.Lerp(transform.localScale, finalScale, Time.deltaTime * smoothing);
                yield return null;
            }
            else
            {
                transform.localScale = Vector3.Lerp(transform.localScale, originalScale, Time.deltaTime * smoothing);
                yield return null;
            }

            if (Vector3.Distance(transform.localScale, finalScale) < 0.05f) initDir = false;
            else if (Vector3.Distance(transform.localScale, originalScale) < 0.05f) initDir = true;
            else continue;
        }
    }

    internal IEnumerator Move(Transform transform, Transform target, float smoothing, float delay)
    {
        yield return new WaitForSeconds(delay);
        while (transform != null && Vector3.Distance(this.transform.position, target.position) > 0.05f)
        {
            transform.position = Vector3.Lerp(transform.position, target.position, smoothing * Time.deltaTime);
            yield return null;
        }
        yield break;
    }

    internal IEnumerator MoveAndDestroy(Transform transform, Transform target, float smoothing, float delay, float destroyAfter)
    {
        yield return new WaitForSeconds(delay);
        float start = 0f;
        while (transform != null && Vector3.Distance(this.transform.position, target.position) > 0.05f)
        {
            transform.position = Vector3.Lerp(transform.position, target.position, smoothing * Time.deltaTime);
            if (start > destroyAfter)
            {
                Destroy(transform.gameObject);
                yield break;
            }
            yield return null;
        }
        yield break;
    }

    internal IEnumerator Shift(Transform transform, Vector3 target, float smoothing, float delay)
    {
        yield return new WaitForSeconds(delay);
        while (transform != null && Vector3.Distance(this.transform.localPosition, target) > 0.05f)
        {
            transform.localPosition = Vector3.Lerp(transform.localPosition, target, smoothing * Time.deltaTime);
            yield return null;
        }
    }

    internal IEnumerator ShiftAndDestroy(Transform transform, Vector3 target, float smoothing, float delay, float destroyAfter)
    {
        yield return new WaitForSeconds(delay);
        float start = 0f;
        while (transform != null && Vector3.Distance(this.transform.localPosition, target) > 0.05f)
        {
            transform.localPosition = Vector3.Lerp(transform.localPosition, target, smoothing * Time.deltaTime);
            start += Time.deltaTime;
            if (start > destroyAfter)
            {
                Destroy(transform.gameObject);
                yield break;
            }
            yield return null;
        }
    }
}

