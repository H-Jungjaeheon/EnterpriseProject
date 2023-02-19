using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

//���� ����� �ִϸ�����

public enum AnimationType
{
    Move, Attack, Hit, Any
}

[System.Serializable]
public class ClipList
{
    public AnimationType AnimationType;
    public AnimationClip AnimationClip;
}

[RequireComponent(typeof(Animator))]
public class AnimationManager : MonoBehaviour
{
    [Header("���� ������Ʈ")]
    [SerializeField]
    private SpriteRenderer SpriteRenderer;
    [SerializeField]
    private Animator Animator;

    [Header("�ִϸ��̼� ����")]
    [SerializeField]
    private RuntimeAnimatorController Controller;
    [SerializeField]
    private List<ClipList> ClipList = new List<ClipList>();
    [SerializeField]
    private AnimationClip[] Animations;
    [SerializeField]
    private float AnimationSpeed;
    [SerializeField]
    private Dictionary<AnimationType, AnimationClip> AnimationList = new Dictionary<AnimationType, AnimationClip>();
    [SerializeField]
    private Coroutine PlayAnimCorutine;

    private void Start()
    {
        gameObject.TryGetComponent<SpriteRenderer>(out SpriteRenderer);
        gameObject.TryGetComponent<Animator>(out Animator);
    }

    public void BasicSetting(RuntimeAnimatorController Anim)
    {
        Controller = Anim;
        Animations = Controller.animationClips;
    }
}
