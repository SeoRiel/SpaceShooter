using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damage : MonoBehaviour
{
    private const string bulletTag = "BULLET";
    private const string enemyTag = "ENEMY";
    private float inializeHp = 100.0f;
    public float currentHp;

    // 델리게이트 및 이벤트 선언
    public delegate void PlayerDieHander();
    public static event PlayerDieHander OnPlayerDie;

    // Start is called before the first frame update
    private void Start()
    {
        currentHp = inializeHp;
    }

    private void OnTriggerEnter(Collider collider)
    {
       // 충돌한 Collider의 Tag가 IsTrigger 옵션이 체크됐을 때 발생
       if(collider.tag == bulletTag)
        {
            Destroy(collider.gameObject);

            currentHp -= 5.0f;
            Debug.Log("Player HP : " + currentHp.ToString());

            if(currentHp <= 0.0f)
            {
                PlayerDie();
            }
        }
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
}
