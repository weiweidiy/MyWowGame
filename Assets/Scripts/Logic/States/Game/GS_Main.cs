using Cysharp.Threading.Tasks;
using DummyServer;
using Framework.Extension;
using Framework.GameFSM;
using Framework.UI;
using Logic.Data;
using Logic.Fight;
using Logic.Manager;
using Logic.UI.Common;
using Logic.UI.UIFight;
using Logic.UI.UIMain;
using Main;
using YooAsset;

namespace Logic.States.Game
{
    /// <summary>
    /// 主场景
    /// </summary>
    public class GS_Main : IState<GameState, GameStateData>
    {
        /// <summary>
        /// 加载是否已经完成
        /// </summary>
        public static float LoadProcess = 0.0f;

        public GS_Main(GameState pType) : base(pType)
        {
        }

        /// <summary>
        /// 最开始进入游戏
        /// </summary>
        /// <param name="pContext">游戏状态数据</param>
        public override async void Enter(GameStateData pContext)
        {
            LoadProcess = 0f;
            await UniTask.NextFrame();

            LoadProcess = 0.7f;
            //打开主场景
            await YooAssets.LoadSceneAsync("MainScene").ToUniTask();

            //卸载
            GameMain.Ins.UnloadUnusedAssets();

            //打开主场景相关的UI 包括一些通用UI
            await UIManager.Ins.OpenUI<UICommonSprites>();
            await UIManager.Ins.OpenUI<UITopInfo>();
            await UIManager.Ins.OpenUI<UIBottomMenu>(); //BottomMenu执行顺序需要再UIRoom前
            await UIManager.Ins.OpenUI<UIRoom>();
            await UIManager.Ins.OpenUI<UIFight>();
            await UIManager.Ins.OpenUI<UICommonTips>();
            await UIManager.Ins.OpenUI<UIMainRight>();
            await UIManager.Ins.OpenUI<UIMainLeft>();
            await UIManager.Ins.OpenUI<UIMainCommon>();

            //初始化某些游戏逻辑
            SingletonCreator.CreateSingleton<SyncUserDataManager>();

            //
            // await UIManager.Instance.OpenUI<UICommonSprites>();
            // await UIManager.Instance.OpenUI<UICommonTips>();
            // await UIManager.Instance.OpenUI<UIMainTop>();
            // await UIManager.Instance.OpenUI<UIMain>();
            // await UIManager.Instance.OpenUI<UIBottomMenu>();
            //
            // await UniTask.NextFrame();
            // await UniTask.NextFrame();
            // await UniTask.NextFrame();
            //


            FightManager.Ins.m_IsInit = true; //FightManager挂在场景中 需要UI都打开之后 然后进行后续的逻辑
            LoadProcess = 1f;
        }

        public override void Update(GameStateData pContext)
        {
        }

        public override void Release(GameStateData pContext)
        {
        }
    }
}