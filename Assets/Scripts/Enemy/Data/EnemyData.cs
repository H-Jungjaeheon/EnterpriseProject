using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyType
{
    ShortDis = 0, LongDis = 1, Air = 2
}

[CreateAssetMenu(fileName = "EnemyData", menuName ="Scriptable/EnemyData")]
public class EnemyData : ScriptableObject
{
    [Header("기본 데이터")]
    public EnemyType EnemyType;
    public Sprite EnemyImg;
    public string EnemyName;
    public int Hp;
    public int AttackPower;
    public float AttackDistance;
    public float AttackDelay;
    public float MoveSpeed;
}
