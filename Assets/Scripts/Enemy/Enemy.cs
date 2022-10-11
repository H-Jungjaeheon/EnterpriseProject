using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Enemy 부모 변수")]
    public EnemyData[] Data;
    public SpriteRenderer EnemySprite;

    [Header("데이터 받아올 변수")]
    [SerializeField]
    private EnemyType EnemyType;
    [SerializeField]
    private string EnemyName;
    [SerializeField]
    private int hp;
    public int Hp 
    {
        get
        {
            return hp;
        }

        set
        {
            hp = value;

            if(hp <= 0)
            {
                EnemySpawner.ReturnEnemy(EnemyType, this);
                Player.Instance.Range.TargetEnemy.Remove(this.gameObject);
            }
        }
    }

    [SerializeField]
    private float AttackPower;
    [SerializeField]
    private float AttackDistance;
    [SerializeField]
    private float MoveSpeed;

    private void Awake()
    {
        EnemySprite = this.GetComponent<SpriteRenderer>();
    }

    protected virtual void Update()
    {
        Move();
    }

    protected virtual void Attack()
    {

    }

    protected virtual void Move()
    {
        this.gameObject.transform.position -= new Vector3(MoveSpeed * Time.deltaTime, 0, 0);
    }

    //데이터 값 세팅
    public void BasicSetting(int Select)
    {
        this.EnemyType = Data[Select].EnemyType;
        this.EnemySprite.sprite = Data[Select].EnemyImg;
        this.EnemyName = Data[Select].EnemyName;

        this.Hp = Data[Select].Hp;
        this.AttackPower = Data[Select].AttackPower;
        this.AttackDistance = Data[Select].AttackDistance;
        this.MoveSpeed = Data[Select].MoveSpeed;
    }
}
