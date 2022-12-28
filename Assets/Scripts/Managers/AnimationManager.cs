using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//직접 만드는 애니메이터

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
    [Header("상위 오브젝트")]
    [SerializeField]
    private SpriteRenderer SpriteRenderer;

    [Header("애니메이션 관린")]
    [SerializeField]
    private List<ClipList> ClipList;
    [SerializeField]
    private float AnimationSpeed;
    [SerializeField]
    private Dictionary<AnimationType, AnimationClip> AnimationList;
}
