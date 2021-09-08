using UnityEngine;

public class EnemyFOV : MonoBehaviour
{
    // 적 캐릭터의 추적 사정 거리의 범위
    public float viewRange = 15.0f;
    [Range(0, 360)]

    // 적 캐릭터의 시야각
    public float viewAngle = 120.0f;

    // Start is called before the first frame update
    private void Start()
    {
        
    }

    // 주어진 각도에 의해 원주 위의 점의 좌표값을 계산하는 함수
    public Vector3 CirclePoint(float angle)
    {
        // 로컬 좌표계 기준으로 설정하기 위해 적 캐릭터의 Y축 회전값을 더함
        // Mathf.Deg2Rad == 2pi / 360
        // Mathf.Rad2Deg == 180 / pi
        angle += transform.eulerAngles.y;
        return new Vector3(Mathf.Sin(angle * Mathf.Deg2Rad), 0, Mathf.Cos(angle * Mathf.Deg2Rad));
    }
}
