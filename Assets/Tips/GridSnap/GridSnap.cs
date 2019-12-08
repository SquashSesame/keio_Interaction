using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridSnap : MonoBehaviour
{
    public GameObject BasePoint;
    public GameObject FirstPoint;

    float leng_W;
    float leng_H;
    Vector3 ofsPos;

    // Start is called before the first frame update
    void Start()
    {
        leng_W = Mathf.Abs(FirstPoint.transform.position.x - BasePoint.transform.position.x);
        leng_H = Mathf.Abs(FirstPoint.transform.position.z - BasePoint.transform.position.z);
    }

    void OnMouseDown()
    {
        var mPos = Input.mousePosition;
        mPos.z = 10.0f;
        var wPos = Camera.main.ScreenToWorldPoint(mPos);
        ofsPos = transform.position - wPos;
    }

    void OnMouseDrag()
    {
        var mPos = Input.mousePosition;
        mPos.z = 10.0f;
        var wPos = Camera.main.ScreenToWorldPoint(mPos);
        var pos = new Vector3(
            wPos.x + ofsPos.x,
            wPos.y + ofsPos.y,
            wPos.z + ofsPos.z
        );
        transform.position = CalcSnapPosition(pos);
    }

    // スナップ位置を計算
    Vector3 CalcSnapPosition(Vector3 srcPos)
    {
        var bPos = BasePoint.transform.position;
        var sPos = srcPos - bPos;
        var pos = new Vector3(
            srcPos.x - (sPos.x%leng_W) + leng_W/2,
            srcPos.y,
            srcPos.z - (sPos.z%leng_H) + leng_H/2
        );
        return pos;
    }
}
