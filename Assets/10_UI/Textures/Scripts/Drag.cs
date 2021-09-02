using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Drag : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    public static GameObject draggingItem = null;

    private Transform itemTransform;
    private Transform inventoryTransform;
    private Transform itemListTransform;
    private CanvasGroup canvasGroup;

    // Start is called before the first frame update
    private void Start()
    {
        // 드래그 이벤트가 발생하면 아이템의 위치를 마우스 커서의 위치로 변경
        itemTransform = GetComponent<Transform>();
        inventoryTransform = GameObject.Find("Inventory").GetComponent<Transform>();
        itemListTransform = GameObject.Find("ItemList").GetComponent<Transform>();

        // Canvas Group Component 추출
        canvasGroup = GetComponent<CanvasGroup>();
    }
    // 드래그 이벤트
    public void OnDrag(PointerEventData eventData)
    {
        itemTransform.position = Input.mousePosition;
    }

    // 드래그를 시작할 때 한 번 호출되는 이벤트
    public void OnBeginDrag(PointerEventData eventData)
    {
        // 부모를 Inventory로 변경
        this.transform.SetParent(inventoryTransform);
        draggingItem = this.gameObject;

        // 드래그가 시작되면 다른 UI Event를 받지 않도록 설정
        canvasGroup.blocksRaycasts = false;
    }

    // 드래그가 종료했을 때, 한 번 호출되는 이벤트 
    public void OnEndDrag(PointerEventData eventData)
    {
        // 드래그가 종료되면 드래그 아이템을 null로 변경
        draggingItem = null;

        // 드래그가 종료되면 다른 UI Event를 받도록 설정
        canvasGroup.blocksRaycasts = true;

        // 슬롯에 드래그를 하지 않았을 때, 원래 위치로 되돌림
        if (itemTransform.parent == inventoryTransform)
        {
            itemTransform.SetParent(itemListTransform.transform);
        }
    }
}
