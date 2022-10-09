using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BulletType
{
    Enemy, Player
}

[CreateAssetMenu(fileName = "BulletData", menuName = "Scriptable/BulletData")]
public class BulletData : ScriptableObject
{
    [Header("�⺻ ������")]
    public BulletType BulletType;
    public Sprite BulletImg;
    public string BulletName;
    public int BulletPower;
    public float BulletSpeed;
}
