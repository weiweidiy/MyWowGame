using Configs;
using Logic.Manager;
using TMPro;
using UnityEngine;

namespace Logic.UI.Cells
{
    public class CommonEngineAttr : MonoBehaviour
    {
        public TextMeshProUGUI attr1Name;
        public TextMeshProUGUI attr1Value;
        public TextMeshProUGUI attr2Name;
        public TextMeshProUGUI attr2Value;

        public void Init(int pID)
        {
            var engineData = EngineManager.Ins.GetGameEngineData(pID);
            var attribute1Data = AttributeCfg.GetData(engineData.Attr1ID);
            var attribute2Data = AttributeCfg.GetData(engineData.Attr2ID);
            attr1Name.text = attribute1Data.Name;
            attr1Value.text = $"{attribute1Data.Value}%";
            attr2Name.text = attribute2Data.Name;
            attr2Value.text = $"{attribute2Data.Value}%";
        }
    }
}