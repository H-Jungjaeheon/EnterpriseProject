using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class AnimationList
{
    public string AnimatorParameter;
    public AnimationClip AnimationClip;
    public bool Check = false;

    public AnimationList(string animatorParameter, AnimationClip animationClip)
    {
        AnimatorParameter = animatorParameter;
        AnimationClip = animationClip;
    }
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
    private List<AnimationList> AnimationList = null;

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
            for (int i = 0; i < Animator.parameterCount; i++)
            {
                AnimationList.Add(new AnimationList(Animator.parameters[i].name, Controller.animationClips[i]));
            }

            ChangeAnimation();
        }

        else
        {
            Debug.Log("Animation Error");
            Debug.Log(Animator.parameterCount);
            Debug.Log(Controller.animationClips.Length);
        }
    }

    #region 애니메이션 바꿈 오버로드 함수
    //첫 번째 등록된 애니메이션 설정
    public void ChangeAnimation()
    {
        for (int i = 0; i < AnimationList.Count; i++)
        {
            if(i == 0)
            {
                AnimationList[i].Check = true;
                Animator.SetBool(AnimationList[i].AnimatorParameter, AnimationList[i].Check);
            }

            else
            {
                AnimationList[i].Check = false;
                Animator.SetBool(AnimationList[i].AnimatorParameter, AnimationList[i].Check);
            }
        }
    }

    //원하는 파라미터 애니메이션 설정
    public void ChangeAnimation(string Paramater)
    {
        for (int i = 0; i < AnimationList.Count; i++)
        {
            if (AnimationList[i].AnimatorParameter == Paramater)
            {
                AnimationList[i].Check = true;
                Animator.SetBool(AnimationList[i].AnimatorParameter, AnimationList[i].Check);
            }

            else
            {
                AnimationList[i].Check = false;
                Animator.SetBool(AnimationList[i].AnimatorParameter, AnimationList[i].Check);
            }
        }
    }

    //원하는 순서의 애니메이션 설정
    public void ChangeAnimation(int Count)
    {
        for (int i = 0; i < AnimationList.Count; i++)
        {
            if (i == Count)
            {
                AnimationList[i].Check = true;
                Animator.SetBool(AnimationList[i].AnimatorParameter, AnimationList[i].Check);
            }

            else
            {
                AnimationList[i].Check = false;
                Animator.SetBool(AnimationList[i].AnimatorParameter, AnimationList[i].Check);
            }
        }
    }
    #endregion
}
