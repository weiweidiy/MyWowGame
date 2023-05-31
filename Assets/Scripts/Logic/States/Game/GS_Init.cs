using Framework.GameFSM;
using Framework.UI;
using Logic.Data;

namespace Logic.States.Game
{
    /// <summary>
    /// 游戏初始化
    /// </summary>
    public class GS_Init : IState<GameState, GameStateData>
    {
        public GS_Init(GameState pType) : base(pType)
        {
        }

        public override void Enter(GameStateData pContext)
        {
            var _ = UIManager.Ins;
            //加载本地数据
            LocalSaveManager.Ins.Load();
            GameSM.Ins.ToLogin();
        }
        
        public override void Update(GameStateData pContext)
        {
        }

        public override void Release(GameStateData pContext)
        {
        }
    }
}