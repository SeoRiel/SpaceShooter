using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(EnemyFOV))]
public class FOVEditor : Editor
{
    private void OnSceneGUI()
    {
        // EmenyFOV class를 참조
        EnemyFOV fov = (EnemyFOV)target;

        // 원주 위의 시작점의 좌표를 계산(주어진 각도의 1/2)
        Vector3 fromAnglePosition = fov.CirclePoint(-fov.viewAngle * 0.5f);

        // 원의 색상을 흰색으로 지정
        Handles.color = Color.white;

        // 외곽선만 표현하는 원반을 그림
        Handles.DrawWireDisc(fov.transform.position, Vector3.up, fov.viewRange);

        // 부채꼴의 색상을 지정
        Handles.color = new Color(1, 1, 1, 0.2f);

        // 부채꼴의 색상을 지정
        Handles.DrawSolidArc(fov.transform.position, Vector3.up, fromAnglePosition, fov.viewAngle, fov.viewRange);

        // 시야각의 텍스트 표시
        Handles.Label(fov.transform.position + (fov.transform.forward * 2.0f), fov.viewAngle.ToString());
    }
}
