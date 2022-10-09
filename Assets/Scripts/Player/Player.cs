using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Player : MonoBehaviour
{
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
    private PlayerRange Range;
    [SerializeField]
    private bool OnAttack = false;
    private Coroutine AttackCorutine;

    [Header("숙련도 변수")]
    //[SerializeField]
    //플레이어 숙련도 스크립터블 들어갈 예정
    [SerializeField]
    private BulletData[] BulletData;

    private void Update()
    {
        if(OnAttack == false && Range.TargetEnemy.Count > 0)
        {
            StartAttack();
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
