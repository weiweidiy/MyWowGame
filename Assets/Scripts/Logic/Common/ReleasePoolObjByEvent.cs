using Framework.Pool;
using UnityEngine;

namespace Logic.Common
{
    /// <summary>
    /// 通过事件回收对象
    /// 通常用于特效 等 序列帧播放完成后 调用事件回收
    /// </summary>
    public class ReleasePoolObjByEvent : MonoBehaviour
    {
        public void OnRelease()
        {
            FightObjPool.Ins.Recycle(gameObject);
        }
    }
}