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
    [Header("콘텐츠 창 관련 변수들")]
    [SerializeField]
    [Tooltip("현재 보여지는 콘텐츠 창 오브젝트")]
    private GameObject nowContentsPanelObj;

    [SerializeField]
    [Tooltip("콘텐츠 창 오브젝트 모음")]
    private GameObject[] contentsPanelObjs;

    [Header("재화 테스트")]
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

    void CalculationOfGoods(int[] aCalculatedValues, int[] aPriceToAdd) //뺄 때에는 가장 높은 단위 비교
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
