using Cysharp.Threading.Tasks;
using Framework.EventKit;
using Framework.GameFSM;
using Framework.UI;
using Logic.Common;
using Logic.Config;
using Logic.Data;
using Logic.UI.UILoading;
using Logic.UI.UILogin;
using Networks;

namespace Logic.States.Game
{
    /// <summary>
    /// 加载进入游戏 状态
    /// </summary>
    public class GS_Loading : IState<GameState, GameStateData>
    {
        public GS_Loading(GameState pType) : base(pType)
        {
            
        }

        public override async void Enter(GameStateData pContext)
        {
            //等待加载完成事件
            m_EventGroup.Register(LogicEvent.LoginSuccess, OnLogicSuccess);
            
            await UIManager.Ins.OpenUI<UILoading>();
            UIManager.Ins.CloseUI<UILogin>();
            //加载配置表
            await ConfigManager.Ins.LoadAllConfigs();
            
            //加载本地数据
            LocalSaveManager.Ins.Load();
            
            //服务器登录
            NetworkManager.Ins.SendMsg(new C2S_Login());
            
            await UniTask.Delay(1000);
        }

        private void OnLogicSuccess(int arg1, object arg2)
        {
            GameSM.Ins.ToMain();
        }
    }
}