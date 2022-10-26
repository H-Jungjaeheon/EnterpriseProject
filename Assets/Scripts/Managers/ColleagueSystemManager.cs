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
    [Tooltip("각 동료 잠금 해제 시 필요한 보석")]
    private int[] gemRequiredForColleaguenlock = new int[(int)ColleagueKind.ColleagueCount];

    private bool[] colleagueUnlocking = new bool[(int)ColleagueKind.ColleagueCount];

    private BigInteger[] moneyRequiredForUpgrade = new BigInteger[(int)ColleagueKind.ColleagueCount];

    private int[] colleagueLevel = new int[(int)ColleagueKind.ColleagueCount];

    [SerializeField]
    [Tooltip("각 동료 가격 표시 텍스트")]
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
            //현재 인덱스의 잠금해제 판별 후 텍스트 색 변경
            priceIndicationText[nowIndex].color = redTextColor;
        }
    }

    public void ColleagueUpgradeOrUnlock(int nowColleagueIndex)
    {
        if (colleagueUnlocking[nowColleagueIndex] == false && GameManager.Instance.GemUnit >= gemRequiredForColleaguenlock[nowColleagueIndex]) //해당 인덱스 동료 비 잠금해제의 경우
        {
            GameManager.Instance.GemUnit -= gemRequiredForColleaguenlock[nowColleagueIndex]; //해당 인덱스 동료 잠금 해제 비용 차감

            colleagueUnlocking[nowColleagueIndex] = true; //해당 인덱스 동료 잠금 해제
        }
        else if (colleagueUnlocking[nowColleagueIndex] && GameManager.Instance.MoneyUnit >= moneyRequiredForUpgrade[nowColleagueIndex]) //해당 인덱스 동료 잠금해제의 경우
        {
            GameManager.Instance.MoneyUnit -= moneyRequiredForUpgrade[nowColleagueIndex]; //해당 인덱스 동료 업그레이드 비용 차감

            colleagueLevel[nowColleagueIndex]++; //해당 인덱스 동료 레벨 증가

            moneyRequiredForUpgrade[nowColleagueIndex] += moneyRequiredForUpgrade[nowColleagueIndex] / 2; //해당 인덱스 동료 업그레이드 비용 증가
        }
        else
        {
            return;
        }

        priceIndicationText[nowColleagueIndex].text = $"업그레이드\n{moneyRequiredForUpgrade[nowColleagueIndex]} 골드"; //해당 인덱스 동료 버튼 텍스트 수정
    }
}
