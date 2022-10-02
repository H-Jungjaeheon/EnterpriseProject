using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Enemy �θ� ����")]
    public EnemyData[] Data;
    public SpriteRenderer EnemySprite;

    [Header("������ �޾ƿ� ����")]
    [SerializeField]
    private int Hp;
    [SerializeField]
    private float AttackPower;
    [SerializeField]
    private float AttackDistance;
    [SerializeField]
    private float MoveSpeed;
    [SerializeField]
    private MonsterType MonsterType;

    private void Awake()
    {
        EnemySprite = this.GetComponent<SpriteRenderer>();
    }

    // Start is called before the first frame update
    protected virtual void Start()
    {
        BasicSetting(); 
    }

    protected virtual void Attack()
    {

    }

    protected virtual void Move()
    {

    }

    //������ �� ����
    private void BasicSetting()
    {
        int Select = 0;

        this.Hp = Data[Select].Hp;
        this.AttackPower = Data[Select].AttackPower;
        this.AttackDistance = Data[Select].AttackDistance;
        this.MoveSpeed = Data[Select].MoveSpeed;
        this.MonsterType = Data[Select].MonsterType;
        this.EnemySprite.sprite = Data[Select].MonsterImg;
    }
}
