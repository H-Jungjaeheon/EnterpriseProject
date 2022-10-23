using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PartnerData", menuName ="Scriptable/PartnerData")]
public class PartnerData : ScriptableObject
{
    [Header("기본 데이터")]
    public Sprite PartnerImg;
    public string PartnerName;
    public float AttackPower;
    public float AttackDelay;
    [Header("기본 스탯 버프량")]
    public float AttackBuff;
    public float HpBuff;
    [Header("필요 재화")]
    public float BuyCost;
    public float UpgreadCost;
}
