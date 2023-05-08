// using Framework.CameraCtrl;
// using UnityEditor;
// using UnityEngine;
//
// [CustomEditor(typeof(DragBounds))]
// public class DragBoundsInspector : Editor
// {
//     DragBounds cb;
//     private Rect boundArea;
//
//     private const float wpSize = 0.1f;
//
//     private void OnEnable() {
//         cb = (DragBounds)target;
//     }
//
//     public override void OnInspectorGUI() {
//         DrawDefaultInspector();
//     }
//
//     private void OnSceneGUI() {
//         float zoom = HandleUtility.GetHandleSize(new Vector3(0, 0, 0)); // basically gets a scene view zoom level
//
//         boundArea.x = cb.transform.position.x;
//         boundArea.y = cb.transform.position.y;
//         boundArea.width = cb.range.x - cb.transform.position.x;
//         boundArea.height = cb.range.y - cb.transform.position.y;
//
//         Handles.Label(cb.transform.position + new Vector3(0,-0.2f,0),"Bottom Left Boundary");
//         Handles.Label(cb.range + new Vector3(0, -0.2f, 0), "Top Right Boundary");
//
//         cb.range = Handles.Slider2D(cb.range, Vector3.forward, Vector3.right, Vector3.up, wpSize * zoom, Handles.CircleHandleCap, 0.1f);
//         cb.transform.position = Handles.Slider2D(cb.transform.position, Vector3.forward, Vector3.right, Vector3.up, wpSize * zoom, Handles.CircleHandleCap, 0.1f);
//
//         if (cb.range.x < cb.transform.position.x) {
//             cb.range.x = cb.transform.position.x;
//         }
//
//         if (cb.range.y < cb.transform.position.y) {
//             cb.range.y = cb.transform.position.y;
//         }
//     }
// }
