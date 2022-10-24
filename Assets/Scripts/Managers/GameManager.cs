using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager> 
{
    public class SaveDatas
    {
        
    }

    [SerializeField]
    [Tooltip("현재 보유 재화(골드)")]
    private int moneyUnit;

    public int MoneyUnit
    {
        get { return moneyUnit; }
        set { moneyUnit = value; }
    }

    [SerializeField]
    [Tooltip("현재 보유 재화(보석)")]
    private int jewelryUnit;

    public int JewelryUnit
    {
        get { return jewelryUnit; }
        set { jewelryUnit = value; }
    }

    [Tooltip("플레이어 기본 능력치 레벨들")]
    public int[] statsLevel;
    

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
