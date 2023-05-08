
using UnityEngine;
using UnityEngine.UI;

namespace Framework.Extension
{
    [RequireComponent(typeof(CanvasRenderer))]
    public class EmptyButton : MaskableGraphic
    {
        protected override void OnPopulateMesh(VertexHelper vh)
        {
            vh.Clear();
        }
    }
}