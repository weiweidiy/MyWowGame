using BreakInfinity;
using Framework.Extension;
using Framework.Pool;
using Logic.Fight.Data;
using Logic.Fight.Effect;
using UnityEngine;

namespace Logic.Fight.Common
{
    /// <summary>
    /// 战斗飘血管理类
    /// </summary>
    public class FightDamageManager : MonoSingleton<FightDamageManager>
    {
        // [NonSerialized]
        // public Color FriendlyColor = new(214 / 255f, 112 / 255f, 10 / 255f);
        // [NonSerialized]
        public Color NormalDamageColor = Color.white;

        private Vector3 offset = new Vector3(-0.1f, -0.15f, 0);
        public void ShowDamage(Transform pTarget, FightDamageData pDamageData, bool pIsEnemy = false)
        {
            if (pDamageData.IsCritical)
            {
                ShowCriticalDamage(pTarget, pDamageData.Damage);
            }
            else
            {
                ShowNormalDamage(pTarget, pDamageData.Damage, pIsEnemy);
            }
        }

        private async void ShowNormalDamage(Transform pTarget, BigDouble pDamage, bool pIsEnemy = false)
        {
            var _Obj = await FightAssetsPool.Ins.Spawn("Damage_Normal");
            _Obj.transform.position = pTarget.position + offset;
            _Obj.GetComponent<ShowNormalDamage>().ShowText(pDamage, NormalDamageColor, pIsEnemy);
        }
        
        private async void ShowCriticalDamage(Transform pTarget, BigDouble pDamage)
        {
            var _Obj = await FightAssetsPool.Ins.Spawn("Damage_Critical");
            _Obj.transform.position = pTarget.position;
            _Obj.GetComponent<ShowCriticalDamage>().ShowText(pDamage);
        }
    }
}
