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

        private void Update()
        {
            if(Input.GetKeyDown(KeyCode.E))
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

            if(Input.GetKeyDown(KeyCode.LeftArrow))
                spineAnimationState.SetAnimation(0, walkAnimationName, true);

            if(Input.GetKeyDown(KeyCode.RightArrow))
                spineAnimationState.SetAnimation(0, idleAnimationName, true);
        }
    }
}
