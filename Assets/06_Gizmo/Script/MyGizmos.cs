using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyGizmos : MonoBehaviour
{
    public enum Type
    {
        NORMAL, WAYPOINT
    }
    private const string wayPointFile = "Enemy";

    public Color color = Color.yellow;
    public Type type = Type.NORMAL;
    public float radius = 0.1f;

    private void OnDrawGizmos()
    {
        if(type == Type.NORMAL)
        {
            // 기즈모 색상 설정
            Gizmos.color = color;
            // 구체 모양의 기즈모 생성, DrawSphere(생성 위치, 반지름)
            Gizmos.DrawSphere(transform.position, radius);
        }
        else
        {
            // 기즈모 색상 설정
            Gizmos.color = color;
            // Enemy Image 파일을 표시
            Gizmos.DrawIcon(transform.position + Vector3.up * 1.0f, wayPointFile, true);
            Gizmos.DrawWireSphere(transform.position, radius);
        }
    }
}
