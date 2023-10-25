using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class EnemySlot : MonoBehaviour, IDropHandler
{
    [SerializeField]
    private Transform floor;
    public void OnDrop(PointerEventData eventData)
    {
        //Debug.Log("OnDrop");
            if (eventData.pointerDrag != null)
        {
            eventData.pointerDrag.transform.SetParent(floor, false);
            eventData.pointerDrag.GetComponent<RectTransform>().position = GetComponent<RectTransform>().position + (Vector3.left * 300);
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
