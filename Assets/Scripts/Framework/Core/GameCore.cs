
using Framework.Extension;
using Logic.States.Game;
using Sirenix.OdinInspector;
using UnityEngine.Rendering;

namespace Framework.Core
{
    /*
     * 游戏核心类, 游戏入口
     *
     * 驱动游戏状态机运行
     */
    public class GameCore : PersistentMonoSingleton<GameCore>
    {
        [LabelText("是否是本地单机模式")]
        [InfoBox("\n 仅供本地测试使用, 服务器功能开发完成后会被废弃 \n")]
        public bool UseDummyServer = true;

        [ShowIf("@this.UseDummyServer == false")]
        public string ServerAddr = "ws://127.0.0.1:8848";
        
        private void Start()
        {
            //启动状态机
            var _StateData = new GameStateData();
            GameSM.Ins.Start(_StateData, _StateData.m_InitState);

            //OnDemandRendering.renderFrameInterval = 10;
        }

        private void Update()
        {
            GameSM.Ins.Update();
        }

        private void OnDestroy()
        {
            GameSM.Ins.Release();
        }
    }
}