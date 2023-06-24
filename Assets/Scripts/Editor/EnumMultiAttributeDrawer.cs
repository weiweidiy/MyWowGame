using Logic.Fight.Common;
using UnityEditor;
using UnityEngine;
/// <summary>
/// 绘制多选属性
/// </summary>
[CustomPropertyDrawer(typeof(EnumMultiAttribute))]
public class EnumMultiAttributeDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        property.intValue = EditorGUI.MaskField(position, label, property.intValue, property.enumNames);
    }
}
