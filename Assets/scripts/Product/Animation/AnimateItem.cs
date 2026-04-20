using UnityEngine;
using System.Collections;

public class AnimateItem : MonoBehaviour
{
    private Coroutine magnifyNShrinkCoroutine;
    private float magnifyValue = 1.1f;
    private float delayBtnMagnification = 1.0f;

    private void OnEnable()
    {
        magnifyNShrinkCoroutine = StartCoroutine(MagnifyNShrink());
    }

    private void OnDisable()
    {
        StopCoroutine(magnifyNShrinkCoroutine);
    }

    IEnumerator MagnifyNShrink()
    {
        while (true)
        {
            transform.localScale = Vector3.one * magnifyValue;
            yield return new WaitForSeconds(delayBtnMagnification);
            transform.localScale = Vector3.one;
            yield return new WaitForSeconds(delayBtnMagnification);

        }
        
    }
}
