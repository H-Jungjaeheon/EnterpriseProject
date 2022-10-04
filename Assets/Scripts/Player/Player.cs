using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    [SerializeField]
    private float AttackPower;
    [SerializeField]
    private float AttackSpeed;
    [SerializeField]
    private float CriticalPercent;
    [SerializeField]
    private float CriticalDamage;

    [Header("숙련도 변수")]
    //[SerializeField]
    //플레이어 숙련도 스크립터블 들어갈 예정
    [SerializeField]
    private BulletData[] BulletData;
}
