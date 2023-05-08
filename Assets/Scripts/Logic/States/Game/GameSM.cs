using Framework.Extension;
using Framework.GameFSM;

namespace Logic.States.Game
{
    //游戏状态机
    public class GameSM : FSMachine<IState<GameState, GameStateData>, GameState, GameStateData>, ISingleton
    {
        public static GameSM Ins => SingletonProperty<GameSM>.Instance;
        
        public override void Release()
        {
            base.Release();
            SingletonProperty<GameSM>.Dispose();
        }

        public void OnSingletonInit()
        {
            
        }

        #region 状态机切换

        public void ToLogin()
        {
            NextState(m_ContextData.m_LoginState);
        }
        
        public void ToLoading()
        {
            NextState(m_ContextData.m_LoadingState);
        }

        public void ToMain()
        {
            NextState(m_ContextData.m_MainState);
        }
        
        public void ToBreakLine()
        {
            NextState(m_ContextData.m_BreakLineState);
        }
        
        #endregion
    }
    
    //状态机上下文
    public class GameStateData
    {
        //状态
        public GS_Init m_InitState = new (GameState.Init);
        public GS_Login m_LoginState = new (GameState.Login);
        public GS_Loading m_LoadingState = new (GameState.Loading);
        public GS_Main m_MainState = new (GameState.Main);
        public GS_BreakLine m_BreakLineState = new(GameState.BreakLine);
    }

    //游戏全局状态机
    public enum GameState
    {
        Init,       //游戏初始化
        Login,      //登录UI / 登录中
        Loading,    //进入游戏
        Main,       //主场景(房间/挂机场景)
        BreakLine,  //断线重连
    }
}