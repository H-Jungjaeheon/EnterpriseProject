using System.Numerics;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    [SerializeField]
    [Tooltip("현재 보유 재화(골드)")]
    private BigInteger moneyUnit;

    public BigInteger MoneyUnit
    {
        get { return moneyUnit; }
        set 
        {
            moneyUnit = value;
            if (bum.contentsPanelObjs[(int)Contents.Colleague].activeSelf)
            {
                csm.TextColorChange();
            }
            if (bum.contentsPanelObjs[(int)Contents.Proficiency].activeSelf)
            {
                psm.TextReSettings();
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
            if (bum.contentsPanelObjs[(int)Contents.Colleague].activeSelf)
            {
                csm.TextColorChange();
            }
        }
    }

    [Tooltip("플레이어 기본 능력치 레벨들")]
    public int[] statsLevel;

    #region 싱글톤 인스턴스 모음
    [Tooltip("BattleUIManager 싱글톤 인스턴스")]
    BattleUIManager bum;

    [Tooltip("ColleagueSystemManager 싱글톤 인스턴스")]
    ColleagueSystemManager csm;

    [Tooltip("ProficiencySystemManager 싱글톤 인스턴스")]
    ProficiencySystemManager psm;
    #endregion

    private void Start()
    {
        bum = BattleUIManager.Instance;
        csm = ColleagueSystemManager.Instance;
        psm = ProficiencySystemManager.Instance;

        moneyUnit = 100000;
    }
}
