using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Animations;

//직접 만드는 애니메이터
public enum AnimationType
{
    Move, Attack, Hit, Any
}

[System.Serializable]
public class AnimationList
{
    public AnimatorControllerParameter AnimatorParameter;
    public AnimatorControllerParameterType AnimatorParameterType;
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
    private List<AnimationList> AnimationList;

    private void Awake()
    {
        gameObject.TryGetComponent<SpriteRenderer>(out SpriteRenderer);
        gameObject.TryGetComponent<Animator>(out Animator);
    }

    public void AnimationSetting(RuntimeAnimatorController Anim)
    {
        Controller = Anim;
        Animator.runtimeAnimatorController = Controller;

        if (Animator.parameterCount == Controller.animationClips.Length)
        {
            Debug.Log("True");
        }

        else
        {
            Debug.Log(Animator.parameterCount);
            Debug.Log(Controller.animationClips.Length);
        }
            
    }
}
