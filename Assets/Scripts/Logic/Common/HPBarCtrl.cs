using System;
using UnityEngine;

namespace Logic.Common
{
    //战斗血条显示控制
    public class HPBarCtrl : MonoBehaviour
    {
        public SpriteRenderer m_HPValue;
        private Vector2 m_OSize;
        private void Awake()
        {
            var size = m_HPValue.size;
            m_OSize = new Vector2(size.x, size.y);
        }

        private void OnDisable()
        {
            m_HPValue.size = m_OSize;
        }
        
        public void SetHP(float pValue)
        {
            var size = m_HPValue.size;
            size.x = m_OSize.x * pValue;
            m_HPValue.size = size;
        }
    }
}
