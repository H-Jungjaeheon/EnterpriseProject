using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleSceneManager : Singleton<BattleSceneManager>
{
    [Header("��ȭ �׽�Ʈ")]
    [Tooltip("���� ��ȭ")]
    [SerializeField]
    private int[] moneyUnit;

    [SerializeField]
    private int[] pmMoneyUnit;

    [SerializeField]
    private int[] pmMoneyUnitT;

    [Tooltip("�ؽ�Ʈ�� ��ȭ ��Ÿ���� �׽�Ʈ��")]
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
