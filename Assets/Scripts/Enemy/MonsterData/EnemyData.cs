using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyType
{
    ShortDis, LongDis, Air
}

[CreateAssetMenu(fileName = "EnemyData", menuName ="Scriptable/EnemyData")]
public class EnemyData : ScriptableObject
{
    [Header("기본 데이터")]
    public EnemyType EnemyType;
    public Sprite EnemyImg;
    public string EnemyName;
    public int Hp;
    public float AttackPower;
    public float AttackDistance;
    public float MoveSpeed;
}
