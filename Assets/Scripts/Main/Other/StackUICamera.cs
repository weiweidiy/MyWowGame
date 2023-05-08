
using UnityEngine;
using UnityEngine.Rendering.Universal;

[RequireComponent(typeof(Camera))]
public class StackUICamera : MonoBehaviour
{
    void Start()
    {
        var _UICamera = GameObject.FindWithTag("UICamera").GetComponent<Camera>();
        if (_UICamera != null)
        {
            var _Camera = GetComponent<Camera>();
            _Camera.GetUniversalAdditionalCameraData().cameraStack.Add(_UICamera);
        }
    }
}
