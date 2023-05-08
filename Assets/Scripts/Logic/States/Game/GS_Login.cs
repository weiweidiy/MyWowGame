using Framework.GameFSM;
using Framework.UI;
using Logic.UI.UILogin;

namespace Logic.States.Game
{
    /// <summary>
    /// 登录状态
    /// </summary>
    public class GS_Login : IState<GameState, GameStateData>
    {
        public GS_Login(GameState pType) : base(pType)
        {
        }

        public override async void Enter(GameStateData pContext)
        {
            await UIManager.Ins.OpenUI<UILogin>();
        }

        public override void Update(GameStateData pContext)
        {
        }

        public override void Release(GameStateData pContext)
        {
        }
    }
}