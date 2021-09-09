using UnityEngine;

public class EnemyFOV : MonoBehaviour
{
    // �� ĳ������ ���� ���� �Ÿ��� ����
    public float viewRange = 15.0f;
    [Range(0, 360)]

    // �� ĳ������ �þ߰�
    public float viewAngle = 120.0f;

    private Transform enemyTransform;
    private Transform playerTransform;
    private int playerLayer;
    private int obstacleLayer;
    private int layerMask;

    private void Start()
    {
        // Component ����
        enemyTransform = GetComponent<Transform>();
        playerTransform = GameObject.FindGameObjectWithTag("PLAYER").transform;

        // Layer Mask �� ���
        playerLayer = LayerMask.NameToLayer("PLAYER");
        obstacleLayer = LayerMask.NameToLayer("OBSTACLE");
        layerMask = 1 << playerLayer | 1 << obstacleLayer;
    }

    // �־��� ������ ���� ���� ���� ���� ��ǥ���� ����ϴ� �Լ�
    public Vector3 CirclePoint(float angle)
    {
        // ���� ��ǥ�� �������� �����ϱ� ���� �� ĳ������ Y�� ȸ������ ����
        // Mathf.Deg2Rad == 2pi / 360
        // Mathf.Rad2Deg == 180 / pi
        angle += transform.eulerAngles.y;
        return new Vector3(Mathf.Sin(angle * Mathf.Deg2Rad), 0, Mathf.Cos(angle * Mathf.Deg2Rad));
    }

    public bool isTraceLayer()
    {
        bool isTrace = false;

        // ���� �ݰ� ���� �ȿ��� Player Character ����
        Collider[] colliders = Physics.OverlapSphere(enemyTransform.position, viewRange, 1 << playerLayer);

        //�迭�� ������ 1�� ��, PlayerCharacter�� ���� �ȿ� �ִٰ� �Ǵ�
        if(colliders.Length == 1)
        {
            // Enemy Charactor�� Player Character�� ���� ���͸� ����ȭ�Ͽ� ���
            Vector3 direction = (playerTransform.position - enemyTransform.position).normalized;

            // Enemy Charactor�� �þ߰��� Player Character�� �����ִ��� �Ǵ�
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

        // Enemy Character�� Player Character ������ ���� ���͸� ����ȭ�Ͽ� ���
        Vector3 direction = (playerTransform.position - enemyTransform.position).normalized;

        // Raycast�� �߻��Ͽ� ��ֹ��� �ִ��� �Ǵ�
        if(Physics.Raycast(enemyTransform.position, direction, out rayHit, viewRange, layerMask))
        {
            isView = rayHit.collider.CompareTag("PLAYER");
        }

        return isView;
    }
}
