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

    [Header("애니메이션 관린")]
    [SerializeField]
    private Sprite OriginalSprite;
    [SerializeField]
    private List<ClipList> ClipList;
    [SerializeField]
    private float AnimationSpeed;
    [SerializeField]
    private Dictionary<AnimationType, AnimationClip> AnimationList;
    [SerializeField]
    private Coroutine PlayAnimCorutine;

    private void Start()
    {
        SpriteRenderer = this.GetComponent<SpriteRenderer>();

        OriginalSprite = SpriteRenderer.sprite;

        Debug.Log(ClipList.Count);
        BasicSetting();
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

    }

    IEnumerator PlayAnimationCorutine(AnimationType Type, float Speed = 1.0f, bool Loop = false)
    {
        yield return null;

        while (true)
        {
            ClipList[0].AnimationClip.SampleAnimation(this.gameObject, 0.5f);
        }
    }
}
