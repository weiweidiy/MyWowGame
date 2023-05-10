
using System;
using System.Collections;
using System.Collections.Generic;
using BreakInfinity;
using Configs;
using Cysharp.Threading.Tasks;
using Framework.Extension;
using Framework.Helper;
using Logic.Common;
using Logic.Fight.Actor;
using Sirenix.OdinInspector;
using Logic.Data;
using Logic.Manager;
using UnityEngine;
using System.Linq;

namespace Logic.Fight.Common
{
    /// <summary>
    /// 攻击怪物的范围
    /// </summary>
    public enum EnemyRange
    {
        Near,
        Middle,
        Far,
    }
    
    class SpawnData
    {
        public string EnemyRes; //怪物资源名
        public BigDouble CurrentEnemyATK; //当前怪物攻击力
        public BigDouble CurrentEnemyHP; //当前怪物血量
        public BigDouble CurrentEnemyDrop; //当前怪物掉落金币
        
        public SpawnData(){}

        public SpawnData(string pEnemyRes, BigDouble pATK, BigDouble pHP, BigDouble pDrop)
        {
            EnemyRes = pEnemyRes;
            CurrentEnemyATK = pATK;
            CurrentEnemyHP = pHP;
            CurrentEnemyDrop = pDrop;
        }
    }
    
    /// <summary>
    /// 战场怪物管理
    /// 根据当前关卡或者副本刷新对应的怪物
    /// </summary>
    public class FightEnemyManager : MonoSingleton<FightEnemyManager>
    {
        [LabelText("近点X坐标阈值")]
        public float NearPosX = 0.5f;
        [LabelText("中点X坐标阈值")]
        public float MiddlePosX = 4f;
        [LabelText("挂机怪属性降低倍率")]
        public float HandUpReduce = 100f;
        
        //怪物实例ID索引
        private long m_EnemyInsIDIndex = 0;
        private Queue<SpawnData> m_EnemySpawnQueue = new (16);
        private bool m_IsSpawningEnemy = false;
        
        //当前关卡的怪物
        private readonly Dictionary<long, Enemy> m_EnemyDic = new (8);

        //怪物刷新间隔
        private int m_SpawnInterval; //一般刷新
        private int m_SpawnIntervalCopyDiamond; //元宝副本
        private int m_SpawnIntervalHangUp; //挂机
        private void Start()
        {
            m_SpawnInterval = GameDefine.NormalSpawnInterval;
            m_SpawnIntervalCopyDiamond = GameDefine.CopyDiamondSpawnInterval;
            m_SpawnIntervalHangUp = GameDefine.HangUpSpawnInterval;
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            m_EnemySpawnQueue.Clear();
        }

        #region 刷怪逻辑
        
        private float _CurrentMoveSpeedMult; //当前怪物移动速度倍率
        /// <summary>
        /// 战斗开始 - 刷新怪物 / 普通关卡
        /// </summary>
        public void StartLevelSpawn()
        {
            m_IsSpawningEnemy = true;
            _CurrentMoveSpeedMult = 1f;
            ProcessLevelSpawn();
            DoSpawnEnemy(m_SpawnInterval);
        }
        
        /// <summary>
        /// 挂机状态的刷怪逻辑 开始刷怪
        /// </summary>
        public void StartSpawnHandUp()
        {
            m_IsSpawningEnemy = true;
            _CurrentMoveSpeedMult = 1f;
            LevelSpawnHangUp();
            DoSpawnEnemy(m_SpawnIntervalHangUp);
        }
        
        /// <summary>
        /// 金币副本BOSS
        /// </summary>
        public void StartSpawnCoinCopyBoss()
        {
            m_IsSpawningEnemy = true;
            _CurrentMoveSpeedMult = 1f;
            var _BossRes = ResCfgEx.GetResGroup(CopyManager.Ins.GetCopyCoinBossID())[0];//  .(Formula.GetLevelResGroupID());
            m_EnemySpawnQueue.Enqueue(new SpawnData(_BossRes, 0, CopyManager.Ins.GetCopyCoinBossHp(), 0));
            
            DoSpawnEnemy(m_SpawnInterval);
        }
        
