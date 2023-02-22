using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;

public class Player : MonoBehaviour
{
    public static Player Instance = null;

    [Header("�÷��̾� ���õ� ������")]
    [SerializeField]
    private PlayerData[] PlayerDatas;
    public int SelectNumber;
    [SerializeField]
    private PlayerData SelectPlayerData;

    [Header("�÷��̾� �⺻ ���� ����")]
    public int MaxHp;
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

    public int HealingValue; 
    public int AttackPower;
    public float AttackDelay;
    public float CriticalPercent;
    public float CriticalDamage;

    [Header("���� ����")]
    [SerializeField]
    public PlayerRange Range;
    [SerializeField]
    private bool IsAttack = false;
    private Coroutine AttackCorutine;

    [Header("��ų ����")]
    public SkillManager SkillManager;

    [Header("�̵� ����")]
    [SerializeField]
    private float MoveTime;
    [SerializeField]
    private float MoveSpeed;
    [SerializeField]
    public float StendPosX;
    [SerializeField]
    private float MovePosX;
    [SerializeField]
    bool IsMove = false;
    private Coroutine MoveCorutine;

    [Header("���õ� ����")]
    //[SerializeField]
    //�÷��̾� ���õ� ��ũ���ͺ� �� ����
    public BulletData[] BulletData;

    [Header("�ִϸ��̼�")]
    private AnimationManager AnimationManager;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        BasicSetting();
        StartCoroutine(HealingHp());
    }

    private void Update()
    {
        OnAttack();
        OnMove();
    }

    public void BasicSetting()
    {
        SelectPlayerData = PlayerDatas[SelectNumber];

        this.MaxHp = SelectPlayerData.Hp;
        this.Hp = SelectPlayerData.Hp;
        this.HealingValue = SelectPlayerData.HealingValue;

        this.AttackPower = SelectPlayerData.AttackPower;
        this.AttackDelay = SelectPlayerData.AttackDelay;

        this.CriticalDamage = SelectPlayerData.CriticalDamage;
        this.CriticalPercent = SelectPlayerData.CriticalPercent;

        //this.GetComponent<SpriteRenderer>().sprite = SelectPlayerData.PlayerSkinImg;

        this.gameObject.TryGetComponent<AnimationManager>(out AnimationManager);

        AnimationManager.AnimationSetting(SelectPlayerData.RuntimeAnimatorController);
    }

    public void CharacterChange(int idx)
    {
        SelectNumber = idx;
        BasicSetting();
    }

    #region Attack
    void OnAttack()
    {
        if (IsAttack == false && Range.TargetEnemy.Count > 0)
        {
            StartAttack();
            AnimationManager.ChangeAnimation("Attack");
        }

        else if (IsAttack == true && Range.TargetEnemy.Count <= 0)
        {
            StopAttack();
            AnimationManager.ChangeAnimation("Move");
        }
    }

    void StartAttack()
    {
        IsAttack = true;
        IsMove = false;

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

    IEnumerator HealingHp()
    {
        yield return new WaitForSeconds(1.0f);

        if (hp + HealingValue <= MaxHp)
        {
            hp += HealingValue;
        }

        else
            hp = MaxHp;

        StartCoroutine(HealingHp());
        yield break;
    }

    void OnMove()
    {
        Vector2 GoalPos = new Vector2(Range.TargetEnemy.Count <= 0 ? MovePosX : StendPosX, this.transform.position.y);
        this.transform.position = Vector2.Lerp(this.transform.position, GoalPos, MoveSpeed * Time.deltaTime);

        if(IsMove == false && IsAttack == false && GoalPos.x == MovePosX)
        {
            IsMove = true;
        }

        if (GoalPos.x == MovePosX)
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
