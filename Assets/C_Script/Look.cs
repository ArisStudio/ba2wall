using UnityEngine;
using UnityEngine.EventSystems;

public class Look : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
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
        spineBaseGo.GetComponent<Control>().SetLooking(true);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        spineBaseGo.GetComponent<Control>().SetLooking(false);
    }
}
