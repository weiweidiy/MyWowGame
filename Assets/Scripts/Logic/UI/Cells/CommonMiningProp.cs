using DG.Tweening;
using Framework.Extension;
using Logic.Common;
using Logic.Manager;
using Logic.UI.Common;
using UnityEngine;
using UnityEngine.UI;

namespace Logic.UI.Cells
{
    public class CommonMiningProp : MonoBehaviour
    {
        public Sprite m_MiningProp;
        public Image m_Image;

        public MiningType m_TreasureType;

        public int m_ID;

        private int m_Count;

        private Vector3 m_OriginPosition;
        private bool m_IsOpen;

        private void Awake()
        {
            m_Image = this.GetComponent<Image>();
            m_OriginPosition = this.transform.position;
        }

        public void Init(int miningPropType, int id)
        {
            m_TreasureType = (MiningType)miningPropType;
            m_ID = id;
            m_IsOpen = false;
            m_Image.DOFade(1f, 0.1f);

            if (m_TreasureType == MiningType.Door)
            {
                m_Image.sprite = m_MiningProp;
            }
            else
            {
                m_TreasureType = (MiningType)miningPropType;
                m_Image.DOFade(0.25f, 0.1f);
                UICommonHelper.LoadMiningType(m_Image, m_TreasureType);
            }

            transform.localScale = new Vector3(1, 1, 1);
            m_Image.SetNativeSize();
            transform.localPosition = m_OriginPosition;
            this.Show();
        }

        public void OnPropMatch()
        {
            if (m_IsOpen)
            {
                return;
            }

            m_IsOpen = true;

            switch (m_TreasureType)
            {
                case MiningType.Hammer:
                case MiningType.Bomb:
                case MiningType.Scope:
                    OnSingleReward(m_TreasureType);
                    break;
                case MiningType.CopperMine:
                case MiningType.SilverMine:
                case MiningType.GoldMine:
                case MiningType.WeaponTreasure:
                case MiningType.ArmorTreasure:
                case MiningType.EquipTreasure:
                case MiningType.Coin:
                case MiningType.BigCoin:
                case MiningType.Diamond:
                case MiningType.Honor:
                case MiningType.Gear:
                    MiningManager.Ins.m_PropCountDictionary[(int)m_TreasureType]--;
                    if (MiningManager.Ins.m_PropCountDictionary[(int)m_TreasureType] <= 0)
                    {
                        MiningManager.Ins.GetThreeMatchReward(m_TreasureType);
                    }

                    break;
            }
        }

        public void OnSingleReward(MiningType treasureType)
        {
            MiningManager.Ins.m_PropDoTweenCount++;
            m_Image.DOFade(1f, 0.5f);
            DOTween.Sequence()
                .Append(transform.DOPunchScale(new Vector3(1.2f, 1.2f, 1f), 0.5f, 1))
                .AppendCallback(() =>
                {
                    switch (treasureType)
                    {
                        case MiningType.Hammer:
                        case MiningType.Bomb:
                        case MiningType.Scope:
                            SendMsgC2SMiningReward(treasureType);
                            break;
                    }

                    this.Hide();
                });
        }

        public void OnThreeMatchReward(Vector3 targetMove, int sendID)
        {
            DOTween.Sequence()
                .Append(transform.DOMove(targetMove, 0.5f))
                .Append(m_Image.DOFade(1f, 0.5f))
                .Append(transform.DOScale(new Vector3(1.2f, 1.2f, 1), 0.5f))
                .Append(transform.DOScale(new Vector3(0, 0, 1), 0.5f))
                .AppendCallback(() =>
                {
                    if (sendID == m_ID)
                    {
                        SendMsgC2SMiningReward(m_TreasureType);
                    }

                    this.Hide();
                });
        }

        private void SendMsgC2SMiningReward(MiningType treasureType)
        {
            RewardManager.Ins.SendMsgC2SMiningReward((int)treasureType);
        }
    }
}