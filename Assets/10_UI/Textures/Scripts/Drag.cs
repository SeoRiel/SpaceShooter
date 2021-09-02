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
        // �巡�� �̺�Ʈ�� �߻��ϸ� �������� ��ġ�� ���콺 Ŀ���� ��ġ�� ����
        itemTransform = GetComponent<Transform>();
        inventoryTransform = GameObject.Find("Inventory").GetComponent<Transform>();
        itemListTransform = GameObject.Find("ItemList").GetComponent<Transform>();

        // Canvas Group Component ����
        canvasGroup = GetComponent<CanvasGroup>();
    }
    // �巡�� �̺�Ʈ
    public void OnDrag(PointerEventData eventData)
    {
        itemTransform.position = Input.mousePosition;
    }

    // �巡�׸� ������ �� �� �� ȣ��Ǵ� �̺�Ʈ
    public void OnBeginDrag(PointerEventData eventData)
    {
        // �θ� Inventory�� ����
        this.transform.SetParent(inventoryTransform);
        draggingItem = this.gameObject;

        // �巡�װ� ���۵Ǹ� �ٸ� UI Event�� ���� �ʵ��� ����
        canvasGroup.blocksRaycasts = false;
    }

    // �巡�װ� �������� ��, �� �� ȣ��Ǵ� �̺�Ʈ 
    public void OnEndDrag(PointerEventData eventData)
    {
        // �巡�װ� ����Ǹ� �巡�� �������� null�� ����
        draggingItem = null;

        // �巡�װ� ����Ǹ� �ٸ� UI Event�� �޵��� ����
        canvasGroup.blocksRaycasts = true;

        // ���Կ� �巡�׸� ���� �ʾ��� ��, ���� ��ġ�� �ǵ���
        if (itemTransform.parent == inventoryTransform)
        {
            itemTransform.SetParent(itemListTransform.transform);
        }
    }
}
