using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mGroundManager : MonoBehaviour
{
    public GameObject BasePoint;
    public GameObject FirstPoint;
    public GameObject PrefabPoint;
    public int maxCountWidth = 5;
    public int maxCountHeight = 5;

    // Start is called before the first frame update
    void Start()
    {
        MakeGrid();
    }

    // Update is called once per frame
    void Update()
    {
    }

    void MakeGrid()
    {
        float lengthW = Mathf.Abs(FirstPoint.transform.position.x - BasePoint.transform.position.x);
        float lengthH = Mathf.Abs(FirstPoint.transform.position.z - BasePoint.transform.position.z);

        for (int y=0 ; y<maxCountHeight; y++){
            for (int x=0 ; x<maxCountWidth; x++){
                //
                Vector3 pos = new Vector3(
                    x*lengthW + BasePoint.transform.position.x,
                    0.0f,
                    y*lengthH + BasePoint.transform.position.z
                );
                //
                var newPoint = GameObject.Instantiate(FirstPoint);
                newPoint.transform.parent = BasePoint.transform;
                newPoint.transform.position = pos;
            }
        }
    }
}
