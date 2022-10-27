using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PartnerData", menuName ="Scriptable/PartnerData")]
public class PartnerData : ScriptableObject
{
    [Header("�⺻ ������")]
    public Sprite PartnerImg;
    public string PartnerName;
    public float AttackPower;
    public float AttackDelay;
    [Header("�⺻ ���� ������")]
    public float AttackBuff;
    public float HpBuff;
    [Header("�ʿ� ��ȭ")]
    public float BuyCost;
    public float UpgreadCost;
}
