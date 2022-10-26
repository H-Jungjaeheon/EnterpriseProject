using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;

public class GameManager : Singleton<GameManager> 
{
    public class SaveDatas
    {
        
    }

    
    [SerializeField]
    [Tooltip("현재 보유 재화(골드)")]
    private BigInteger moneyUnit;

    public BigInteger MoneyUnit
    {
        get { return moneyUnit; }
        set 
        {
            moneyUnit = value;
            ColleagueSystemManager.Instance.TextColorChange();
        }
    }

    
    [SerializeField]
    [Tooltip("현재 보유 재화(보석)")]
    private BigInteger gemUnit;

    public BigInteger GemUnit
    {
        get { return gemUnit; }
        set 
        {
            gemUnit = value;
            ColleagueSystemManager.Instance.TextColorChange();
        }
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
        if (Input.GetKeyDown(KeyCode.Space))
        {
            MoneyUnit += 10;
            GemUnit += 10;
        }
    }
}
