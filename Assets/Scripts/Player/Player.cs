using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Player : MonoBehaviour
{
    public static Player Instance = null;

    [Header("플레이어 숙련도 데이터")]
    [SerializeField]
    private PlayerData[] PlayerDatas;
    [SerializeField]
    private int SelectNumber;
    [SerializeField]
    private PlayerData SelectPlayerData;

    [Header("플레이어 기본 스탯 변수")]
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

            if (hp <= 0)
            {
                //Die
            }
        }
    }
    [SerializeField]
    private int HealingValue; 
    
    public int[] AttackPower;

    [SerializeField]
    private float AttackDelay;
    [SerializeField]
    private float CriticalPercent;
    [SerializeField]
    private float CriticalDamage;

    [Header("전투 변수")]
    [SerializeField]
    public PlayerRange Range;
    [SerializeField]
    private bool IsAttack = false;
    private Coroutine AttackCorutine;

    [Header("이동 변수")]
    [SerializeField]
    private float MoveTime;
    [SerializeField]
    private float MoveSpeed;
    [SerializeField]
    public float StendPosX;
    [SerializeField]
    private float MovePosX;
    private Coroutine MoveCorutine;

    [Header("숙련도 변수")]
    //[SerializeField]
    //플레이어 숙련도 스크립터블 들어갈 예정
    [SerializeField]
    private BulletData[] BulletData;

    private void Awake()
    {
        Instance = this;

        BasicSetting();
    }

    private void Update()
    {
        OnAttack();
        OnMove();
    }

    void BasicSetting()
    {
        SelectPlayerData = PlayerDatas[SelectNumber];

        this.Hp = SelectPlayerData.Hp;
        this.HealingValue = SelectPlayerData.HealingValue;

        //this.AttackPower = SelectPlayerData.AttackPower;
        this.AttackDelay = SelectPlayerData.AttackDelay;

        this.CriticalDamage = SelectPlayerData.CriticalDamage;
        this.CriticalPercent = SelectPlayerData.CriticalPercent;
    }

    #region Attack
    void OnAttack()
    {
        if (IsAttack == false && Range.TargetEnemy.Count > 0)
        {
            StartAttack();
        }

        else if (IsAttack == true && Range.TargetEnemy.Count <= 0)
        {
            StopAttack();
        }
    }

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

    IEnumerator Attack()
    {
        yield return null;

        while (Range.TargetEnemy.Count > 0)
        {
            //Debug.Log("Attack");
            yield return new WaitForSeconds(AttackDelay);
            PlayerBulletObjectPool.Instance.GetBullet(Range.TargetEnemy[0]);
        }
    }
    #endregion

    void OnMove()
    {
        Vector2 GoalPos = new Vector2(Range.TargetEnemy.Count <= 0 ? MovePosX : StendPosX, this.transform.position.y);
        this.transform.position = Vector2.Lerp(this.transform.position, GoalPos, MoveSpeed * Time.deltaTime);

        if(GoalPos.x == MovePosX)
        {
            MoveTime += Time.deltaTime;

            if(MoveTime > 2)
            {
                EnemySpawner.Instance.StartEnemySpawn();
                MoveTime = 0;
            }
        }
    }
}
