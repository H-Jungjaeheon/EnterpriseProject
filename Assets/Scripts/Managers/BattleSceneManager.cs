using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleSceneManager : Singleton<BattleSceneManager>
{
    [Header("재화 테스트")]
    [Tooltip("현재 재화")]
    [SerializeField]
    private int[] moneyUnit;

    [SerializeField]
    private int[] pmMoneyUnit;

    [SerializeField]
    private int[] pmMoneyUnitT;

    [Tooltip("텍스트로 재화 나타내기 테스트용")]
    [SerializeField]
    private Text test;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Q))
        {
            BattleUIManager.Instance.CalculationOfGoods(moneyUnit, pmMoneyUnit, test);
        }
        else if (Input.GetKey(KeyCode.W))
        {
            BattleUIManager.Instance.CalculationOfGoods(moneyUnit, pmMoneyUnitT, test);
        }
    }
}
