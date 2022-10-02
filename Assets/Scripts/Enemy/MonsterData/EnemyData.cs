using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MonsterType
{
    ShortDis, LongDis, Air
}

[CreateAssetMenu(fileName = "EnemyData", menuName ="Scriptable/Data")]
public class EnemyData : ScriptableObject
{
    [Header("기본 데이터")]
    public int Hp;
    public float AttackPower;
    public float AttackDistance;
    public float MoveSpeed;
    public MonsterType MonsterType;
    public Sprite MonsterImg;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
