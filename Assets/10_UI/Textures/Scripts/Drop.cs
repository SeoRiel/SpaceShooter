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
            // 슬롯에 추가된 아이템을 GameData에 추가 하기 위해 AddItem을 호출
            Item item = Drag.draggingItem.GetComponent<ItemInfomation>().itemData;
            GameManager.instance.AddItem(item);
        }
    }
}
