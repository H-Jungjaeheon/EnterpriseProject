using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Player : MonoBehaviour
{
    public static Player Instance = null;

    [Header("�÷��̾� �⺻ ���� ����")]
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

    [Header("���� ����")]
    [SerializeField]
    public PlayerRange Range;
    [SerializeField]
    private bool OnAttack = false;
    private Coroutine AttackCorutine;

    [Header("���õ� ����")]
    //[SerializeField]
    //�÷��̾� ���õ� ��ũ���ͺ� �� ����
    [SerializeField]
    private BulletData[] BulletData;

    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        if(OnAttack == false && Range.TargetEnemy.Count > 0)
        {
            StartAttack();
        }

        else if(OnAttack == true && Range.TargetEnemy.Count <= 0)
        {
            StopAttack();
        }
    }

    void StartAttack()
    {
        OnAttack = true;
        AttackCorutine = StartCoroutine(Attack());
    }

    void StopAttack()
    {
        OnAttack = false;
        StopCoroutine(AttackCorutine);
    }

    IEnumerator Attack()
    {
        yield return null;

        while (Range.TargetEnemy.Count > 0)
        {
            yield return new WaitForSeconds(AttackDelay);
            PlayerBulletObjectPool.Instance.GetBullet(Range.TargetEnemy[0]);
        }
    }
}
