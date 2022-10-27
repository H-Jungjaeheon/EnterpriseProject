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

public class ColleagueSystemManager : Singleton<ColleagueSystemManager>
{
    [SerializeField]
    [Tooltip("각 동료 잠금 해제 시 필요한 보석")]
    private int[] gemRequiredForColleaguenlock = new int[(int)ColleagueKind.ColleagueCount];

    private bool[] colleagueUnlocking = new bool[(int)ColleagueKind.ColleagueCount]; //동료 잠금 해제 판별

    private BigInteger[] moneyRequiredForUpgrade = new BigInteger[(int)ColleagueKind.ColleagueCount];

    private int[] colleagueLevel = new int[(int)ColleagueKind.ColleagueCount];

    [SerializeField]
    [Tooltip("잠금 해제 시 나타나는 장착 버튼 오브젝트")]
    private GameObject[] colleagueEquip;

    [SerializeField]
    [Tooltip("각 동료 가격 표시 텍스트")]
    private Text[] priceIndicationText = new Text[(int)ColleagueKind.ColleagueCount];

    [SerializeField]
    [Tooltip("동료 아이콘 모음")]
    private Sprite[] colleagueIcons;

    [SerializeField]
    [Tooltip("현재 장착중인 동료 아이콘")]
    private SpriteRenderer nowColleagueIcon;

    Color redTextColor = new Color(1, 0, 0);

    Color greenTextColor = new Color(0, 1, 0.03f);

    void Start()
    {
        StartSettings();
    }

    private void OnEnable()
    {
        if (gameObject.activeSelf)
        {
            TextColorChange();
        }
        EquipButtonSetActive();
    }

    private void StartSettings()
    {
        for (int nowIndex = 0; nowIndex < (int)ColleagueKind.ColleagueCount; nowIndex++)
        {
            moneyRequiredForUpgrade[nowIndex] = 10;
        }
    }

    public void EquipColleague(int nowEquipColleagueIndex)
    {
        if (nowColleagueIcon.sprite != colleagueIcons[nowEquipColleagueIndex])
        {
            print("동료 장착 실행");
            nowColleagueIcon.sprite = colleagueIcons[nowEquipColleagueIndex];
        }
    }

    public void ColleagueUpgradeOrUnlock(int nowColleagueIndex)
    {
        if (colleagueUnlocking[nowColleagueIndex] == false && GameManager.Instance.GemUnit >= gemRequiredForColleaguenlock[nowColleagueIndex]) //해당 인덱스 동료 비 잠금해제의 경우
        {
            GameManager.Instance.GemUnit -= gemRequiredForColleaguenlock[nowColleagueIndex]; //해당 인덱스 동료 잠금 해제 비용 차감

            colleagueUnlocking[nowColleagueIndex] = true; //해당 인덱스 동료 잠금 해제
                
            EquipButtonSetActive();
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

    public void TextColorChange()
    {
        for (int nowIndex = 0; nowIndex < (int)ColleagueKind.ColleagueCount; nowIndex++)
        {
            if (colleagueUnlocking[nowIndex]) //잠금 해제 완료 시
            {
                priceIndicationText[nowIndex].color = (moneyRequiredForUpgrade[nowIndex] <= GameManager.Instance.MoneyUnit) ? greenTextColor : redTextColor;
                priceIndicationText[nowIndex].text = $"업그레이드\n{moneyRequiredForUpgrade[nowIndex]} 골드"; //해당 인덱스 동료 버튼 텍스트 수정
            }
            else //잠금 해제 비 완료 시
            {
                priceIndicationText[nowIndex].color = (gemRequiredForColleaguenlock[nowIndex] <= GameManager.Instance.GemUnit) ? greenTextColor : redTextColor;
                priceIndicationText[nowIndex].text = $"구매\n{gemRequiredForColleaguenlock[nowIndex]} 젬"; //해당 인덱스 동료 버튼 텍스트 수정
            }
        }
    }

    private void EquipButtonSetActive()
    {
        for (int nowIndex = 0; nowIndex < (int)ColleagueKind.ColleagueCount; nowIndex++)
        {
            if (colleagueUnlocking[nowIndex] && colleagueEquip[nowIndex].activeSelf == false)
            {
                colleagueEquip[nowIndex].SetActive(true);
            }
        }
    }
}
