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

    public int[] statsLevel;

    [Header("�������� ������")]
    public List<GameObject> BackGrounds;
    public int Difficult = 0;
    public int Stage = 0;
    public Image SceneChagePanel;
    public Coroutine SceneChangeCoroutine = null;

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
