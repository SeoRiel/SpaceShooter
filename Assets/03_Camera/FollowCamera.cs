using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    public Transform target;                // ���� ���
    public float moveDamping = 15.0f;       // �̵� �ӵ� ���
    public float rotateDamping = 10.0f;     // ȸ�� �ӵ� ���
    public float distance = 5.0f;           // ���� ������ �Ÿ�
    public float height = 4.0f;             // ���� ������ ����
    public float targetOffset = 2.0f;       // ���� ��ǥ�� ������

    private Transform camTransform;         // CameraRig�� Transform Component

    [Header("Wall Obstacle Setting")]       
    public float heightAboveWall = 7.0f;    // ī�޶� �ö� ����
    public float colliderRadius = 1.8f;     // �浹ü�� ������
    public float overDamping = 5.0f;        // �̵� �ӵ� ���
    public float originHeight;              // ���� ���̸� ������ ����

    [Header("Etc Obstacle Setting")]
    public float heightAboveObstacle = 12.0f;   // ī�޶� �ö� ����
    public float castOffset = 1.0f;             // ���ΰ��� ������ ���� ĳ��Ʈ�� ���� ������

    // Start is called before the first frame update
    private void Start()
    {
        // CameraRig�� transform Component ����
        camTransform = GetComponent<Transform>();

        // ������ ī�޶� ���� ����
        originHeight = height;
    }

    private void Update()
    {
        // ��ü ������ �浹ü�� �浹 ���� �˻�
        if(Physics.CheckSphere(transform.position, colliderRadius))
        {
            // ���� �Լ��� �̿��Ͽ� ī�޶��� ���̸� �ε巴�� ����
            height = Mathf.Lerp(height, heightAboveWall, Time.deltaTime * overDamping);
        }
        else
        {
            // ���� �Լ��� �̿��Ͽ� ī�޶��� ���̸� �ε巴�� ����
            height = Mathf.Lerp(height, originHeight, Time.deltaTime * overDamping);
        }

        // ���ΰ��� ��ֹ��� ���������� �Ǵ��� RayCast�� ������ ����
        Vector3 castTarget = target.position + (target.up * castOffset);

        // castTarget ��ǥ�� ���� ���� ���
        Vector3 castDirection = (castTarget - transform.position).normalized;

        // �浹 ������ ��ȯ ���� ����
        RaycastHit rayHit;

        if(Physics.Raycast(camTransform.position, castDirection, out rayHit, Mathf.Infinity))
        {
            if(!rayHit.collider.CompareTag("PLAYER"))
            {
                height = Mathf.Lerp(height, heightAboveObstacle, Time.deltaTime * overDamping);
            }
            else
            {
                height = Mathf.Lerp(height, originHeight, Time.deltaTime * overDamping);
            }
        }

    }

    private void LateUpdate()
    {
        // ī�޶��� ���̿� �Ÿ��� ���
        var cameraPosition = target.position - (target.forward * distance) + (target.up * height);

        // �̵��� ���� �ӵ� ����� ����
        camTransform.position = Vector3.Slerp(transform.position, cameraPosition, Time.deltaTime * moveDamping);

        // ȸ���� ���� �ӵ� ����� ����
        camTransform.rotation = Quaternion.Slerp(transform.rotation, target.rotation, Time.deltaTime * rotateDamping);

        // ī�޶� ���� ������� Z���� ȸ����Ŵ
        camTransform.LookAt(target.position + (target.up * targetOffset));
    }

    // ������ ��ǥ�� �ð������� ǥ��
    private void OnDrawGizmos()
    {
        // Gizmo�� ���� ����
        Gizmos.color = Color.green;

        // ���� �� �þ߸� ���� ��ġ�� ǥ��
        Gizmos.DrawWireSphere(target.position + (target.up * targetOffset), 0.1f);

        // ���� ī�޶�� ���� ���� ���� ���� ǥ��
        Gizmos.DrawLine(target.position + (target.up * targetOffset), transform.position);

        // ī�޶��� �浹ü�� ǥ���ϱ� ���� ��ü ǥ��
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, colliderRadius);

        // Player Character�� ��ֹ��� ���������� �Ǵ��� ���̸� ǥ��
        Gizmos.color = Color.red;
        Gizmos.DrawLine(target.position + (target.up * castOffset), transform.position);
    }
}