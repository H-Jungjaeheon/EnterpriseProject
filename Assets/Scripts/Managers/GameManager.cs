using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;

public class GameManager : Singleton<GameManager> 
{
    public class SaveDatas
    {
        
    }

    
    [Tooltip("���� ���� ��ȭ(���)")]
    [SerializeField]
    private BigInteger moneyUnit;

    public BigInteger MoneyUnit
    {
        get { return moneyUnit; }
        set { moneyUnit = value; }
    }

    
    [Tooltip("���� ���� ��ȭ(����)")]
    [SerializeField]
    private BigInteger gemUnit;

    public BigInteger GemUnit
    {
        get { return gemUnit; }
        set { gemUnit = value; }
    }

    [Tooltip("�÷��̾� �⺻ �ɷ�ġ ������")]
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
