using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(EnemyFOV))]
public class FOVEditor : Editor
{
    private void OnSceneGUI()
    {
        // EmenyFOV class�� ����
        EnemyFOV fov = (EnemyFOV)target;

        // ���� ���� �������� ��ǥ�� ���(�־��� ������ 1/2)
        Vector3 fromAnglePosition = fov.CirclePoint(-fov.viewAngle * 0.5f);

        // ���� ������ ������� ����
        Handles.color = Color.white;

        // �ܰ����� ǥ���ϴ� ������ �׸�
        Handles.DrawWireDisc(fov.transform.position, Vector3.up, fov.viewRange);

        // ��ä���� ������ ����
        Handles.color = new Color(1, 1, 1, 0.2f);

        // ��ä���� ������ ����
        Handles.DrawSolidArc(fov.transform.position, Vector3.up, fromAnglePosition, fov.viewAngle, fov.viewRange);

        // �þ߰��� �ؽ�Ʈ ǥ��
        Handles.Label(fov.transform.position + (fov.transform.forward * 2.0f), fov.viewAngle.ToString());
    }
}
