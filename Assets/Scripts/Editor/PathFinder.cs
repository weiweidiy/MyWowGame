using System.Text;
using UnityEditor;
using UnityEngine;

public class PathFinder
{
    [MenuItem("GameObject/Find Absolute Path", false, 0)]
    public static void FindGameObjectPath()
    {
        GameObject selectedObject = Selection.activeGameObject;
        if (selectedObject != null)
        {
            StringBuilder sb = new StringBuilder();
            Transform tr = selectedObject.transform;
            while (tr != null)
            {
                sb.Insert(0, tr.name);
                if (tr.parent != null)
                {
                    sb.Insert(0, "/");
                }

                tr = tr.parent;
            }

            Debug.Log($"{sb}\nAbsolute Path Of {selectedObject.name}");
        }
    }
}