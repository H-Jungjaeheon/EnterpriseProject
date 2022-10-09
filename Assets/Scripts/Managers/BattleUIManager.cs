using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum Contents
{
    StatUpgradeContents,
    TestContents,
    ContentsLength
}

public enum UpgradeableBasicStats
{
    Damage,
    MaxHp
}

public class BattleUIManager : Singleton<BattleUIManager>
{
    [Header("콘텐츠 창 관련 변수들")]
    [SerializeField]
    [Tooltip("현재 보여지는 콘텐츠 창 오브젝트")]
    private GameObject nowContentsPanelObj;

    [SerializeField]
    [Tooltip("콘텐츠 창 오브젝트 모음")]
    private GameObject[] contentsPanelObjs;

    #region 스탯 업그레이드창 텍스트 모음
    [SerializeField]
    [Tooltip("스탯 레벨 표기 텍스트")]
    private Text[] basicStatLevelText;
    [SerializeField]
    [Tooltip("현재 스탯 수치 표기 텍스트")]
    private Text[] basicStatFigureText;
    [SerializeField]
    [Tooltip("스탯 업그레이드 비용 표기 텍스트")]
    private Text[] goodsTextRequiredForUpgrade;
    #endregion

    [SerializeField]
    private int[] damageGoodsRequiredForUpgrade = new int[25];

    [SerializeField]
    private GameObject Player;

