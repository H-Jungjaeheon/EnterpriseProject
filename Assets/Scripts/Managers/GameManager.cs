using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager> 
{
    public class SaveDatas
    {
        
    }

    [SerializeField]
    [Tooltip("���� ���� ��ȭ(���)")]
    private int moneyUnit;

    public int MoneyUnit
    {
        get { return moneyUnit; }
        set { moneyUnit = value; }
    }

    [SerializeField]
    [Tooltip("���� ���� ��ȭ(����)")]
    private int jewelryUnit;

    public int JewelryUnit
    {
        get { return jewelryUnit; }
        set { jewelryUnit = value; }
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
