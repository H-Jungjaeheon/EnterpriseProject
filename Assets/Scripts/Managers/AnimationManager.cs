using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

//직접 만드는 애니메이터

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
    [Header("상위 오브젝트")]
    [SerializeField]
    private SpriteRenderer SpriteRenderer;
    [SerializeField]
    private Animator Animator;

    [Header("애니메이션 관련")]
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
