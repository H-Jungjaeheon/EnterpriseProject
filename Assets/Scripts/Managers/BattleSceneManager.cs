using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleSceneManager : Singleton<BattleSceneManager>
{
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            //BattleUIManager.Instance.CalculationOfGoods(moneyUnit, pmMoneyUnit, test);
        }
        else if (Input.GetKey(KeyCode.W))
        {
            //BattleUIManager.Instance.CalculationOfGoods(moneyUnit, pmMoneyUnitT, test);
        }
    }
}
