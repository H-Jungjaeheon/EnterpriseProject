using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerData", menuName = "Scriptable/PlayerData")]
public class PlayerData : ScriptableObject
{
    [Header("기본 데이터")]
    public Sprite PlayerSkinImg;
    public string PlayerSkinName;

    public int Hp;
    public int HealingValue;

    public int AttackPower;
    public float AttackDelay;

    public float CriticalPercent;
    public float CriticalDamage;
    [Header("필요 재화")]
    public float BuyCost;
    [Header("애니메이션")]
    public RuntimeAnimatorController RuntimeAnimatorController;
}
