using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShakeCamera : MonoBehaviour
{
    public Transform shakeCamera;       // ����ũ ȿ���� �� ī�޶��� Transform�� ������ ����
    public bool shakeRotate = false;    // ȸ����ų �������� �Ǵ��� ����
    private Vector3 originPosition;     // �ʱ� ��ǥ�� ȸ������ ������ ����
    private Quaternion originRotation;

    // Start is called before the first frame update
    private void Start()
    {
        // ī�޶��� �ʱ갪�� ����
        originPosition = shakeCamera.localPosition;
        originRotation = shakeCamera.localRotation;
    }

    public IEnumerator CameraShake(float duration = 0.05f, float magnitudePosition = 0.03f, float magnitudeRotation = 0.1f)
    {
        // �ð��� ������ ����
        float passTime = 0.0f;

        // ���� �ð����� ���� ��ȸ
        while(passTime < duration)
        {
            // �ұ�Ģ�� ��ġ ����
            Vector3 shakePosition = Random.insideUnitSphere;
            // ī�޶��� ��ġ�� ����
            shakeCamera.localPosition = shakePosition * magnitudePosition;

            if(shakeRotate)
            {
                // �ұ�Ģ�� ȸ������ �޸� ������ �Լ��� �̿��� ����
                Vector3 shakeRotate = new Vector3(0, 0, Mathf.PerlinNoise(Time.deltaTime * magnitudeRotation, 0.0f));

                // ī�޶��� ȸ������ ����
                shakeCamera.localRotation = Quaternion.Euler(shakeRotate);
            }
            // ���� �ð� ����
            passTime += Time.deltaTime;

            yield return null;
        }

        // ������ ���� ��, ī�޶��� �ʱⰪ���� ����
        shakeCamera.localPosition = originPosition;
        shakeCamera.localRotation = originRotation;
    }
}