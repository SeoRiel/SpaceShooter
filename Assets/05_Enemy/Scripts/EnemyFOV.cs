using UnityEngine;

public class EnemyFOV : MonoBehaviour
{
    // 적 캐릭터의 추적 사정 거리의 범위
    public float viewRange = 15.0f;
    [Range(0, 360)]

    // 적 캐릭터의 시야각
    public float viewAngle = 120.0f;

    private Transform enemyTransform;
    private Transform playerTransform;
    private int playerLayer;
    private int obstacleLayer;
    private int layerMask;

    private void Start()
    {
        // Component 추출
        enemyTransform = GetComponent<Transform>();
        playerTransform = GameObject.FindGameObjectWithTag("PLAYER").transform;

        // Layer Mask 값 계산
        playerLayer = LayerMask.NameToLayer("PLAYER");
        obstacleLayer = LayerMask.NameToLayer("OBSTACLE");
        layerMask = 1 << playerLayer | 1 << obstacleLayer;
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

    public bool isTraceLayer()
    {
        bool isTrace = false;

        // 추적 반경 범위 안에서 Player Character 추출
        Collider[] colliders = Physics.OverlapSphere(enemyTransform.position, viewRange, 1 << playerLayer);

        //배열의 개수가 1일 때, PlayerCharacter가 범위 안에 있다고 판단
        if(colliders.Length == 1)
        {
            // Enemy Charactor와 Player Character의 방향 벡터를 정규화하여 계산
            Vector3 direction = (playerTransform.position - enemyTransform.position).normalized;

            // Enemy Charactor의 시야각에 Player Character가 들어와있는지 판단
            if(Vector3.Angle(enemyTransform.forward, direction) < viewAngle * 0.5f)
            {
                isTrace = true;
            }
        }

        return isTrace;
    }

    public bool isViewPlayer()
    {
        bool isView = false;
        RaycastHit rayHit;

        // Enemy Character와 Player Character 사이의 방향 벡터를 정규화하여 계산
        Vector3 direction = (playerTransform.position - enemyTransform.position).normalized;

        // Raycast를 발사하여 장애물이 있는지 판단
        if(Physics.Raycast(enemyTransform.position, direction, out rayHit, viewRange, layerMask))
        {
            isView = rayHit.collider.CompareTag("PLAYER");
        }

        return isView;
    }
}
