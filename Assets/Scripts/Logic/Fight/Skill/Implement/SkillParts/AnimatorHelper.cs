using System.Collections;
using UnityEngine;

namespace Logic.Fight.Skill.Implement
{
    public class AnimatorHelper
    {
        /// <summary>
        /// 检测是否播放完成
        /// </summary>
        /// <param name="playAnimationName"></param>
        /// <param name="action"></param>
        /// <param name="IsBack">是否倒放</param>
        /// <returns></returns>
        public IEnumerator CheckAnimationComplete(Animator animator, string playAnimationName, System.Action action, bool IsBack = false)
        {
            bool condition = true;

            while (condition)
            {
                if (IsBack)
                {
                    condition = !animator.GetCurrentAnimatorStateInfo(0).IsName(playAnimationName)
                    || animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0f;
                }
                else
                {
                    condition = !animator.GetCurrentAnimatorStateInfo(0).IsName(playAnimationName)
                    || animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1f;
                }

                yield return null;
            }
            action?.Invoke();
        }
    }
}