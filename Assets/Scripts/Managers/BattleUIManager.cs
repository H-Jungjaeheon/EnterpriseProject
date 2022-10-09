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
    [Header("������ â ���� ������")]
    [SerializeField]
    [Tooltip("���� �������� ������ â ������Ʈ")]
    private GameObject nowContentsPanelObj;

    [SerializeField]
    [Tooltip("������ â ������Ʈ ����")]
    private GameObject[] contentsPanelObjs;

    #region ���� ���׷��̵�â �ؽ�Ʈ ����
    [SerializeField]
    [Tooltip("���� ���� ǥ�� �ؽ�Ʈ")]
    private Text[] basicStatLevelText;
    [SerializeField]
    [Tooltip("���� ���� ��ġ ǥ�� �ؽ�Ʈ")]
    private Text[] basicStatFigureText;
    [SerializeField]
    [Tooltip("���� ���׷��̵� ��� ǥ�� �ؽ�Ʈ")]
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

        CalculationOfGoods(gmInstance.MoneyUnit, damageGoodsRequiredForUpgrade, basicStatFigureText[statsToUpgradeCurrently], false); //��ȭ �谨
        gmInstance.statsLevel[statsToUpgradeCurrently]++; //���� ����
        basicStatLevelText[statsToUpgradeCurrently].text = $"Lv {gmInstance.statsLevel[statsToUpgradeCurrently]}"; //���� �ؽ�Ʈ ����

        for (int nowIndex = unitMaximumArrayIndex; nowIndex >= 0; nowIndex--) //��� ����(����)
        {
            if (damageGoodsRequiredForUpgrade[nowIndex] > 0)
            {
                nextGoodsRequiredForUpgrade[nowIndex]++;
                break;
            }
        }

        CalculationOfGoods(damageGoodsRequiredForUpgrade, nextGoodsRequiredForUpgrade, goodsTextRequiredForUpgrade[statsToUpgradeCurrently], true); //���׷��̵� ��� ����
        goodsTextRequiredForUpgrade[statsToUpgradeCurrently].text = $"��ȭ\n{goodsTextRequiredForUpgrade[statsToUpgradeCurrently].text}��";

        for (int nowIndex = unitMaximumArrayIndex; nowIndex >= 0; nowIndex--) //��� ����(����)
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
        //�˾� ��Ʈ�� �ִϸ��̼� �ֱ�
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

    public void CalculationOfGoods(int[] aCalculatedValues, int[] aPriceToAdd, Text commodityConversionText, bool isAddition) //�� ������ ���� ���� ���� ��
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
