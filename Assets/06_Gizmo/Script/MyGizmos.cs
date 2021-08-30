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
            // ����� ���� ����
            Gizmos.color = color;
            // ��ü ����� ����� ����, DrawSphere(���� ��ġ, ������)
            Gizmos.DrawSphere(transform.position, radius);
        }
        else
        {
            // ����� ���� ����
            Gizmos.color = color;
            // Enemy Image ������ ǥ��
            Gizmos.DrawIcon(transform.position + Vector3.up * 1.0f, wayPointFile, true);
            Gizmos.DrawWireSphere(transform.position, radius);
        }
    }
}
