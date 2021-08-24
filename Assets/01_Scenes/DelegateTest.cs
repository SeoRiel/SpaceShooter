using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DelegateTest : MonoBehaviour
{
    // ��������Ʈ�� ������ �Լ��� ���� ����
    delegate void CallNumberDelegate(int number);

    // ��������Ʈ ���� ����
    CallNumberDelegate callNumber;

    // Start is called before the first frame update
    private void Start()
    {
        // callNumber Delegate Parameter�� OnPlusNumber Function ����
        callNumber = OnePlusNumber;
        // �Լ� ȣ��
        callNumber(5);

        // callNumber Delegate Parameter�� PowerNumber Function ����
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
