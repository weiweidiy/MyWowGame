
using System.Collections;
using System.Collections.Generic;
using BreakInfinity;
using DG.Tweening;
using Framework.Extension;
using Logic.Common;
using Logic.Fight.Actor;
using Logic.Fight.Common;
using Logic.Fight.Skill.Implement.SkillParts;
using UnityEngine;

namespace Logic.Fight.Skill.Implement
{
    public class Skill_5 : SkillBase
    {
        /// <summary>
        /// 技能实例对象模板
        /// </summary>
        public GameObject m_skillSubObjectTemplate;

        public float m_TargetPosX;
        public float m_AttackRange;

        [Tooltip("释放的个数")]
        public int m_subSkillObjectCount;
        [Tooltip("技能子物体发出间隔")]
        public Vector2 m_spawnInterval;

        [Min(1f)]
        public float m_moveSpeed;

        /// <summary>
        /// 用于创建技能body实例
        /// </summary>
        Coroutine m_coCreateSkillSubObject = null;

        /// <summary>
        /// 存放技能实例的列表
        /// </summary>
        List<GameObject> m_goSkillSubObjects = new List<GameObject>();

        public override void Init(int pSkillId)
        {
            base.Init(pSkillId);
        }
        
        public override void OnStartSkill()
        {
            base.OnStartSkill();
            m_coCreateSkillSubObject = StartCoroutine(CreateSkillSubObjects());
        }

        
        IEnumerator CreateSkillSubObjects()
        {
            for (int i = 0; i < m_subSkillObjectCount; i++)
            {
                var go = CreateSkillSubObject();
                m_goSkillSubObjects.Add(go);
                yield return new WaitForSeconds(Random.Range(m_spawnInterval.x, m_spawnInterval.y));
            }
        }

        GameObject CreateSkillSubObject()
        {
            var go = Instantiate(m_skillSubObjectTemplate, transform);
            go.SetActive(true);
            go.transform.position += new Vector3(0, Random.Range(-0.35f, 0.05f), 0);
            DOMove(go.GetComponent<SkillSubObject>());
            return go;
        }

        public override void OnStopSkill()
        {

            if (m_coCreateSkillSubObject != null)
                StopCoroutine(m_coCreateSkillSubObject);

            foreach (var go in m_goSkillSubObjects)
            {
                GameObject.Destroy(go);
            }
        }

        public override void OnSkillReset()
        {
            OnStopSkill();
            m_SM.ToIdle();
        }
        
        public override bool NeedSearchTarget()
        {
            return false;
        }
        
        //private List<Enemy> m_HitList = new List<Enemy>(6);
        public void DOMove(SkillSubObject skillSubObject)
        {

            var _hitList = new List<Enemy>(6);
            var _body = skillSubObject.m_body.gameObject;
            var _HitEffect = skillSubObject.m_hitEffect.GetComponent<Skill_3MissilesHit>();
            var _ObjTrans = skillSubObject.transform;
            var _Tweener = _ObjTrans.DOMoveX(m_TargetPosX, m_moveSpeed).SetSpeedBased();
            _Tweener.OnComplete(() =>
            {
                _body.SetActive(false);
            });
            _Tweener.OnUpdate(() =>
            {
                var _Enemy = FightEnemyManager.Ins.GetOneTarget();
                if (_Enemy != null && Mathf.Abs(_Enemy.transform.position.x - _ObjTrans.position.x) <= 0.1f)
                {
                    _body.SetActive(false);
                    _Tweener.Kill();

                    _HitEffect.Show();
                    _HitEffect.PlayAni(_Enemy.transform.position);

                    var position = _ObjTrans.position;
                    FightEnemyManager.Ins.GetTargetByRange(_hitList, position.x - m_AttackRange, 
                        position.x + m_AttackRange);
                    
                    BigDouble _ATK = Formula.GetGJJAtk() * GetSkillBaseDamage()/100;
                    foreach (var enemy in _hitList)
                    {
                        if(enemy != null && enemy.CanAttack())
                            enemy.m_Health.Damage(_ATK);
                    }
                }
            });
        }
    }
}