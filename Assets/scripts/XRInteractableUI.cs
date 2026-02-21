using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class XRUIFeedback : MonoBehaviour,
    IPointerEnterHandler,
    IPointerExitHandler,
    IPointerClickHandler
{
    private Image image;

    public Color normalColor = Color.white;
    public Color hoverColor = Color.cyan;
    public Color clickColor = Color.green;

    void Awake()
    {
        image = GetComponent<Image>();
        image.color = normalColor;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        image.color = hoverColor;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        image.color = normalColor;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        image.color = clickColor;
    }
}
