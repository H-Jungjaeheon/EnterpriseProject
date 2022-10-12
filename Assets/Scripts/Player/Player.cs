using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Player : MonoBehaviour
{
    public static Player Instance = null;

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
    private float MoveSpeed;
    [SerializeField]
    public float StendPosX;
    [SerializeField]
    private float MovePosX;
    [SerializeField]
    private bool IsMove = false;
    private Coroutine MoveCorutine;

    [Header("숙련도 변수")]
    //[SerializeField]
    //플레이어 숙련도 스크립터블 들어갈 예정
    [SerializeField]
    private BulletData[] BulletData;

    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        OnAttack();
        OnMove();
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
            Debug.Log("Attack");
            yield return new WaitForSeconds(AttackDelay);
            PlayerBulletObjectPool.Instance.GetBullet(Range.TargetEnemy[0]);
        }
    }
    #endregion

    #region Move
    void OnMove()
    {
        if(Range.TargetEnemy.Count <= 0 && IsMove == false)
        {
            TowardMove();
        }

        else if(Range.TargetEnemy.Count > 0 && IsMove == false)
        {
            BackMove();
        }
    }

    void TowardMove()
    {
        IsMove = true;
        MoveCorutine = StartCoroutine(Move("Toward"));
    }

    void BackMove()
    {
        IsMove = true;
        MoveCorutine = StartCoroutine(Move("Back"));
    }

    IEnumerator Move(string MoveDir)
    {
        Vector2 GoalPos = new Vector2(MoveDir == "Toward" ? MovePosX : StendPosX, this.transform.position.y);

        while (true)
        {
            yield return null;

            this.transform.position = Vector2.Lerp(this.transform.position, GoalPos, MoveSpeed * Time.deltaTime);

            if(MoveDir == "Toward" && this.transform.position.x >= GoalPos.x - 0.005f)
            {
                break;
            }

            else if(MoveDir != "Toward" && this.transform.position.x <= GoalPos.x + 0.005f)
            {
                break;
            }
        }

        this.transform.position = GoalPos;

        StopCoroutine(MoveCorutine);

        yield break;
    }
    #endregion
}
