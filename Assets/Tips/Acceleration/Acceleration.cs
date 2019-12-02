using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Acceleration : MonoBehaviour {

    const int HISMAX = 10;      // 履歴の個数
    const float BORDER = 2.0f;  // しきい値
    Vector3 center;

    List<Vector3> history = new List<Vector3>(HISMAX);

    // Use this for initialization
    void Start () {
        center = transform.position;
    }
	
	// Update is called once per frame
	void Update () {

        float scale = 2f;
        Vector3 dir = Input.acceleration;
        Vector3 pos = new Vector3 (
            center.x + dir.x * scale,
            center.y + dir.y * scale,
            center.z + dir.z * scale
        );
        this.transform.position = pos;

        if (history.Count >= HISMAX) {
            history.RemoveAt (0);
        }
        history.Add (pos);

        DrawLines ();
    }

    private void OnGUI ()
    {
        Vector3 dir = Input.acceleration;
        GUI.TextField (new Rect (10, 10, 100, 100), "X = " + dir.x.ToString ());
        GUI.TextField (new Rect (10, 30, 100, 100), "Y = " + dir.y.ToString ());
        GUI.TextField (new Rect (10, 50, 100, 100), "Z = " + dir.z.ToString ());
    }

    void DrawLines ()
    {
        for (int i = 1; i < history.Count; i++) {
            float distance = Vector3.Distance (history [i - 1], history [i]);
            Color col = (distance >= BORDER) ? Color.red : Color.green;
            Debug.DrawLine (history[i-1], history[i], col, 2.0f);
        }
    }
}
