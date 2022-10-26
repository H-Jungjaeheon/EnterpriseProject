using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using UnityEngine.UI;

public enum ColleagueKind
{
    FirstColleague,
    SecondColleague,
    ColleagueCount
}

public class ColleagueSystemManager : MonoBehaviour
{
    [SerializeField]
    [Tooltip("�� ���� ��� ���� �� �ʿ��� ����")]
    private int[] gemRequiredForColleaguenlock = new int[(int)ColleagueKind.ColleagueCount];

    private bool[] colleagueUnlocking = new bool[(int)ColleagueKind.ColleagueCount];

    private BigInteger[] moneyRequiredForUpgrade = new BigInteger[(int)ColleagueKind.ColleagueCount];

    private int[] colleagueLevel = new int[(int)ColleagueKind.ColleagueCount];

    [SerializeField]
    [Tooltip("�� ���� ���� ǥ�� �ؽ�Ʈ")]
    private Text[] priceIndicationText = new Text[(int)ColleagueKind.ColleagueCount];

    Color redTextColor = new Color(1, 0, 0);

    Color greenTextColor = new Color(0, 1, 0.03f);

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnEnable()
    {
        for(int nowIndex = 0; nowIndex <= (int)ColleagueKind.ColleagueCount; nowIndex++)
        {
            //���� �ε����� ������� �Ǻ� �� �ؽ�Ʈ �� ����
            priceIndicationText[nowIndex].color = redTextColor;
        }
    }

    public void ColleagueUpgradeOrUnlock(int nowColleagueIndex)
    {
        if (colleagueUnlocking[nowColleagueIndex] == false && GameManager.Instance.GemUnit >= gemRequiredForColleaguenlock[nowColleagueIndex]) //�ش� �ε��� ���� �� ��������� ���
        {
            GameManager.Instance.GemUnit -= gemRequiredForColleaguenlock[nowColleagueIndex]; //�ش� �ε��� ���� ��� ���� ��� ����

            colleagueUnlocking[nowColleagueIndex] = true; //�ش� �ε��� ���� ��� ����
        }
        else if (colleagueUnlocking[nowColleagueIndex] && GameManager.Instance.MoneyUnit >= moneyRequiredForUpgrade[nowColleagueIndex]) //�ش� �ε��� ���� ��������� ���
        {
            GameManager.Instance.MoneyUnit -= moneyRequiredForUpgrade[nowColleagueIndex]; //�ش� �ε��� ���� ���׷��̵� ��� ����

            colleagueLevel[nowColleagueIndex]++; //�ش� �ε��� ���� ���� ����

            moneyRequiredForUpgrade[nowColleagueIndex] += moneyRequiredForUpgrade[nowColleagueIndex] / 2; //�ش� �ε��� ���� ���׷��̵� ��� ����
        }
        else
        {
            return;
        }

        priceIndicationText[nowColleagueIndex].text = $"���׷��̵�\n{moneyRequiredForUpgrade[nowColleagueIndex]} ���"; //�ش� �ε��� ���� ��ư �ؽ�Ʈ ����
    }
}