        /// <summary>
        /// 元宝副本BOSS
        /// </summary>
        public void StartSpawnDiamondCopyBoss()
        {
            m_IsSpawningEnemy = true;
            _CurrentMoveSpeedMult = GameDefine.CopyDiamondMoveSpeedMult;
            var _Count = CopyManager.Ins.GetCopyDiamondBossCount();
            
            var _ATK = (CopyManager.Ins.GetCopyDiamondBossATK() / _Count).Ceiling();
            var _HP = (CopyManager.Ins.GetCopyDiamondBossHp() / _Count).Ceiling();

            var _EList = ResCfgEx.GetResGroup(CopyManager.Ins.GetCopyDiamondBossID());
            
            for (int i = 0; i < _Count; i++)
            {
                m_EnemySpawnQueue.Enqueue(new SpawnData(_EList.Next(), _ATK, _HP, 0));    
            }
            
            DoSpawnEnemy(m_SpawnIntervalCopyDiamond);
        }

        /// <summary>
        /// 原油副本BOSS
        /// </summary>
        public void StartSpawnOilCopyBoss()
        {
            m_IsSpawningEnemy = true;
            _CurrentMoveSpeedMult = GameDefine.CopyDiamondMoveSpeedMult;
            var _Count = CopyManager.Ins.GetOilCopyBossCount();

            var _ATK = (CopyManager.Ins.GetCopyOilBossATK() / _Count).Ceiling();
            var _HP = (CopyManager.Ins.GetCopyOilBossHp() / _Count).Ceiling();

            var _EList = ResCfgEx.GetResGroup(CopyManager.Ins.GetCopyOilBossID());

            for (int i = 0; i < _Count; i++)
            {
                m_EnemySpawnQueue.Enqueue(new SpawnData(_EList.Next(), _ATK, _HP, 0));
            }

            DoSpawnEnemy(m_SpawnIntervalCopyDiamond);
        }

        
        #endregion
        
        /// <summary>
        /// 终止还未完成的刷怪逻辑
        /// 立即回收还没死亡的怪
        /// </summary>
        public void ClearBattleground()
        {
            m_EnemySpawnQueue.Clear();
            m_IsSpawningEnemy = false;
            foreach (var _Enemy in m_EnemyDic.Values)
            {
                _Enemy.ImmediatelyRecycle();
            }
            m_EnemyDic.Clear();
        }

        /// <summary>
        /// 是否已经干翻所有怪物了
        /// </summary>
        /// <returns>结果</returns>
        public bool IsClear()
        {
            if(m_IsSpawningEnemy)
                return false;
            
            return m_EnemyDic.Count <= 0;
        }
        
        /// <summary>
        /// 怪物死亡时 从列表中移除
        /// </summary>
        /// <param name="pEnemyInsID">怪物实例ID</param>
        public void RemoveEnemy(long pEnemyInsID)
        {
            m_EnemyDic.Remove(pEnemyInsID);
        }

        #region 获取目标接口
        
        /// <summary>
        /// 获取一个最近的目标
        /// </summary>
        public Enemy GetOneTarget()
        {
            float x = 10000;
            Enemy _Enemy = null;
            foreach (var enemy in m_EnemyDic.Values)
            {
                if (enemy.transform.position.x < x)
                {
                    _Enemy = enemy;
                    x = enemy.transform.position.x;
                }
            }

            return _Enemy;
        }
        
