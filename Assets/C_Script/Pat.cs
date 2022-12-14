using UnityEngine;
using UnityEngine.EventSystems;

public class Pat : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public GameObject spineBaseGo;

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        spineBaseGo.GetComponent<Control>().SetPatting(true);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        spineBaseGo.GetComponent<Control>().SetPatting(false);
    }
}
