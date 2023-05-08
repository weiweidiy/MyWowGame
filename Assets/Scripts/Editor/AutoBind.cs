using System.Reflection;
using Framework.Extension;
using Framework.UI;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities.Editor;
using UnityEditor;
using UnityEngine;

/// <summary>
/// 自动绑定元素到脚本里的变量, GO的名字必须与变量名及类型一致
/// </summary>

[CustomEditor(typeof(UIPage), true)]
public class AutoBind : OdinEditor
{
    public static void UIAutoBind()
    {
        if (Selection.gameObjects.Length == 0)
            return;
        var _Selected = Selection.gameObjects[0];
        var _T = _Selected.GetComponent<UIPage>();
        if (_T == null)
            return;
        var _Infos = _T.GetType().GetFields(BindingFlags.Instance | BindingFlags.Public);
        foreach (var _FieldInfo in _Infos)
        {
            string _AttrName = _FieldInfo.Name;
            if (_FieldInfo.FieldType.Name == "GameObject")
            {
                var _Target = _Selected.transform.FindChildRecursion(_AttrName);
                if(_Target == null)
                    continue;
                
                _FieldInfo.SetValue(_T, _Target.gameObject);
            }
            else
            {
                var _Target = _Selected.transform.FindChildRecursion(t =>
                {
                    if(t.gameObject.name == _AttrName && t.gameObject.GetComponent(_FieldInfo.FieldType) != null)
                        return true;
                    return false;
                });
                
                if(_Target == null)
                    continue;
                
                var _Type = _Target.gameObject.GetComponent(_FieldInfo.FieldType);
                _FieldInfo.SetValue(_T, _Type);
            }
        }
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        
        if (GUILayout.Button("AutoBind"))
        {
            UIAutoBind();
        }
    }
}

public class AutoBindGameObject
{
    [MenuItem("GameObject/AutoBind", false, 0)]
    public static void AutoBindGameObj()
    {
        if (Selection.gameObjects.Length == 0)
            return;
        var _Selecteds = Selection.gameObjects;
        foreach (var _Selected in _Selecteds)
        {
            var _Ts = _Selected.GetComponents<MonoBehaviour>();
            foreach (var _T in _Ts)
            {
                var _Infos = _T.GetType().GetFields(BindingFlags.Instance | BindingFlags.Public);
                foreach (var _FieldInfo in _Infos)
                {
                    string _AttrName = _FieldInfo.Name;
                    if (_FieldInfo.FieldType.Name == "GameObject")
                    {
                        var _Target = _Selected.transform.FindChildRecursion(_AttrName);
                        if(_Target == null)
                            continue;
                
                        _FieldInfo.SetValue(_T, _Target.gameObject);
                    }
                    else
                    {
                        var _Target = _Selected.transform.FindChildRecursion(t =>
                        {
                            if(t.gameObject.name == _AttrName && t.gameObject.GetComponent(_FieldInfo.FieldType) != null)
                                return true;
                            return false;
                        });
                
                        if(_Target == null)
                            continue;
                
                        var _Type = _Target.gameObject.GetComponent(_FieldInfo.FieldType);
                        _FieldInfo.SetValue(_T, _Type);
                    }
                }   
            }
        }
    }
}
