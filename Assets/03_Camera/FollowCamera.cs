using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    public Transform target;            // ���� ���
    public float moveDamping = 15.0f;   // �̵� �ӵ� ���
    public float rotateDamping = 10.0f; // ȸ�� �ӵ� ���
    public float distance = 5.0f;       // ���� ������ �Ÿ�
    public float height = 4.0f;         // ���� ������ ����
    public float targetOffset = 2.0f;   // ���� ��ǥ�� ������

    private Transform camTransform;        // CameraRig�� Transform Component

    // Start is called before the first frame update
    private void Start()
    {
        // CameraRig�� transform Component ����
        camTransform = GetComponent<Transform>();
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
        Gizmos.color = Color.green;

        // ���� �� �þ߸� ���� ��ġ�� ǥ��
        Gizmos.DrawWireSphere(target.position + (target.up * targetOffset), 0.1f);

        // ���� ī�޶�� ���� ���� ���� ���� ǥ��
        Gizmos.DrawLine(target.position + (target.up * targetOffset), transform.position);
    }
}