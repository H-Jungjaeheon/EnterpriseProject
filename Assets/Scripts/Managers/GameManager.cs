using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public class SaveDatas
    {
        public int hpLevel;
        public int damageLevel;
    }


    private int currentProficiency;

    public int CurrentProficiency
    {
        get { return currentProficiency; }
        set
        {
            currentProficiency = value;
            if (BattleUIManager.Instance.contentsPanelObjs[(int)Contents.ProficiencyContents].activeSelf)
            {
                ProficiencySystemManager.Instance.TextReSettings();
            }
        }
    }

    [SerializeField]
    [Tooltip("현재 보유 재화(골드)")]
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
            if (BattleUIManager.Instance.contentsPanelObjs[(int)Contents.ProficiencyContents].activeSelf)
            {
                ProficiencySystemManager.Instance.TextReSettings();
            }
        }
    }

    
    [SerializeField]
    [Tooltip("현재 보유 재화(보석)")]
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

    [Tooltip("플레이어 기본 능력치 레벨들")]
    public int[] statsLevel;
    

    // Start is called before the first frame update
    void Start()
    {
        MoneyUnit += 100000000000;
        GemUnit += 1000;
        CurrentProficiency += 10;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            MoneyUnit += 100000000000;
            GemUnit += 1000;
            CurrentProficiency += 10;
        }
    }
}
