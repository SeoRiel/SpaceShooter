using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyGizmos : MonoBehaviour
{
    public Color color = Color.yellow;
    public float radius = 0.1f;

    private void OnDrawGizmos()
    {
        Gizmos.color = color;                           // 기즈모 색상
        Gizmos.DrawSphere(transform.position, radius);  // 구체 모양의 기즈모 생성.
                                                        // DrawSphere(생성 위치, 반지름)
    }
}
