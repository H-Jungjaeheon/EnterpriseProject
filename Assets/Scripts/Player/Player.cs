using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
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
    [SerializeField]
    private float AttackPower;
    [SerializeField]
    private float AttackSpeed;
    [SerializeField]
    private float CriticalPercent;
    [SerializeField]
    private float CriticalDamage;

    [Header("���õ� ����")]
    //[SerializeField]
    //�÷��̾� ���õ� ��ũ���ͺ� �� ����
    [SerializeField]
    private BulletData[] BulletData;
}
