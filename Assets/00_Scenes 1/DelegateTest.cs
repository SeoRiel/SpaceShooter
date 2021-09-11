using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DelegateTest : MonoBehaviour
{
    // 델리게이트에 연결할 함수의 원형 정의
    delegate void CallNumberDelegate(int number);

    // 델리게이트 변수 선언
    CallNumberDelegate callNumber;

    // Start is called before the first frame update
    private void Start()
    {
        // callNumber Delegate Parameter에 OnPlusNumber Function 연결
        callNumber = OnePlusNumber;
        // 함수 호출
        callNumber(5);

        // callNumber Delegate Parameter에 PowerNumber Function 연결
        callNumber = PowerNumber;
        callNumber(5);
    }

    private void OnePlusNumber(int number)
    {
        int result = number + 1;
        Debug.Log("One Plus = " + result);
    }

    private void PowerNumber(int number)
    {
        int result = number * number;
        Debug.Log("Power = " + result);
    }
}