        /// <summary>
        /// 根据攻击起始位置 获取一个最近的目标
        /// 当怪物位置小于此位置 才会开始攻击
        /// </summary>
        public Enemy GetOneTarget(float pBeginAtkPosX)
        {
            float x = 10000;
            Enemy _Enemy = null;
            foreach (var enemy in m_EnemyDic.Values)
            {
                if (enemy.transform.position.x < x)
                {
                    _Enemy = enemy;
                    x = enemy.transform.position.x;
                }
            }

            if (_Enemy != null && _Enemy.transform.position.x > pBeginAtkPosX)
                return null;
            return _Enemy;
        }

        /// <summary>
        /// 获取指定数量随机敌人
        /// </summary>
        /// <param name="pList"></param>
        /// <param name="count"></param>
        public void GetRandomTargets(List<Enemy> pList, int count)
        {
            var _EnemyCount = m_EnemyDic.Keys.Count;
            if (_EnemyCount < count)
                return;

            var _Lst = m_EnemyDic.ToList();
            for(int i = 0; i < count; i ++)
            {
                var _RandomIndex = UnityEngine.Random.Range(0, _EnemyCount);
                Enemy _Enemy = _Lst.ElementAt(_RandomIndex).Value;
                pList.Add(_Enemy);
            }
        }
        
        /// <summary>
        /// 获取范围内的所有目标
        /// </summary>
        public void GetTargetByDistance(List<Enemy> pList, EnemyRange pRange)
        {
            pList.Clear();
            float pMinX = 0;
            float pMaxX = 0;

            switch (pRange)
            {
                case EnemyRange.Near:
                    pMinX = -100;
                    pMaxX = NearPosX;
                    break;
                case EnemyRange.Middle:
                    pMinX = NearPosX;
                    pMaxX = MiddlePosX;
                    break;
                case EnemyRange.Far:
                    pMinX = MiddlePosX;
                    pMaxX = 100;
                    break;
            }
            
            foreach (var enemy in m_EnemyDic.Values)
            {
                if (enemy.transform.position.x > pMinX && enemy.transform.position.x < pMaxX)
                {
                    pList.Add(enemy);
                }
            }
        }
        
        /// <summary>
        /// 获取范围内的所有目标
        /// </summary>
        public void GetTargetByRange(List<Enemy> pList, float pMinX, float pMaxX)
        {
            pList.Clear();
            foreach (var enemy in m_EnemyDic.Values)
            {
                if (enemy.transform.position.x > pMinX && enemy.transform.position.x < pMaxX)
                {
                    pList.Add(enemy);
                }
            }
        }
        
        #endregion

        private async void DoSpawnEnemy(int pInterval)
        {
            while (m_EnemySpawnQueue.Count > 0)
            {
                var _EnemyData = m_EnemySpawnQueue.Dequeue();
                var _EnemyInsID = ++m_EnemyInsIDIndex;
                var _Enemy = await FightEnemySpawnManager.Ins.SpawnEnemy(_EnemyData.EnemyRes);
                _Enemy.Init(_EnemyInsID, _EnemyData.CurrentEnemyHP, _EnemyData.CurrentEnemyATK, _EnemyData.CurrentEnemyDrop, _CurrentMoveSpeedMult);
                
                m_EnemyDic.Add(_EnemyInsID, _Enemy);

                await UniTask.Delay(pInterval);
            }

            m_IsSpawningEnemy = false;
        }

        #region 普通副本刷怪

        private void ProcessLevelSpawn()
        {
            var _LevelCfg = LevelCfg.GetLevelData(GameDataManager.Ins.CurLevelID);
            switch ((SpawnType)_LevelCfg.SpawnType[GameDataManager.Ins.CurLevelNode - 1])
            {
                case SpawnType.Normal:
                    LevelSpawnNormal(_LevelCfg);
                    break;
                case SpawnType.Elite:
                    LevelSpawnElite(_LevelCfg);
                    break;
                case SpawnType.Both:
                    LevelSpawnNormal(_LevelCfg);
                    LevelSpawnElite(_LevelCfg);
                    break;
                case SpawnType.Boss:
                    LevelSpawnBOSS(_LevelCfg);
                    break;
                case SpawnType.BossNormal:
                    LevelSpawnNormal(_LevelCfg);
                    LevelSpawnBOSS(_LevelCfg);
                    break;
                case SpawnType.All:
                    LevelSpawnNormal(_LevelCfg);
                    LevelSpawnElite(_LevelCfg);
                    LevelSpawnBOSS(_LevelCfg);
                    break;
            }
        }

