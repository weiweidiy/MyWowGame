using Chronos;
using Cysharp.Threading.Tasks;
using Framework.Extension;
using Framework.Helper;
using Framework.Pool;
using Logic.Fight.Actor;
using Logic.Manager;
using Sirenix.OdinInspector;
using System;
using UnityEngine;

namespace Logic.Fight.Common
{
    public class FightObjectSpawnManager : MonoSingleton<FightObjectSpawnManager>
    {

    }

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
        private int iEnemySkyIndex = 0;


        /// <summary>
        /// 创建一个敌人
        /// </summary>
        /// <param name="pResName"></param>
        /// <returns></returns>
        public async UniTask<Enemy> SpawnEnemy(string pResName)
        {
            var _Enemy = await Spawn(pResName);

            SetEnemyPosition(_Enemy);

            return _Enemy;
        }

        public async UniTask<Enemy> SpawnEnemy(string pResName, int index)
        {
            var _Enemy = await Spawn(pResName);

            SetEnemyPosition(_Enemy, index);

            return _Enemy;
        }


        /// <summary>
        /// boss是指定位置刷新
        /// </summary>
        /// <param name="pResName"></param>
        /// <returns></returns>
        public async UniTask<Enemy> SpawnEnemyBoss(string pResName)
        {
            var _Enemy = await Spawn(pResName);

            var index = 0;
            switch (_Enemy.GetPositionType())
            {
                case Enemy.PositionType.Ground:
                    index = m_EnemyGroundSpawnPoss.Length - 1;
                    break;
                case Enemy.PositionType.Sky:
                    index = m_EnemySkySpawnPoss.Length - 1;
                    break;
            }

            SetEnemyPosition(_Enemy,index);
            return _Enemy;
        }


        async UniTask<Enemy> Spawn(string pResName)
        {
            var _GO = await FightAssetsPool.Ins.Spawn(pResName);
            var _Enemy = _GO.GetComponent<Enemy>();
            if (_Enemy == null)
                throw new System.Exception("Enemy is null! ResName: " + pResName);

            return _Enemy;
        }

        void SetEnemyPosition(Enemy enemy)
        {
            var posType = enemy.GetPositionType();
            SetEnemyPosition(enemy, GetCurrentPositionIndex(posType));
        }

        /// <summary>
        /// 设置敌人坐标在指定索引上
        /// </summary>
        /// <param name="enemy"></param>
        /// <param name="index"></param>
        void SetEnemyPosition(Enemy enemy, int index)
        {
            Vector3 _Pos = GetPosition(enemy.GetPositionType(), index).position;
            _Pos.y += RandomHelper.Range(-0.2f, 0.2f);
            enemy.transform.position = _Pos;
        }


        /// <summary>
        /// 获取当前的坐标索引
        /// </summary>
        /// <param name="posType"></param>
        /// <returns></returns>
        int GetCurrentPositionIndex(Enemy.PositionType posType)
        {
            switch(posType)
            {
                case Enemy.PositionType.Ground:
                    {
                        var maxCount = GetPosition(posType).Length;
                        int index = iEnemyGroundIndex++;
                        if (index >= maxCount)
                        {
                            iEnemyGroundIndex = 0;
                            index = 0;
                        }
                        return index;
                    }
                    
                case Enemy.PositionType.Sky:
                    {
                        var maxCount = GetPosition(posType).Length;
                        int index = iEnemyGroundIndex++;
                        if (index >= maxCount)
                        {
                            iEnemySkyIndex = 0;
                            index = 0;
                        }
                        return index;
                    }
                    
                default:
                    Debug.LogError("未实现的坐标类型 " + posType);
                    break;
            }

            return -1;
        }






        Transform GetPosition(Enemy.PositionType posType, int spawnPositionIndex)
        {
            try
            {
                var tran = GetPosition(posType)[spawnPositionIndex];
            }
            catch(Exception e)
            {
                Debug.LogError("索引越界 " + spawnPositionIndex);
                throw e;
            }
            return GetPosition(posType)[spawnPositionIndex];
        }

        Transform[] GetPosition(Enemy.PositionType posType)
        {
            Transform[] transforms = null;
            switch (posType)
            {
                case Enemy.PositionType.Ground:
                    transforms = m_EnemyGroundSpawnPoss;
                    break;
                case Enemy.PositionType.Sky:
                    transforms = m_EnemySkySpawnPoss;
                    break;
                default:
                    Debug.LogError("未来实现的怪物类型 " + posType);
                    break;
            }

            return transforms;
        }


        public void RecycleEnemy(Enemy pEnemy)
        {
            FightAssetsPool.Ins.Recycle(pEnemy.gameObject);
        }
    }
}