    // Start is called before the first frame update
    void Start()
    {
        basicStatLevelText[(int)UpgradeableBasicStats.Damage].text = $"Lv {GameManager.Instance.statsLevel[(int)UpgradeableBasicStats.Damage]}";
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void BasicStatUpgrade(int statsToUpgradeCurrently)
    {
        int unitMaximumArrayIndex = 24;
        var gmInstance = GameManager.Instance;
        int largestIndex = 0;
        int[] nextGoodsRequiredForUpgrade = new int[25];
        int[] statsForUpgrade = new int[25];
        var playerComponent = Player.GetComponent<Player>();

        for (int nowIndex = 0; nowIndex < gmInstance.MoneyUnit.Length; nowIndex++)
        {
            if (gmInstance.MoneyUnit[nowIndex] > 0)
            {
                largestIndex = nowIndex;
            }
        }


        for (int nowIndex = unitMaximumArrayIndex; nowIndex >= 0; nowIndex--)
        {
            if (nowIndex > largestIndex && gmInstance.MoneyUnit[nowIndex] > damageGoodsRequiredForUpgrade[nowIndex])
            {
                break;
            }
            else if(nowIndex <= largestIndex)
            {
                if (gmInstance.MoneyUnit[nowIndex] < damageGoodsRequiredForUpgrade[nowIndex])
                {
                    return;
                }
                else if (gmInstance.MoneyUnit[nowIndex] > damageGoodsRequiredForUpgrade[nowIndex])
                {
                    break;
                }
                else if (nowIndex == 0 && gmInstance.MoneyUnit[nowIndex] == damageGoodsRequiredForUpgrade[nowIndex])
                {
                    break;
                }
            }
        }

        CalculationOfGoods(gmInstance.MoneyUnit, damageGoodsRequiredForUpgrade, basicStatFigureText[statsToUpgradeCurrently], false); //재화 삭감
        gmInstance.statsLevel[statsToUpgradeCurrently]++; //레벨 증가
        basicStatLevelText[statsToUpgradeCurrently].text = $"Lv {gmInstance.statsLevel[statsToUpgradeCurrently]}"; //레벨 텍스트 수정

        for (int nowIndex = unitMaximumArrayIndex; nowIndex >= 0; nowIndex--) //비용 수정(연산)
        {
            if (damageGoodsRequiredForUpgrade[nowIndex] > 0)
            {
                nextGoodsRequiredForUpgrade[nowIndex]++;
                break;
            }
        }

        CalculationOfGoods(damageGoodsRequiredForUpgrade, nextGoodsRequiredForUpgrade, goodsTextRequiredForUpgrade[statsToUpgradeCurrently], true); //업그레이드 비용 수정
        goodsTextRequiredForUpgrade[statsToUpgradeCurrently].text = $"강화\n{goodsTextRequiredForUpgrade[statsToUpgradeCurrently].text}원";

        for (int nowIndex = unitMaximumArrayIndex; nowIndex >= 0; nowIndex--) //비용 수정(연산)
        {
            if (playerComponent.AttackPower[nowIndex] > 0)
            {
                statsForUpgrade[nowIndex]++;
                break;
            }
        }

        CalculationOfGoods(playerComponent.AttackPower, statsForUpgrade, basicStatFigureText[statsToUpgradeCurrently], true);
    }

    public void AnotherContentsPopUp(GameObject PopUpObj)
    {
        //팝업 다트윈 애니메이션 넣기
        GameObject lastViewContents = PopUpObj;
        if (PopUpObj.activeSelf)
        {
            nowContentsPanelObj = contentsPanelObjs[(int)Contents.StatUpgradeContents];
        }
        else
        {
            nowContentsPanelObj = PopUpObj;
        }
        lastViewContents.SetActive(false);
        nowContentsPanelObj.SetActive(true);
    }

    public void AnotherContentsChangeScrollView(GameObject PopUpObj)
    {
        GameObject lastViewContents = PopUpObj;
        if (PopUpObj.activeSelf)
        {
            nowContentsPanelObj = contentsPanelObjs[(int)Contents.StatUpgradeContents];
        }
        else
        {
            nowContentsPanelObj = PopUpObj;
        }
        lastViewContents.SetActive(false);
        nowContentsPanelObj.SetActive(true);
    }

    public void CalculationOfGoods(int[] aCalculatedValues, int[] aPriceToAdd, Text commodityConversionText, bool isAddition) //뺄 때에는 가장 높은 단위 비교
    {
        int maxUnitIndex = 0;
        for (int nowUnitOfGoodsIndex = 0; nowUnitOfGoodsIndex < aCalculatedValues.Length; nowUnitOfGoodsIndex++)
        {
            if (isAddition)
            {
                aCalculatedValues[nowUnitOfGoodsIndex] += aPriceToAdd[nowUnitOfGoodsIndex];
            }
            else
            {
                aCalculatedValues[nowUnitOfGoodsIndex] -= aPriceToAdd[nowUnitOfGoodsIndex];
            }

            if (aCalculatedValues[nowUnitOfGoodsIndex] >= 1000)
            {
                aCalculatedValues[nowUnitOfGoodsIndex + 1] += aCalculatedValues[nowUnitOfGoodsIndex] / 1000;
                aCalculatedValues[nowUnitOfGoodsIndex] %= 1000;
            }
            else if(aCalculatedValues[nowUnitOfGoodsIndex] < 0)
            {
                aCalculatedValues[nowUnitOfGoodsIndex + 1] -= aCalculatedValues[nowUnitOfGoodsIndex] / 1000;
                aCalculatedValues[nowUnitOfGoodsIndex] %= 1000;

                if (aCalculatedValues[nowUnitOfGoodsIndex] < 0)
                {
                    aCalculatedValues[nowUnitOfGoodsIndex + 1]--;
                    aCalculatedValues[nowUnitOfGoodsIndex] = 1000 + aCalculatedValues[nowUnitOfGoodsIndex];
                }
            }

            if (aCalculatedValues[nowUnitOfGoodsIndex] > 0)
            {
                maxUnitIndex = nowUnitOfGoodsIndex;
            }
        }
        ConvertGoodsString(commodityConversionText, maxUnitIndex, aCalculatedValues);
    }

    private void ConvertGoodsString(Text commodityConversionText, int maxUnitIndex, int[] aCalculatedValues)
    {
        char aUnitOfGoods = '\0';
        if (maxUnitIndex != 0)
        {
            aUnitOfGoods = (char)(maxUnitIndex + 96);
        }

        if (maxUnitIndex != 0)
        {
            if (aCalculatedValues[maxUnitIndex] / 10 >= 10)
            {
                commodityConversionText.text = $"{aCalculatedValues[maxUnitIndex]}{aUnitOfGoods}";
            }
            else if (aCalculatedValues[maxUnitIndex] / 10 > 0)
            {
                commodityConversionText.text = $"{aCalculatedValues[maxUnitIndex]}.{0 + aCalculatedValues[maxUnitIndex - 1] / 100}{aUnitOfGoods}";
            }
            else if (aCalculatedValues[maxUnitIndex] / 10 <= 0)
            {
                int hundredUnits = aCalculatedValues[maxUnitIndex - 1] / 100;
                int tenUnits = (aCalculatedValues[maxUnitIndex - 1] % 100) / 10;
                commodityConversionText.text = $"{aCalculatedValues[maxUnitIndex]}.{hundredUnits}{tenUnits}{aUnitOfGoods}";
            }
        }
        else
        {
            commodityConversionText.text = $"{aCalculatedValues[maxUnitIndex]}{aUnitOfGoods}";
        }
    }
}
