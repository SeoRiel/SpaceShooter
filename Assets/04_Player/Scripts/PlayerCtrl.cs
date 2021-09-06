using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 클래스는 System.Serializable 이라는 어트리뷰트(Attribute)를 명시해야 인스펙터 뷰에 노출됨
[System.Serializable]
public class PlayerAnimation
{
    public AnimationClip idle;
    public AnimationClip runForward;
    public AnimationClip runBack;
    public AnimationClip runLeft;
    public AnimationClip runRight;
}

public class PlayerCtrl : MonoBehaviour
{
    private float vertical = 0.0f;
    private float horizontal = 0.0f;
    private float rotate = 0.0f;

    private Transform playerTransform;                   // 접근해야 하는 컴포넌트는 반드시 변수에 할당한 후 사용
    
    public float moveSpeed = 10.0f;                      // 이동 속도 변수(public으로 선언되어 Inspector에 노출됨)
    public float rotateSpeed = 80.0f;                    // 회전 속도 변수

    public PlayerAnimation playerAnimation;              // 인스펙터 뷰에 표시할 애니메이션 클래스 변수 
    
    [HideInInspector]
    public Animation animation;                          // Animation Component를 저장하기 위한 변수

    private void Start()                                 // Start is called before the first frame update
    {
        playerTransform = GetComponent<Transform>();     // 스크립트가 실행된 후,
                                                         // 처음 수행되는 Start 함수에서 Transform 컴포넌트 할당

        animation = GetComponent<Animation>();           // Animation Component를 변수에 할당
        animation.clip = playerAnimation.idle;           // Animation Component의 애니메이션 클립을 저장하고 실행
        animation.Play();

        moveSpeed = GameManager.instance.gameData.speed; // 불러온 데이터 값을 moveSpeed에 적용
    }

    private void Update()                                // Update is called once per frame
    {
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");
        rotate = Input.GetAxis("Mouse X");

        // Debug.Log("horizontal = " + horizontal.ToString());
        // Debug.Log("Vertical = " + vertical.ToString());

        // 전후좌우 이동 방향 벡터 계산
        Vector3 moveDirection = (Vector3.forward * vertical) + (Vector3.right * horizontal);

        // Translate(이동 방향 * 속도 * 변위값 * Time.deltaTime, 기준 좌표)
        playerTransform.Translate(moveDirection.normalized * moveSpeed * Time.deltaTime, Space.Self);

        // Vector3.up 축을 기준으로 rotateSpeed만큼의 속도로 회전
        playerTransform.Rotate(Vector3.up, rotateSpeed * Time.deltaTime * rotate);

        // 키보드 입력값을 기준으로 동작할 애니메이션 수행
        if(vertical >= 0.1f)
        {
            // 전방 이동 애니메이션
            animation.CrossFade(playerAnimation.runForward.name, 0.3f);
        }
        else if(vertical <= -0.1f)
        {
            // 후진 이동 애니메이션
            animation.CrossFade(playerAnimation.runBack.name, 0.3f);
        }
        else if(horizontal >= 0.1f)
        {
            // 오른쪽 이동 애니메이션
            animation.CrossFade(playerAnimation.runRight.name, 0.3f);
        }
        else if(horizontal <= -0.1f)
        {
            // 왼쪽 이동 애니메이션
            animation.CrossFade(playerAnimation.runLeft.name, 0.3f);
        }
    }
}
