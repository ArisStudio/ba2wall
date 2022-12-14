using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Talk : MonoBehaviour, IPointerClickHandler
{
    public GameObject spineBaseGo;

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        spineBaseGo.GetComponent<Control>().SetTalking();
    }
}
