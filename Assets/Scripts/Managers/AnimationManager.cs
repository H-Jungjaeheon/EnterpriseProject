using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

public class AnimationManager : MonoBehaviour
{
    [Header("상위 오브젝트")]
    [SerializeField]
    private SpriteRenderer SpriteRenderer;

    [Header("애니메이션 관련")]
    [SerializeField]
    private RuntimeAnimatorController RuntimeAnimatorController;
    [SerializeField]
    private Sprite OriginalSprite;
    [SerializeField]
    private List<ClipList> ClipList = new List<ClipList>();
    [SerializeField]
    private float AnimationSpeed;
    [SerializeField]
    private Dictionary<AnimationType, AnimationClip> AnimationList = new Dictionary<AnimationType, AnimationClip>();
    [SerializeField]
    private Coroutine PlayAnimCorutine;

    private void Start()
    {
        SpriteRenderer = this.GetComponent<SpriteRenderer>();

        OriginalSprite = SpriteRenderer.sprite;

        Debug.Log(ClipList.Count);
    }

    private void BasicSetting()
    {
        for (int i = 0; i < ClipList.Count; i++)
        {
            Debug.Log("123");
            AnimationList.Add(ClipList[i].AnimationType, ClipList[i].AnimationClip);
        }
    }

    public void PlayAnimation(AnimationType Type, float Speed = 1.0f, bool Loop = false)
    {
        PlayAnimCorutine = StartCoroutine(PlayAnimationCorutine(Type, Speed, Loop));
    }

    public void StopAnimation()
    {
        StopCoroutine(PlayAnimCorutine);
        SpriteRenderer.sprite = OriginalSprite;
    }

    public void ChageState(AnimationType Type)
    {
        switch (Type)
        {
            case AnimationType.Move:
                break;
            case AnimationType.Attack:
                break;
            case AnimationType.Hit:
                break;
            case AnimationType.Any:
                break;
            default:
                break;
        }
    }

    IEnumerator PlayAnimationCorutine(AnimationType Type, float Speed = 1.0f, bool Loop = false)
    {
        yield return null;

        while (true)
        {
            
        }
    }
}
