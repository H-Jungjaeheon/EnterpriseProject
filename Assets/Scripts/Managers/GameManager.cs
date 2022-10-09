using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager> 
{
    public class SaveDatas
    {
        
    }

    [SerializeField]
    [Tooltip("현재 보유 재화")]
    private int[] moneyUnit;

    public int[] MoneyUnit
    {
        get { return moneyUnit; }
        set { moneyUnit = value; }
    }

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
