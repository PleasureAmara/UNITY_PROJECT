using UnityEngine;
using System.Collections;

public class AnimateItem : MonoBehaviour
{
    private Coroutine magnifyNShrinkCoroutine;

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
            transform.localScale = Vector3.one * 1.2f;
            yield return new WaitForSeconds(0.5f);
            transform.localScale = Vector3.one;
            yield return new WaitForSeconds(0.5f);

        }
        
    }
}
