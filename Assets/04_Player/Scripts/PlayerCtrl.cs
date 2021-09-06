using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Ŭ������ System.Serializable �̶�� ��Ʈ����Ʈ(Attribute)�� ����ؾ� �ν����� �信 �����
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

    private Transform playerTransform;                   // �����ؾ� �ϴ� ������Ʈ�� �ݵ�� ������ �Ҵ��� �� ���
    
    public float moveSpeed = 10.0f;                      // �̵� �ӵ� ����(public���� ����Ǿ� Inspector�� �����)
    public float rotateSpeed = 80.0f;                    // ȸ�� �ӵ� ����

    public PlayerAnimation playerAnimation;              // �ν����� �信 ǥ���� �ִϸ��̼� Ŭ���� ���� 
    
    [HideInInspector]
    public Animation animation;                          // Animation Component�� �����ϱ� ���� ����

    private void Start()                                 // Start is called before the first frame update
    {
        playerTransform = GetComponent<Transform>();     // ��ũ��Ʈ�� ����� ��,
                                                         // ó�� ����Ǵ� Start �Լ����� Transform ������Ʈ �Ҵ�

        animation = GetComponent<Animation>();           // Animation Component�� ������ �Ҵ�
        animation.clip = playerAnimation.idle;           // Animation Component�� �ִϸ��̼� Ŭ���� �����ϰ� ����
        animation.Play();

        moveSpeed = GameManager.instance.gameData.speed; // �ҷ��� ������ ���� moveSpeed�� ����
    }

    private void Update()                                // Update is called once per frame
    {
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");
        rotate = Input.GetAxis("Mouse X");

        // Debug.Log("horizontal = " + horizontal.ToString());
        // Debug.Log("Vertical = " + vertical.ToString());

        // �����¿� �̵� ���� ���� ���
        Vector3 moveDirection = (Vector3.forward * vertical) + (Vector3.right * horizontal);

        // Translate(�̵� ���� * �ӵ� * ������ * Time.deltaTime, ���� ��ǥ)
        playerTransform.Translate(moveDirection.normalized * moveSpeed * Time.deltaTime, Space.Self);

        // Vector3.up ���� �������� rotateSpeed��ŭ�� �ӵ��� ȸ��
        playerTransform.Rotate(Vector3.up, rotateSpeed * Time.deltaTime * rotate);

        // Ű���� �Է°��� �������� ������ �ִϸ��̼� ����
        if(vertical >= 0.1f)
        {
            // ���� �̵� �ִϸ��̼�
            animation.CrossFade(playerAnimation.runForward.name, 0.3f);
        }
        else if(vertical <= -0.1f)
        {
            // ���� �̵� �ִϸ��̼�
            animation.CrossFade(playerAnimation.runBack.name, 0.3f);
        }
        else if(horizontal >= 0.1f)
        {
            // ������ �̵� �ִϸ��̼�
            animation.CrossFade(playerAnimation.runRight.name, 0.3f);
        }
        else if(horizontal <= -0.1f)
        {
            // ���� �̵� �ִϸ��̼�
            animation.CrossFade(playerAnimation.runLeft.name, 0.3f);
        }
    }
}
