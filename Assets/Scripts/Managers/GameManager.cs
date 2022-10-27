using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;

public class GameManager : Singleton<GameManager> 
{
    public class SaveDatas
    {
        
    }

    
    [SerializeField]
    [Tooltip("���� ���� ��ȭ(���)")]
    private BigInteger moneyUnit;

    public BigInteger MoneyUnit
    {
        get { return moneyUnit; }
        set 
        {
            moneyUnit = value;
            if (BattleUIManager.Instance.contentsPanelObjs[(int)Contents.ColleagueContents].activeSelf)
            {
                ColleagueSystemManager.Instance.TextColorChange();
            }
        }
    }

    
    [SerializeField]
    [Tooltip("���� ���� ��ȭ(����)")]
    private int gemUnit;

    public int GemUnit
    {
        get { return gemUnit; }
        set 
        {
            gemUnit = value;
            if (BattleUIManager.Instance.contentsPanelObjs[(int)Contents.ColleagueContents].activeSelf)
            {
                ColleagueSystemManager.Instance.TextColorChange();
            }
        }
    }

    [Tooltip("�÷��̾� �⺻ �ɷ�ġ ������")]
    public int[] statsLevel;
    

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            MoneyUnit += 10;
            GemUnit += 10;
        }
    }
}
