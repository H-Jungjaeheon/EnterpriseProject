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
    [Tooltip("���� ���� ��ȭ(����)")]
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

    [Tooltip("�÷��̾� �⺻ �ɷ�ġ ������")]
    public int[] statsLevel;

    #region �̱��� �ν��Ͻ� ����
    [Tooltip("BattleUIManager �̱��� �ν��Ͻ�")]
    BattleUIManager bum;

    [Tooltip("ColleagueSystemManager �̱��� �ν��Ͻ�")]
    ColleagueSystemManager csm;

    [Tooltip("ProficiencySystemManager �̱��� �ν��Ͻ�")]
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
