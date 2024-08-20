using UnityEngine;
using UnityEngine.EventSystems;

public class HoverBtn : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] Sprite baseImage;
    [SerializeField] Sprite hoverImage;

    private void Start()
    {
        GetComponent<UnityEngine.UI.Image>().sprite = baseImage;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        GetComponent<UnityEngine.UI.Image>().sprite = hoverImage;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        GetComponent<UnityEngine.UI.Image>().sprite = baseImage;
    }
}