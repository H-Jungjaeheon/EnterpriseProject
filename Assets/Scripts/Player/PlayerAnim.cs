using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine;

namespace Spine.Unity.Examples
{
    public class PlayerAnim : MonoBehaviour
    {
        [SerializeField]
        List<GameObject> PlayerCharacters;

        #region Inspector
        [SpineAnimation]
        public string idleAnimationName;

        [SpineAnimation]
        public string walkAnimationName;

        public float runWalkDuration = 1.5f;
        #endregion

        SkeletonAnimation skeletonAnimation;

        public Spine.AnimationState spineAnimationState;
        public Spine.Skeleton skeleton;

        public void AnimationSetting()
        {
            PlayerCharacters[Player.Instance.SelectNumber].SetActive(true);

            for (int i = 0; i < PlayerCharacters.Count; i++)
            {
                if (Player.Instance.SelectNumber == i)
                    continue;

                PlayerCharacters[i].SetActive(false);
            }

            skeletonAnimation = PlayerCharacters[Player.Instance.SelectNumber].GetComponent<SkeletonAnimation>();
            spineAnimationState = skeletonAnimation.AnimationState;
            skeleton = skeletonAnimation.Skeleton;
        }

        public void OnWalkAnimation()
        {
            spineAnimationState.SetAnimation(0, walkAnimationName, true);
        }

        public void OnIdleAnimation()
        {
            spineAnimationState.SetAnimation(0, idleAnimationName, true);
        }
    }
}
