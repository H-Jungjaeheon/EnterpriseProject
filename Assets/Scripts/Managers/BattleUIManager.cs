using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Contents
{
    StatUpgradeContents,
    TestContents,
    ContentsLength
}

public class BattleUIManager : MonoBehaviour
{
    [Header("������ â ���� ������")]
    [SerializeField]
    [Tooltip("���� �������� ������ â ������Ʈ")]
    private GameObject nowContentsPanelObj;

    [SerializeField]
    [Tooltip("������ â ������Ʈ ����")]
    private GameObject[] contentsPanelObjs;

    [Header("��ȭ �׽�Ʈ")]
    [SerializeField]
    private int[] moneyUnit;

    int index;

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

    void CalculationOfGoods(int[] aCalculatedValues, int[] aPriceToAdd) //�� ������ ���� ���� ���� ��
    {
        for (int nowUnitOfGoodsIndex = 0; nowUnitOfGoodsIndex < moneyUnit.Length; nowUnitOfGoodsIndex++)
        {
            aCalculatedValues[nowUnitOfGoodsIndex] += aPriceToAdd[nowUnitOfGoodsIndex];
            if (aCalculatedValues[nowUnitOfGoodsIndex] > 0)
            {
                index = nowUnitOfGoodsIndex;
            }
        }

        for (int i = 0; i <= index; i++)
        {
            if (moneyUnit[i] >= 1000)
            {
                moneyUnit[i] -= 1000;
                moneyUnit[i + 1] += 1;
            }
            if (moneyUnit[i] < 0)
            {
                if (index > i)
                {
                    moneyUnit[i + 1] -= 1;
                    moneyUnit[i] += 1000;
                }
            }
        }
    }
}
