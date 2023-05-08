using System;
using System.Collections.Generic;
using System.Linq;
using Configs;
using Cysharp.Threading.Tasks;
using Framework.EventKit;
using Framework.Extension;
using Logic.Common;
using Logic.Fight.Skill;
using Logic.Manager;
using Networks;
using Sirenix.OdinInspector;
using UnityEngine;
using YooAsset;

namespace Logic.Fight.Common
{
    /// <summary>
    /// 战斗技能管理,管理技能的实例对象
    /// </summary>
    public class FightSkillManager : MonoSingleton<FightSkillManager>
    {
        [LabelText("技能实例列表")]
        List<SkillBase> m_CurSkillList = new List<SkillBase>();

        /// <summary>
        /// 资源handle，销毁时要release
        /// </summary>
        Dictionary<int, AssetOperationHandle> m_assetHandle = new Dictionary<int, AssetOperationHandle>();

        /// <summary>
        /// 挂载技能实例的父节点
        /// </summary>
        [SerializeField]
        Transform m_skillsParent;

        private readonly EventGroup m_EventGroup = new();
        protected override async void Awake()
        {
            base.Awake();
            
            m_EventGroup.Register(LogicEvent.SkillOn, OnSkillOn);
            m_EventGroup.Register(LogicEvent.SkillOff, OnSkillOff);
            
            for (int i = 0; i < SkillManager.Ins.SkillOnList.Count; i++)
            {
                if(SkillManager.Ins.SkillOnList[i] == 0)
                    continue;

                //创建技能实例
                var pSkillId = SkillManager.Ins.SkillOnList[i];
                if (!Existed(pSkillId))
                {
                    var _skillBase = await SpawnSkillObject(pSkillId);
                    _skillBase.Show();
                }
                          
                //ActiveSkill(SkillManager.Ins.SkillOnList[i] - 3001);
            }
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();

            foreach (var key in m_assetHandle.Keys)
            {
                m_assetHandle[key].Release();
            }
        }

        private bool Existed(int pSkillId)
        {
            return GetSkillObject(pSkillId) != null;        
        }

        /// <summary>
        /// 创建技能实例
        /// </summary>
        /// <param name="pSkillId"></param>
        /// <returns></returns>
        private async UniTask<SkillBase> SpawnSkillObject(int pSkillId)
        {
            var _skillData = SkillCfg.GetData(pSkillId);
            var _pResPath = _skillData.PrefabResName;

            AssetOperationHandle _assetHandle = null;
            if (m_assetHandle.ContainsKey(pSkillId))
            {
                _assetHandle = m_assetHandle[pSkillId];
            }            
            else
            {
                _assetHandle = YooAssets.LoadAssetAsync<GameObject>(_pResPath);
                m_assetHandle.Add(pSkillId, _assetHandle);
            }
               
            await _assetHandle.ToUniTask();
            var _goHandle = _assetHandle.InstantiateAsync(m_skillsParent);
            await _goHandle;
            var _go = _goHandle.Result;
            var _skillBase = _go.GetComponent<SkillBase>();

            m_CurSkillList.Add(_skillBase);

            return _skillBase;
        }

        
        private bool RemoveSkillObject(int pSkillId)
        {
            var _skillBase = GetSkillObject(pSkillId);
            if(m_CurSkillList.Remove(_skillBase))
            {
                GameObject.Destroy(_skillBase.gameObject);
                return true;
            }

            return false;
        }


        public void CastSkill(int pSkillID)
        {
            //var _Skill = m_CurSkillList[1];
            var _Skill = GetSkillObject(pSkillID);
            if(_Skill == null)
            {
                Debug.LogError("使用技能失败，因没有找到指定的技能对象：" + pSkillID);
                return;
            }
                
            _Skill.CastSkill();
        }
        

        public SkillBase GetSkillObject(int pSkillId)
        {
            return m_CurSkillList.Where(p => p.m_SkillId.Equals(pSkillId)).SingleOrDefault();
        }

        #region 消息处理

        private async void OnSkillOn(int arg1, object arg2)
        {
            var _Para = ((S2C_SkillOn)arg2);
            //var _SkillBase = m_CurSkillList[_Para.m_SkillID - 3001]; 
            //if (_SkillBase != null)
            //{
            //    _SkillBase.Show();
            //    _SkillBase.OnSkillOn();
            //}
            var pSkillId = _Para.m_SkillID;
            if (!Existed(pSkillId))
            {
                var _skillBase = await SpawnSkillObject(pSkillId);
                _skillBase.Show();
                _skillBase.OnSkillOn();
                
            }
            else
            {
                Debug.LogError("已经存在技能 " + pSkillId);
            }
        }

        private void OnSkillOff(int arg1, object arg2)
        {
            var _Para = ((S2C_SkillOff)arg2);
            //var _SkillBase = m_CurSkillList[_Para.m_SkillID - 3001]; 
            //if (_SkillBase != null)
            //{
            //_SkillBase.OnSkillReset();
            //    _SkillBase.Hide();
            //}
            var pSkillId = _Para.m_SkillID;
            if (Existed(pSkillId))
            {
                RemoveSkillObject(pSkillId);
            }
            else
            {
                Debug.LogError("删除技能失败，因不存在技能 " + pSkillId);
            }
                
        }

        #endregion


        [Obsolete("请使用GetSkillObject方法")]
        public SkillBase GetSkill(int pSkillID)
        {
            return m_CurSkillList[pSkillID - 3001];
        }

        [Obsolete("请使用CreateaSkillObject方法")]
        private void ActiveSkill(int index)
        {
            if (m_CurSkillList[index] != null)
                m_CurSkillList[index].Show();
        }


    }
}   
