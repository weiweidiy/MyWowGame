
using UnityEngine;

namespace Main.Other
{
    public class KeepGameObject : MonoBehaviour
    {
        void Awake()
        {
            DontDestroyOnLoad(gameObject);
        }
    }
}
