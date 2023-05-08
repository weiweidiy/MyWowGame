using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[ExecuteInEditMode]
public class ImageLine : MonoBehaviour
{
    public GameObject lineImagePre;

    public Transform lineTransform;

    //public GameObject line;

    public Transform Start;
    public Transform End;

    private void Awake()
    {
        SetLine(Start.position, End.position);
    }

    public void Update()
    {
       // Debug.Log("11");
        //SetLine(Start.position, End.position);
    }

    public void SetLine(Vector2 startPoint, Vector2 endPoint)
    {
        // 角度计算
        Vector2 dir = endPoint - startPoint;
        Vector2 dirV2 = new Vector2(dir.x, dir.y);
        float angle = Vector2.SignedAngle(dirV2, Vector2.down);

        var lineImage = Instantiate(lineImagePre, lineTransform.transform);

        // 距离长度，偏转设置
        lineImage.transform.Rotate(0, 0, angle);
        lineImage.transform.localRotation = Quaternion.AngleAxis(-angle, Vector3.forward);
        float distance = Vector2.Distance(endPoint, startPoint);

        lineImage.GetComponent<RectTransform>().sizeDelta = new Vector2(4f, Mathf.Max(1, distance));

        // 设置位置
        dir = endPoint + startPoint;
        lineImage.GetComponent<RectTransform>().position = new Vector3((float)(dir.x * 0.5f), (float)(dir.y * 0.5f), 0f);
    }

    //public void drawLine(GameObject ICO1, GameObject ICO2, out double length, out double angel)
    //{
    //    Vector2 position1 = ICO1.transform.localPosition;
    //    Vector2 position2 = ICO2.transform.localPosition;
    //    float xValue = 0, yValue = 0;
    //    if ((position1.x > 0 && position2.x > 0) || (position1.x < 0 && position2.x < 0))
    //        xValue = Mathf.Abs(position1.x - position2.x);
    //    else
    //        xValue = Mathf.Abs(position1.x) + Mathf.Abs(position2.x);
    //    if ((position1.y > 0 && position2.y > 0) || (position1.y < 0 && position2.y < 0))
    //        yValue = Mathf.Abs(position1.y - position2.y);
    //    else
    //        yValue = Mathf.Abs(position1.y) + Mathf.Abs(position2.y);
    //    length = Mathf.Sqrt(xValue * xValue + yValue * yValue);

    //    if (position1.x <= position2.x && position1.y <= position2.y)
    //        angel = -90 + Mathf.Atan(yValue / xValue) / Mathf.PI * 180;
    //    else if (position1.x >= position2.x && position1.y <= position2.y)
    //        angel = 90 - Mathf.Atan(yValue / xValue) / Mathf.PI * 180;
    //    else if (position1.x >= position2.x && position1.y >= position2.y)
    //        angel = 90 + Mathf.Atan(yValue / xValue) / Mathf.PI * 180;
    //    else
    //        angel = -90 - Mathf.Atan(yValue / xValue) / Mathf.PI * 180;
    //}

}


//using UnityEngine;
//using UnityEngine.UI;

//[RequireComponent(typeof(Image))]
//[ExecuteAlways]
/////<summary>
///// Draws a UI line between 2 points
///// </summary>
//public class ImageLine : MonoBehaviour
//{
//	public RectTransform rt;
//	public Vector2 pos1, pos2;
//	public float lineWidth;
//	public void Update()
//	{
//		UpdateLine();
//	}

//	public void UpdateLine()
//	{
//		if (!rt)
//			rt = GetComponent<RectTransform>();
//		rt.sizeDelta = new Vector2(Vector2.Distance(pos1, pos2), lineWidth);
//		rt.anchoredPosition = Vector2.Lerp(pos1, pos2, .5f);
//		if (pos2.x - pos1.x != 0)
//			rt.localEulerAngles = Vector3.forward * Mathf.Rad2Deg * Mathf.Atan((pos2.y - pos1.y) / (pos2.x - pos1.x));

//	}
//}