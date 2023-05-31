namespace Logic.Common.RedDot
{
    public struct RedDotInfo
    {

        public string uid;
        public RedDotStatus status;
        public int number;
        public RedDotInfo(string uid, RedDotStatus status, int number)
        {
            this.uid = uid;
            this.status = status;
            this.number = number;
        }

    }


    public enum RedDotStatus
    {
        Null,
        Normal, //普通红点
        Number, //数字红点
        New,    //新字红点
        Full,   //满字红点
    }

}