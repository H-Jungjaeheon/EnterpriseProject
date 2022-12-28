using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//���� ����� �ִϸ�����

public enum AnimationType
{
    Move, Attack, Hit, Any
}

public class ClipList
{
    public AnimationType AnimationType;
    public AnimationClip AnimationClip; 
}

public class AnimationManager : MonoBehaviour
{
    [Header("���� ������Ʈ")]
    [SerializeField]
    private SpriteRenderer SpriteRenderer;

    [Header("�ִϸ��̼� ����")]
    [SerializeField]
    private List<ClipList> ClipList;
    [SerializeField]
    private float AnimationSpeed;
    [SerializeField]
    private Dictionary<AnimationType, AnimationClip> AnimationList;
}
