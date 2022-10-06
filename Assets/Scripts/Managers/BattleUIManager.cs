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

public class BattleUIManager : Singleton<BattleUIManager>
{
    [Header("������ â ���� ������")]
    [SerializeField]
    [Tooltip("���� �������� ������ â ������Ʈ")]
    private GameObject nowContentsPanelObj;

    [SerializeField]
    [Tooltip("������ â ������Ʈ ����")]
    private GameObject[] contentsPanelObjs;
    

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
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

    public void CalculationOfGoods(int[] aCalculatedValues, int[] aPriceToAdd, Text commodityConversionText) //�� ������ ���� ���� ���� ��
    {
        int maxUnitIndex = 0;
        for (int nowUnitOfGoodsIndex = 0; nowUnitOfGoodsIndex < aCalculatedValues.Length; nowUnitOfGoodsIndex++)
        {
            aCalculatedValues[nowUnitOfGoodsIndex] += aPriceToAdd[nowUnitOfGoodsIndex];

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
