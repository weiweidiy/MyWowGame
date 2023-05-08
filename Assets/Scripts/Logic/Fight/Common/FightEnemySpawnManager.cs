using Cysharp.Threading.Tasks;
using Framework.Extension;
using Framework.Helper;
using Framework.Pool;
using Logic.Fight.Actor;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Logic.Fight.Common
{
    /// <summary>
    /// 战斗 怪物 / 召唤物 生成
    /// </summary>
    [DefaultExecutionOrder(-100)]
    public class FightEnemySpawnManager : MonoSingleton<FightEnemySpawnManager>
    {
        [Header("怪物刷新点")]
        [LabelText("地面刷新点")]
        public Transform[] m_EnemyGroundSpawnPoss;
        [LabelText("空中刷新点")]
        public Transform[] m_EnemySkySpawnPoss;

        private int iEnemyGroundIndex = 0;
        public async UniTask<Enemy> SpawnEnemy(string pResName)
        {
            //设定初始位置
            if(iEnemyGroundIndex >= m_EnemyGroundSpawnPoss.Length)
                iEnemyGroundIndex = 0;
            Vector3 _Pos = m_EnemyGroundSpawnPoss[iEnemyGroundIndex++].position;
            _Pos.y += RandomHelper.Range(-0.2f, 0.2f);

            var _GO = await FightAssetsPool.Ins.Spawn(pResName);
            var _Enemy = _GO.GetComponent<Enemy>();
            if(_Enemy == null)
                throw new System.Exception("Enemy is null! ResName: " + pResName);
            
            _Enemy.transform.position = _Pos;
            //_GO.ToLeft();

            return _Enemy;
        }
        
        public void RecycleEnemy(Enemy pEnemy)
        {
            FightAssetsPool.Ins.Recycle(pEnemy.gameObject);
        }
    }
}