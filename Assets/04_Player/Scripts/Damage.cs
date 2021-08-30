using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Damage : MonoBehaviour
{
    private const string bulletTag = "BULLET";
    private const string enemyTag = "ENEMY";
    private float inializeHp = 100.0f;
    public float currentHp;

    // 델리게이트 및 이벤트 선언
    public delegate void PlayerDieHander();
    public static event PlayerDieHander OnPlayerDie;

    // BloodScreen Texture를 저장하기 위한 변수
    public Image bloodScreen;

    // HP bar Image를 저장하기 위한 변수
    public Image hpBar;

    // HP bar의 처음 색상
    private readonly Color initColor = new Vector4(0, 1.0f, 0.0f, 1.0f);
    private Color currentColor;

    // Start is called before the first frame update
    private void Start()
    {
        currentHp = inializeHp;

        // HP bar의 초기 색상 설정
        hpBar.color = initColor;
        currentColor = initColor;
    }

    private void OnTriggerEnter(Collider collider)
    {
       // 충돌한 Collider의 Tag가 IsTrigger 옵션이 체크됐을 때 발생
       if(collider.tag == bulletTag)
        {
            Destroy(collider.gameObject);

            // 혈흔 효과를 표현할 코루틴 함수 호출
            StartCoroutine(ShowBloodScreen());

            currentHp -= 5.0f;
            // Debug.Log("Player HP : " + currentHp.ToString());

            // HP bar의 색상 및 크기 변경 함수 호출
            DisplayHpBar();

            if(currentHp <= 0.0f)
            {
                PlayerDie();
            }
        }
    }

    private IEnumerator ShowBloodScreen()
    {
        // BloodScreen Texture의 Alpha 값을 불규칙하게 변경
        bloodScreen.color = new Color(1, 0, 0, Random.Range(0.2f, 0.3f));
        yield return new WaitForSeconds(0.1f);

        // BloodScreen Texture의 색상을 모두 0으로 변경
        bloodScreen.color = Color.clear;
    }

    // Player의 사망 처리 루틴
    private void PlayerDie()
    {
        OnPlayerDie();
        //  Debug.Log("Player Die!");
        // "ENEMY" Tag로 지정된 모든 적 캐릭터를 추출하여 배열에 저장
        // GameObject[] enemies = GameObject.FindGameObjectsWithTag(enemyTag);

        // 배열의 처음부터 순회하면서 적 캐릭터의 OnPlayDie Function 호출
        //for(int index = 0; index < enemies.Length; index++)
        //{
        //    enemies[index].SendMessage("OnPlayerDie", SendMessageOptions.DontRequireReceiver);
        //}
    }

    private void DisplayHpBar()
    {
        // HP가 50%일 때까지는 녹색에서 노란색으로 변경
        if(currentHp / inializeHp > 0.5f)
        {
            currentColor.r = (1 - (currentHp / inializeHp)) * 2.0f;
        }
        // HP가 0%일 때까지는 노란색에서 빨간색으로 변경
        else
        {
            currentColor.g = (currentHp / inializeHp) * 2.0f;  
        }

        // HpBar의 색상 변경
        hpBar.color = currentColor;
        // HpBar의 크기 변경
        hpBar.fillAmount = (currentHp / inializeHp);
    }
}
