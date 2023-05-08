using Framework.Extension;
using UnityEngine;

namespace Framework.Helper
{
    public static class GameObjectHelper
    {
        //查找或者创建GameObject
        public static GameObject FindOrCreateGameObject(string name)
        {
            var go = GameObject.Find(name);
            if (go == null)
            {
                go = new GameObject(name);
            }
            return go;
        }

        //查找或者创建GameObject 并附加组件
        public static T FindOrCreateGameObjectComp<T>(string name) where T : Component
        {
            var comp = FindOrCreateGameObject(name).GetOrAddComponent<T>();
            return comp;
        }
    }
}