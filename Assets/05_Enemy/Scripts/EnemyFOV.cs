using UnityEngine;

public class EnemyFOV : MonoBehaviour
{
    // �� ĳ������ ���� ���� �Ÿ��� ����
    public float viewRange = 15.0f;
    [Range(0, 360)]

    // �� ĳ������ �þ߰�
    public float viewAngle = 120.0f;

    // Start is called before the first frame update
    private void Start()
    {
        
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
}
