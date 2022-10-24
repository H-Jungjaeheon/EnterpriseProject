using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BehaviorType
{
    Move, Attack, Stance
}

public class Enemy : MonoBehaviour
{
    [Header("Enemy 부모 변수")]
    public EnemyData[] Data;
    public SpriteRenderer EnemySprite;
    public Transform TargetPos;
    public BehaviorType CurBehaviorType;

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
                Player.Instance.Range.TargetEnemy.Remove(this.gameObject);
                EnemySpawner.ReturnEnemy(EnemyType, this);
            }
        }
    }

    [SerializeField]
    private int AttackPower;
    [SerializeField]
    private float AttackDistance;
    [SerializeField]
    private float AttackDelay;
    [SerializeField]
    private bool IsAttack;
    private Coroutine AttackCorutine;
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

    #region Attack
    void StartAttack()
    {
        IsAttack = true;
        AttackCorutine = StartCoroutine(Attack());
    }

    void StopAttack()
    {
        IsAttack = false;
        StopCoroutine(AttackCorutine);
    }

    protected virtual IEnumerator Attack()
    {
        yield return null;

        while (TargetPos.gameObject.activeSelf != false)
        {
            //Debug.Log("Attack");
            yield return new WaitForSeconds(AttackDelay);
            Player.Instance.Hp -= AttackPower;
            
        }
    }
    #endregion

    protected virtual void Move()
    {
        if (Vector2.Distance(TargetPos.position, this.transform.position) > AttackDistance)
        {
            SwichBehaviorType(BehaviorType.Move);
            this.gameObject.transform.position -= new Vector3(MoveSpeed * Time.deltaTime, 0, 0);
        }

        else
            SwichBehaviorType(BehaviorType.Attack);
    }

    void SwichBehaviorType(BehaviorType Type)
    {
        if (CurBehaviorType != Type)
        {
            CurBehaviorType = Type;

            switch (Type)
            {
                case BehaviorType.Attack:
                    StartAttack();
                    break;

                case BehaviorType.Move:
                    StopAttack();
                    break;

                case BehaviorType.Stance:
                    break;
            }
        }
    }

    //데이터 값 세팅
    public void BasicSetting(int Select)
    {
        TargetPos = FindObjectOfType<Player>().transform;

        this.EnemyType = Data[Select].EnemyType;
        this.EnemySprite.sprite = Data[Select].EnemyImg;
        this.EnemyName = Data[Select].EnemyName;

        this.Hp = Data[Select].Hp;
        this.AttackPower = Data[Select].AttackPower;
        this.AttackDistance = Data[Select].AttackDistance;
        this.AttackDelay = Data[Select].AttackDelay;
        this.MoveSpeed = Data[Select].MoveSpeed;
    }
}
