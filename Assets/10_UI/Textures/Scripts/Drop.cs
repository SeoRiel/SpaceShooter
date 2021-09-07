using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using DataInfomation;

public class Drop : MonoBehaviour, IDropHandler
{
    public void OnDrop(PointerEventData eventData)
    {
        if(transform.childCount == 0)
        {
            Drag.draggingItem.transform.SetParent(this.transform);
            // ���Կ� �߰��� �������� GameData�� �߰� �ϱ� ���� AddItem�� ȣ��
            Item item = Drag.draggingItem.GetComponent<ItemInfomation>().itemData;
            GameManager.instance.AddItem(item);
        }
    }
}
