﻿
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
using Framework.EventKit;

namespace Logic.Fight.Common
{
    public class EnumMultiAttribute : PropertyAttribute
    {

    }

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
        //public Enemy.EnemyType EnemeyType; //怪物类型
        //public Enemy.PositionType PositionType; //位置类型
        public int SpawnIndex; //刷新点索引
        public float MoveSpeed;
        public int SpawnInterval; //刷新间隔
        public float AttackSpeed; //攻击速度


        public SpawnData(){}

        public SpawnData(string pEnemyRes, BigDouble pATK, BigDouble pHP, BigDouble pDrop,int pSpawnIndex,float pMoveSpeed,int pSpawnInterval,float pAttackSpeed)
        {
            EnemyRes = pEnemyRes;
            CurrentEnemyATK = pATK;
            CurrentEnemyHP = pHP;
            CurrentEnemyDrop = pDrop;
            //EnemeyType = pEenemyType;
            //PositionType = positionType;
            SpawnIndex = pSpawnIndex;
            MoveSpeed = pMoveSpeed;
            SpawnInterval = pSpawnInterval;
            AttackSpeed = pAttackSpeed;
        }
    }

    public struct WaveEnemyInfo
    {
        public int curEnemyCount;
        public int waveEnemeyCount;
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
        private int m_SpawnIntervalCopyTrophy; //战利品副本
        private int m_SpawnIntervalCopyReform; //战利品副本

        private void Start()
        {
            m_SpawnInterval = 100;// GameDefine.NormalSpawnInterval;
            m_SpawnIntervalCopyDiamond = GameDefine.CopyDiamondSpawnInterval;
            m_SpawnIntervalHangUp = GameDefine.HangUpSpawnInterval;
            m_SpawnIntervalCopyTrophy = GameDefine.CopyTrophySpawnInterval;
            m_SpawnIntervalCopyReform = GameDefine.CopyReformSpawnInterval;
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
            ProcessLevelSpawn(false);
            DoSpawnEnemy();
        }
        
        /// <summary>
        /// 挂机状态的刷怪逻辑 开始刷怪
        /// </summary>
        public void StartSpawnHandUp()
        {
            m_IsSpawningEnemy = true;
            _CurrentMoveSpeedMult = 1f;
            LevelSpawnHangUp();
            DoSpawnEnemy();
        }
        
        /// <summary>
        /// 金币副本BOSS
        /// </summary>
        public void StartSpawnCoinCopyBoss()
        {
            m_IsSpawningEnemy = true;
            _CurrentMoveSpeedMult = 1f;
            var _BossRes = ResCfgEx.GetResGroup(CopyManager.Ins.GetCopyCoinBossID())[0];//  .(Formula.GetLevelResGroupID());
            m_EnemySpawnQueue.Enqueue(new SpawnData(_BossRes, 0, CopyManager.Ins.GetCopyCoinBossHp(), 0,1,1, m_SpawnInterval,1));
            
            DoSpawnEnemy();
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
                m_EnemySpawnQueue.Enqueue(new SpawnData(_EList.Next(), _ATK, _HP, 0,1,1, m_SpawnIntervalCopyDiamond, 1));    
            }
            DoSpawnEnemy();
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
                m_EnemySpawnQueue.Enqueue(new SpawnData(_EList.Next(), _ATK, _HP, 0,1,1, m_SpawnIntervalCopyDiamond, 1));
            }

            DoSpawnEnemy();
        }

        /// <summary>
        /// 开始刷新战利品怪物
        /// </summary>
        public void StartSpawnTrophyCopyEnemy()
        {
            m_IsSpawningEnemy = true;
            _CurrentMoveSpeedMult = GameDefine.CopyTrophyMoveSpeedMult;
            var _Count = CopyManager.Ins.GetCopyTrophyBossCount();

            var _ATK = (CopyManager.Ins.GetCopyTrophyBossATK() / _Count).Ceiling();
            var _HP = (CopyManager.Ins.GetCopyTrophyBossHp() / _Count).Ceiling();

            var _EList = ResCfgEx.GetResGroup(CopyManager.Ins.GetCopyTrophyBossID());

            for (int i = 0; i < _Count; i++)
            {
                m_EnemySpawnQueue.Enqueue(new SpawnData(_EList.Next(), _ATK, _HP, 0,1,1, m_SpawnIntervalCopyTrophy, 1));
            }

            DoSpawnEnemy();
        }

        /// <summary>
        /// 开始刷新战利品怪物
        /// </summary>
        public void StartSpawnReformCopyEnemy()
        {
            m_IsSpawningEnemy = true;
            _CurrentMoveSpeedMult = GameDefine.CopyReformMoveSpeedMult;
            var _Count = CopyManager.Ins.GetCopyReformBossCount();

            var _ATK = (CopyManager.Ins.GetCopyRefromBossATK() / _Count).Ceiling();
            var _HP = (CopyManager.Ins.GetCopyRefromBossHp() / _Count).Ceiling();

            var _EList = ResCfgEx.GetResGroup(CopyManager.Ins.GetCopyTrophyBossID());

            for (int i = 0; i < _Count; i++)
            {
                m_EnemySpawnQueue.Enqueue(new SpawnData(_EList.Next(), _ATK, _HP, 0, 1,1, m_SpawnIntervalCopyReform, 1));
            }

            DoSpawnEnemy();
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

            WaveEnemyInfo enemyInfo = new WaveEnemyInfo();
            enemyInfo.curEnemyCount = m_EnemyDic.Values.Count;
            enemyInfo.waveEnemeyCount = m_EnemySpawnQueue.Count;
            EventManager.Call(LogicEvent.Fight_EnemyCountChanged, enemyInfo);
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
        /// 获取一个最近的目标
        /// </summary>
        public Enemy GetOneTarget(Enemy.PositionType posType)
        {
            float x = 10000;
            Enemy _Enemy = null;
            foreach (var enemy in m_EnemyDic.Values)
            {
                if (enemy.transform.position.x < x && IsSelectEnumType(enemy.positionType ,posType))
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

        public Enemy GetOneTarget(float pBeginAtkPosX, Enemy.PositionType posType)
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

            if (_Enemy != null && _Enemy.transform.position.x > pBeginAtkPosX && IsSelectEnumType(_Enemy.positionType, posType))
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

        public void GetTargetByDistance(List<Enemy> pList, EnemyRange pRange, Enemy.PositionType posType)
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
                if (enemy.transform.position.x > pMinX && enemy.transform.position.x < pMaxX && IsSelectEnumType(enemy.positionType, posType))
                {
                    pList.Add(enemy);
                }
            }
        }

        public bool IsSelectEnumType(Enemy.PositionType enemyType, Enemy.PositionType skillType)
        {
            if ((enemyType & skillType) == enemyType)
                return true;
            return false;
            //int index = 1 << (int)enemyType;
            //int eventTypeResult = (int)skillType;
            //if ((eventTypeResult & index) == index)
            //{
            //    return true;
            //}
            //return false;
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

        /// <summary>
        /// 获取范围内指定类型的敌人
        /// </summary>
        /// <param name="pList"></param>
        /// <param name="pMinX"></param>
        /// <param name="pMaxX"></param>
        /// <param name="posType"></param>
        public void GetTargetByRange(List<Enemy> pList, float pMinX, float pMaxX, Enemy.PositionType posType)
        {
            pList.Clear();
            foreach (var enemy in m_EnemyDic.Values)
            {
                if (enemy.transform.position.x > pMinX && enemy.transform.position.x < pMaxX && IsSelectEnumType(enemy.positionType, posType))
                {
                    pList.Add(enemy);
                }
            }
        }

        #endregion

        
        private async void DoSpawnEnemy()
        {
            WaveEnemyInfo enemyInfo = new WaveEnemyInfo();
            enemyInfo.curEnemyCount = m_EnemySpawnQueue.Count;
            enemyInfo.waveEnemeyCount = m_EnemySpawnQueue.Count;
            EventManager.Call(LogicEvent.Fight_EnemyCountChanged, enemyInfo);

            while (m_EnemySpawnQueue.Count > 0)
            {
                var _EnemyData = m_EnemySpawnQueue.Dequeue();
                var _EnemyInsID = ++m_EnemyInsIDIndex;

                //延迟
                await UniTask.Delay(_EnemyData.SpawnInterval);

                if (!m_IsSpawningEnemy)
                    return;

                Enemy _Enemy = await FightEnemySpawnManager.Ins.SpawnEnemy(_EnemyData.EnemyRes, _EnemyData.SpawnIndex);
                _Enemy.Init(_EnemyInsID, _EnemyData.CurrentEnemyHP, _EnemyData.CurrentEnemyATK, _EnemyData.CurrentEnemyDrop, _EnemyData.AttackSpeed, _EnemyData.MoveSpeed);
                
                m_EnemyDic.Add(_EnemyInsID, _Enemy);
            }

            m_IsSpawningEnemy = false;
        }

        #region 普通副本刷怪

        private void ProcessLevelSpawn(bool isHandUp)
        {

            var levelCfg = LevelCfg.GetLevelData(GameDataManager.Ins.CurLevelID);
            var nodeLevel = GameDataManager.Ins.CurLevelNode;
            var formationGroupId =isHandUp != true ? levelCfg.FormationGroupID[nodeLevel - 1] : levelCfg.HangupFormationGroup;
            var formationGroupCfg = FormationGroupCfg.GetData(formationGroupId);
            var formations = formationGroupCfg.FormationID;
            var randomIndex = UnityEngine.Random.Range(0, formations.Count);
            var formationId = formations[randomIndex];
            var formationCfg = FormationCfg.GetData(formationId);
            var enemyList = formationCfg.MonsterList;
            var atkPower = isHandUp != true? levelCfg.AtkPower : levelCfg.HangupAtk;
            var hpPower = isHandUp != true ? levelCfg.HPPower : levelCfg.HangupHP;
            var waveAtk = isHandUp != true ? levelCfg.WaveAtk[nodeLevel-1] : 1f;
            var waveHp= isHandUp != true ? levelCfg.WaveHP[nodeLevel - 1] : 1f;
            var waveDrop = isHandUp != true ? levelCfg.WaveDrop[nodeLevel - 1] : 1f;
            BigDouble drop = levelCfg.DropBase * MathF.Pow(10, isHandUp != true ? levelCfg.DropPower : levelCfg.HangupDrop);
            drop *= waveDrop;

            for(int i = 0; i < enemyList.Count; i++)
            {
                var enemyId = enemyList[i];
                var enemyDrop = formationCfg.Drop[i];
                var finalDrop = drop * enemyDrop;


                var spawnInterval = formationCfg.RefreshTime[i];
                var speed = formationCfg.Speed[i];
                var point = formationCfg.RefreshPoint[i]-1;
                if (point >= 10)
                    point -= 10;

                BigDouble atk =  GetNormalEnemeyAtk(enemyId, atkPower) * waveAtk;
                BigDouble hp = GetNormalEnemyHp(enemyId, hpPower) * waveHp;
                float atkSpeed = GetEnemyAttackSpeed(enemyId);
                var enemyRes = GetNormalEnemyRes(enemyId);
                m_EnemySpawnQueue.Enqueue(new SpawnData(enemyRes, atk, hp, finalDrop, point, speed, spawnInterval, atkSpeed));
            }



            BigDouble GetNormalEnemeyAtk(int enemyId, float power)
            {
                var enemyCfg = MonsterCfg.GetData(enemyId);
                return enemyCfg.AtkWight * Mathf.Pow(10, power); 
            }

            BigDouble GetNormalEnemyHp(int enemyId, float power)
            {
                var enemyCfg = MonsterCfg.GetData(enemyId);
                return enemyCfg.HPWight * Mathf.Pow(10, power);
            }

            string GetNormalEnemyRes(int enemyId)
            {
                var enemyCfg = MonsterCfg.GetData(enemyId);
                var resList = ResCfgEx.GetResGroup(enemyCfg.ResGroupID);
                return resList[UnityEngine.Random.Range(0, resList.Count)];
            }

            float GetEnemyAttackSpeed(int enemyId)
            {
                var enemyCfg = MonsterCfg.GetData(enemyId);
                return enemyCfg.AtkSpeed;
            }
        }

    



        ////普通小怪
        //private void LevelSpawnNormal(LevelData pLevelData)
        //{
        //    //BigDouble _Count = RandomHelper.Range(pLevelData.MinCount, pLevelData.MaxCount);
            
        //    //var _ATK = (pLevelData.AtkWight[0] * 0.01 * Formula.GetLevelAtk() / _Count).Ceiling();
        //    //var _HP = (pLevelData.HPWight[0] * 0.01 * Formula.GetLevelHP() / _Count).Ceiling();
        //    //var _Drop = (pLevelData.DropWight[0] * 0.01 * Formula.GetLevelDrop() / _Count).Ceiling();
            
        //    //var _EList = r(Formula.GetLevelNormalResGroupID());
        //    //for (int i = 0; i < _Count; i++)
        //    //{
        //    //    m_EnemySpawnQueue.Enqueue(new SpawnData(_EList.Next(), _ATK, _HP, _Drop, Enemy.EnemyType.Normal));    
        //    //}

        //}
        
        ////精英怪
        //private void LevelSpawnElite(LevelData pLevelData)
        //{
        //    //BigDouble _Count = RandomHelper.Range(pLevelData.EliteMinCount, pLevelData.EliteMaxCount);
            
        //    //var _ATK = (pLevelData.AtkWight[1] * 0.01 * Formula.GetLevelAtk() / _Count).Ceiling();
        //    //var _HP = (pLevelData.HPWight[1] * 0.01 * Formula.GetLevelHP() / _Count).Ceiling();
        //    //var _Drop = (pLevelData.DropWight[1] * 0.01 * Formula.GetLevelDrop() / _Count).Ceiling();
            
        //    //var _EList = ResCfgEx.GetResGroup(Formula.GetLevelEliteResGroupID());
        //    //for (int i = 0; i < _Count; i++)
        //    //{
        //    //    m_EnemySpawnQueue.Enqueue(new SpawnData(_EList.Next(), _ATK, _HP, _Drop, Enemy.EnemyType.Elite));    
        //    //}
        //}
        
        ////BOSS
        //private void LevelSpawnBOSS(LevelData pLevelData)
        //{
        //    //var _ATK = (pLevelData.AtkWight[2] * 0.01 * Formula.GetLevelAtk()).Ceiling();
        //    //var _HP = (pLevelData.HPWight[2] * 0.01 * Formula.GetLevelHP()).Ceiling();
        //    //var _Drop = (pLevelData.DropWight[2] * 0.01 * Formula.GetLevelDrop()).Ceiling();
    
        //    //var _BossRes = LevelResCfg.GetData((int)Formula.GetLevelResID()).BOSSRes;
        //    //m_EnemySpawnQueue.Enqueue(new SpawnData(_BossRes, _ATK, _HP, _Drop,Enemy.EnemyType.Boss));
        //}
        
        //挂机怪
        private void LevelSpawnHangUp()
        {
            ProcessLevelSpawn(true);
            //var _LevelCfg = LevelCfg.GetLevelData(GameDataManager.Ins.CurLevelID);
            //BigDouble _Count = _LevelCfg.HangUpCount;

            //var _ATK = (GameDefine.HangUpATKWight * Formula.GetLevelAtk() / _Count).Ceiling();
            //var _HP = (GameDefine.HangUpHPWight * Formula.GetLevelHP() / _Count).Ceiling();
            //var _Drop = (GameDefine.HangUpDropWight * Formula.GetLevelDrop() / _Count).Ceiling();

            //var _EList = ResCfgEx.GetResGroup(Formula.GetLevelNormalResGroupID());
            //for (int i = 0; i < _Count; i++)
            //{
            //    m_EnemySpawnQueue.Enqueue(new SpawnData(_EList.Next(), _ATK, _HP, _Drop));    
            //}
        }

        #endregion
    }
}