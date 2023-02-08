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
            if (BattleUIManager.Instance.contentsPanelObjs[(int)Contents.Colleague].activeSelf)
            {
                ColleagueSystemManager.Instance.TextColorChange();
            }
            if (BattleUIManager.Instance.contentsPanelObjs[(int)Contents.Proficiency].activeSelf)
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
            if (BattleUIManager.Instance.contentsPanelObjs[(int)Contents.Colleague].activeSelf)
            {
                ColleagueSystemManager.Instance.TextColorChange();
            }
        }
    }

    [Tooltip("플레이어 기본 능력치 레벨들")]
    public int[] statsLevel;
}
