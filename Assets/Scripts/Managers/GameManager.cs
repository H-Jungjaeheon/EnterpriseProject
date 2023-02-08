using System.Numerics;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    [SerializeField]
    [Tooltip("���� ���� ��ȭ(���)")]
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
    [Tooltip("���� ���� ��ȭ(����)")]
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

    [Tooltip("�÷��̾� �⺻ �ɷ�ġ ������")]
    public int[] statsLevel;
}