        //普通小怪
        private void LevelSpawnNormal(LevelData pLevelData)
        {
            BigDouble _Count = RandomHelper.Range(pLevelData.MinCount, pLevelData.MaxCount);
            
            var _ATK = (pLevelData.AtkWight[0] * 0.01 * Formula.GetLevelAtk() / _Count).Ceiling();
            var _HP = (pLevelData.HPWight[0] * 0.01 * Formula.GetLevelHP() / _Count).Ceiling();
            var _Drop = (pLevelData.DropWight[0] * 0.01 * Formula.GetLevelDrop() / _Count).Ceiling();
            
            var _EList = ResCfgEx.GetResGroup(Formula.GetLevelNormalResGroupID());
            for (int i = 0; i < _Count; i++)
            {
                m_EnemySpawnQueue.Enqueue(new SpawnData(_EList.Next(), _ATK, _HP, _Drop));    
            }
        }
        
        //精英怪
        private void LevelSpawnElite(LevelData pLevelData)
        {
            BigDouble _Count = RandomHelper.Range(pLevelData.EliteMinCount, pLevelData.EliteMaxCount);
            
            var _ATK = (pLevelData.AtkWight[1] * 0.01 * Formula.GetLevelAtk() / _Count).Ceiling();
            var _HP = (pLevelData.HPWight[1] * 0.01 * Formula.GetLevelHP() / _Count).Ceiling();
            var _Drop = (pLevelData.DropWight[1] * 0.01 * Formula.GetLevelDrop() / _Count).Ceiling();
            
            var _EList = ResCfgEx.GetResGroup(Formula.GetLevelEliteResGroupID());
            for (int i = 0; i < _Count; i++)
            {
                m_EnemySpawnQueue.Enqueue(new SpawnData(_EList.Next(), _ATK, _HP, _Drop));    
            }
        }
        
        //BOSS
        private void LevelSpawnBOSS(LevelData pLevelData)
        {
            var _ATK = (pLevelData.AtkWight[2] * 0.01 * Formula.GetLevelAtk()).Ceiling();
            var _HP = (pLevelData.HPWight[2] * 0.01 * Formula.GetLevelHP()).Ceiling();
            var _Drop = (pLevelData.DropWight[2] * 0.01 * Formula.GetLevelDrop()).Ceiling();
    
            var _BossRes = LevelResCfg.GetData((int)Formula.GetLevelResID()).BOSSRes;
            m_EnemySpawnQueue.Enqueue(new SpawnData(_BossRes, _ATK, _HP, _Drop));
        }
        
        //挂机怪
        private void LevelSpawnHangUp()
        {
            var _LevelCfg = LevelCfg.GetLevelData(GameDataManager.Ins.CurLevelID);
            BigDouble _Count = _LevelCfg.HangUpCount;
            
            var _ATK = (GameDefine.HangUpATKWight * Formula.GetLevelAtk() / _Count).Ceiling();
            var _HP = (GameDefine.HangUpHPWight * Formula.GetLevelHP() / _Count).Ceiling();
            var _Drop = (GameDefine.HangUpDropWight * Formula.GetLevelDrop() / _Count).Ceiling();
            
            var _EList = ResCfgEx.GetResGroup(Formula.GetLevelNormalResGroupID());
            for (int i = 0; i < _Count; i++)
            {
                m_EnemySpawnQueue.Enqueue(new SpawnData(_EList.Next(), _ATK, _HP, _Drop));    
            }
        }

        #endregion
    }
}