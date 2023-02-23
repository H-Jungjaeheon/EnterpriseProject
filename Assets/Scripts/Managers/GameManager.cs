using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;

using UnityEngine.UI;
using DG.Tweening;

public class GameManager : Singleton<GameManager>
{
    [SerializeField]
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

    public int[] statsLevel;

    [Header("Ω∫≈◊¿Ã¡ˆ µ•¿Ã≈Õ")]
    public List<GameObject> BackGrounds;
    public int Difficult = 0;
    public int Stage = 0;
    public Image SceneChagePanel;
    public Coroutine SceneChangeCoroutine = null;

    #region ΩÃ±€≈Ê ¿ŒΩ∫≈œΩ∫ ∏¿Ω
    [Tooltip("BattleUIManager ΩÃ±€≈Ê ¿ŒΩ∫≈œΩ∫")]
    BattleUIManager bum;

    [Tooltip("ColleagueSystemManager ΩÃ±€≈Ê ¿ŒΩ∫≈œΩ∫")]
    ColleagueSystemManager csm;

    [Tooltip("ProficiencySystemManager ΩÃ±€≈Ê ¿ŒΩ∫≈œΩ∫")]
    ProficiencySystemManager psm;
    #endregion

    private void Start()
    {
        bum = BattleUIManager.Instance;
        csm = ColleagueSystemManager.Instance;
        psm = ProficiencySystemManager.Instance;
    }

    public void StartSceneChange()
    {
        if (SceneChangeCoroutine == null)
        {
            SceneChangeCoroutine = StartCoroutine(SceneChange());
        }
    }

    private IEnumerator SceneChange()
    {
        yield return null;

        SceneChagePanel.gameObject.SetActive(true);
        SceneChagePanel.DOFade(1.0f, 0.5f);
        yield return new WaitForSeconds(0.5f);

        BackGrounds[Stage].SetActive(false);
        Stage = Stage < BackGrounds.Count - 1 ? (Stage + 1) : 0;
        BackGrounds[Stage].SetActive(true);
        yield return new WaitForSeconds(0.2f);

        SceneChagePanel.DOFade(1.0f, 0.5f);
        yield return new WaitForSeconds(0.5f);

        SceneChagePanel.gameObject.SetActive(false);

        StopCoroutine(SceneChangeCoroutine);
        SceneChangeCoroutine = null;
        yield break;
    }
}
