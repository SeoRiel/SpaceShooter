using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShakeCamera : MonoBehaviour
{
    public Transform shakeCamera;       // 셰이크 효과를 줄 카메라의 Transform을 저장할 변수
    public bool shakeRotate = false;    // 회전시킬 것인지를 판단할 변수
    private Vector3 originPosition;     // 초기 좌표와 회전값을 저장할 변수
    private Quaternion originRotation;

    // Start is called before the first frame update
    private void Start()
    {
        // 카메라의 초깃값을 저장
        originPosition = shakeCamera.localPosition;
        originRotation = shakeCamera.localRotation;
    }

    public IEnumerator CameraShake(float duration = 0.05f, float magnitudePosition = 0.03f, float magnitudeRotation = 0.1f)
    {
        // 시간을 누적할 변수
        float passTime = 0.0f;

        // 진동 시간동안 루프 순회
        while(passTime < duration)
        {
            // 불규칙한 위치 산출
            Vector3 shakePosition = Random.insideUnitSphere;
            // 카메라의 위치를 변경
            shakeCamera.localPosition = shakePosition * magnitudePosition;

            if(shakeRotate)
            {
                // 불규칙한 회전값을 펄린 노이즈 함수를 이용해 추출
                Vector3 shakeRotate = new Vector3(0, 0, Mathf.PerlinNoise(Time.deltaTime * magnitudeRotation, 0.0f));

                // 카메라의 회전값을 변경
                shakeCamera.localRotation = Quaternion.Euler(shakeRotate);
            }
            // 진동 시간 누적
            passTime += Time.deltaTime;

            yield return null;
        }

        // 진동이 끝난 후, 카메라의 초기값으로 설정
        shakeCamera.localPosition = originPosition;
        shakeCamera.localRotation = originRotation;
    }
}