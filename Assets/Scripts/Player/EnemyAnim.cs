using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine;

namespace Spine.Unity.Examples
{
    public class EnemyAnim : MonoBehaviour
    {
        [SerializeField]
        List<GameObject> EnemyCharacters;

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

        public void AnimationSetting(int Select)
        {
            EnemyCharacters[Select].SetActive(true);

            for (int i = 0; i < EnemyCharacters.Count; i++)
            {
                if (Select == i)
                    continue;

                EnemyCharacters[Select].SetActive(false);
            }

            skeletonAnimation = EnemyCharacters[Select].GetComponent<SkeletonAnimation>();
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
