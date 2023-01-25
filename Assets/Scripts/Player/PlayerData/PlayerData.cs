using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerData", menuName = "Scriptable/PlayerData")]
public class PlayerData : ScriptableObject
{
    [Header("�⺻ ������")]
    public Sprite PlayerSkinImg;
    public string PlayerSkinName;

    public int Hp;
    public int HealingValue;

    public int AttackPower;
    public float AttackDelay;

    public float CriticalPercent;
    public float CriticalDamage;
    [Header("�ʿ� ��ȭ")]
    public float BuyCost;
    [Header("�ִϸ��̼�")]
    public RuntimeAnimatorController RuntimeAnimatorController;
}
