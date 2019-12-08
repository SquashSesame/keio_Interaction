using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridSnap2 : MonoBehaviour
{
    public float SnapRange = 1.0f;

    GameObject[] snapPointList;
    Vector3 ofsPos;

    // Start is called before the first frame update
    void Start()
    {
        snapPointList = GameObject.FindGameObjectsWithTag("SnapPoint");
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
        if (snapPointList.Length <= 0){
            // SnapPoint がないとき
            return srcPos;
        }

        // 近くのSnapPointを探す
        GameObject spoint = snapPointList[0];
        float miniDistance = 1000.0f;
        foreach (var item in snapPointList){
            // SnapPoint 毎に距離を測定して判定
            float distance = Vector3.Distance(
                srcPos, item.transform.position);
            if (miniDistance > distance){
                miniDistance = distance;
                spoint = item; //<- 近いSnapPoint
            }
        }
        // Snap Range内か？
        if (miniDistance > SnapRange){
            // Snap Range 以上のとき
            return srcPos;
        }

        // SnapPoint へ移動
        return spoint.transform.position;
    }
}
