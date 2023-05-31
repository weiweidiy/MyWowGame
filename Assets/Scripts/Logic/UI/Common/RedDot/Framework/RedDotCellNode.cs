using System;

namespace Logic.Common.RedDot
{
    public abstract class RedDotCellNode : RedDotMono
    {
        /// <summary>
        /// 当不是父亲节点时，该属性代表唯一id标识符
        /// </summary>
        public string Uid;

        protected override void DoRegist(string key, Action<string, RedDotInfo, string> action)
        {
            GetRedDotManager().Regist(key, OnStatusUpdate, Uid);
        }

        protected override void DoUnRegist(string key, Action<string, RedDotInfo, string> onStatusUpdate)
        {
            GetRedDotManager().UnRegist(key, OnStatusUpdate);
        }

        protected override string GetTextContent(RedDotInfo showInfo)
        {
            return showInfo.status == RedDotStatus.Number ? showInfo.number.ToString() : "";
        }

        protected override int GetSpriteIndex(RedDotInfo info)
        {
            switch (info.status)
            {
                case RedDotStatus.New:
                    return 1;
                case RedDotStatus.Full:
                    return 2;
                default:
                    return 0;
            }
        }
    }

}