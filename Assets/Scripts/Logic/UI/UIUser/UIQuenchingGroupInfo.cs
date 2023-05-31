using System;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using Framework.Extension;
using Framework.UI;
using Logic.Common;
using Logic.Manager;
using TMPro;
using UnityEngine;

namespace Logic.UI.UIUser
{
    public class UIQuenchingGroupInfo : UIPage
    {
        public GameObject m_Bg;
        [Header("Do")] public GameObject m_DoCombo;
        public TextMeshProUGUI m_DoLevel;
        public TextMeshProUGUI m_DoAttr1, m_DoAttr2, m_DoAttr3;
        [Header("Re")] public GameObject m_ReCombo;
        public TextMeshProUGUI m_ReLevel;
        public TextMeshProUGUI m_ReAttr1, m_ReAttr2, m_ReAttr3;
        [Header("Mi")] public GameObject m_MiCombo;
        public TextMeshProUGUI m_MiLevel;
        public TextMeshProUGUI m_MiAttr1, m_MiAttr2, m_MiAttr3;
        [Header("Fa")] public GameObject m_FaCombo;
        public TextMeshProUGUI m_FaLevel;
        public TextMeshProUGUI m_FaAttr1, m_FaAttr2, m_FaAttr3;
        [Header("Sol")] public GameObject m_SolCombo;
        public TextMeshProUGUI m_SolLevel;
        public TextMeshProUGUI m_SolAttr1, m_SolAttr2, m_SolAttr3;

        private void Awake()
        {
            transform.localScale = Vector3.zero;
        }

        private void Start()
        {
            Init();
        }

        public override void OnShow()
        {
            base.OnShow();
            UpdateQuenchingGroup();
            transform.DOScale(new Vector3(1, 1, 1), 0.2f).OnComplete(() => { m_Bg.Show(); });
        }

        public void OnBtnCloseClick()
        {
            m_Bg.Hide();
            transform.DOScale(new Vector3(0, 0, 1), 0.2f).OnComplete(this.Hide);
        }

        private Tuple<int, string, float> GeMelodyInfo(int dId)
        {
            var djComboData = QuenchingManager.Ins.GetDJComboData(dId);
            var comboCount = djComboData.ComboCount;
            var attributeDes = djComboData.AttributeDes;
            var attributeValue = QuenchingManager.Ins.GetAttributeData(djComboData.AttributeID).Value;
            return new Tuple<int, string, float>(comboCount, attributeDes, attributeValue);
        }

        private void Init()
        {
            var infos = new Tuple<int, string, float>[]
            {
                GeMelodyInfo(11), GeMelodyInfo(12), GeMelodyInfo(13),
                GeMelodyInfo(21), GeMelodyInfo(22), GeMelodyInfo(23),
                GeMelodyInfo(31), GeMelodyInfo(32), GeMelodyInfo(33),
                GeMelodyInfo(41), GeMelodyInfo(42), GeMelodyInfo(43),
                GeMelodyInfo(51), GeMelodyInfo(52), GeMelodyInfo(53),
            };

            var attrs = new TextMeshProUGUI[]
            {
                m_DoAttr1, m_DoAttr2, m_DoAttr3,
                m_ReAttr1, m_ReAttr2, m_ReAttr3,
                m_MiAttr1, m_MiAttr2, m_MiAttr3,
                m_FaAttr1, m_FaAttr2, m_FaAttr3,
                m_SolAttr1, m_SolAttr2, m_SolAttr3,
            };

            var doInfoLength = infos.Length;
            for (var i = 0; i < doInfoLength; i++)
            {
                attrs[i].text = $"({infos[i].Item1}){string.Format(infos[i].Item2, infos[i].Item3)}";
            }
        }

        private void UpdateQuenchingGroup()
        {
            var quenchingMap = QuenchingManager.Ins.QuenchingMap;
            var quenchingLevels = new Dictionary<QuenchingType, int>
            {
                { QuenchingType.Do, 0 },
                { QuenchingType.Re, 0 },
                { QuenchingType.Mi, 0 },
                { QuenchingType.Fa, 0 },
                { QuenchingType.Sol, 0 },
            };

            foreach (var quenchingType in quenchingMap.Values.Select(quenchingData =>
                         (QuenchingType)quenchingData.MelodyId))
            {
                quenchingLevels[quenchingType]++;
            }

            var attrs = new[]
            {
                (m_DoAttr1, m_DoAttr2, m_DoAttr3, m_DoLevel, m_DoCombo, QuenchingType.Do),
                (m_ReAttr1, m_ReAttr2, m_ReAttr3, m_ReLevel, m_ReCombo, QuenchingType.Re),
                (m_MiAttr1, m_MiAttr2, m_MiAttr3, m_MiLevel, m_MiCombo, QuenchingType.Mi),
                (m_FaAttr1, m_FaAttr2, m_FaAttr3, m_FaLevel, m_FaCombo, QuenchingType.Fa),
                (m_SolAttr1, m_SolAttr2, m_SolAttr3, m_SolLevel, m_SolCombo, QuenchingType.Sol)
            };

            foreach (var (attr1, attr2, attr3, level, combo, quenchingType) in attrs)
            {
                var levelValue = quenchingLevels[quenchingType];
                switch (levelValue)
                {
                    case 5:
                        attr1.color = Color.white;
                        attr2.color = Color.white;
                        attr3.color = Color.yellow;
                        combo.Show();
                        level.text = "3级";
                        break;
                    case >= 3 and < 5:
                        attr1.color = Color.white;
                        attr2.color = Color.yellow;
                        attr3.color = Color.white;
                        combo.Show();
                        level.text = "2级";
                        break;
                    case >= 2 and < 3:
                        attr1.color = Color.yellow;
                        attr2.color = Color.white;
                        attr3.color = Color.white;
                        combo.Show();
                        level.text = "1级";
                        break;
                    default:
                        attr1.color = Color.white;
                        attr2.color = Color.white;
                        attr3.color = Color.white;
                        combo.Hide();
                        level.text = "0级";
                        break;
                }
            }
        }
    }
}