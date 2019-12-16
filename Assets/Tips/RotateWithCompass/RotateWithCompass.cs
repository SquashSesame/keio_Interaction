using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateWithCompass : MonoBehaviour {

    double old_CompassTime = 0;
    Quaternion curCorrection = Quaternion.identity;
    Quaternion aimCorrection = Quaternion.identity;

    static Vector3 GetCompassRawVector() {
        Vector3 ret = Input.compass.rawVector;
        if (Application.platform == RuntimePlatform.Android) 
        {
            // Androidでは、rawVectorの軸を変換
            switch (Screen.orientation) {
                case ScreenOrientation.LandscapeLeft:
                    ret = new Vector3 (-ret.y, ret.x, ret.z);
                    break;

                case ScreenOrientation.LandscapeRight:
                    ret = new Vector3 (ret.y, -ret.x, ret.z);
                    break;

                case ScreenOrientation.PortraitUpsideDown:
                    ret = new Vector3 (-ret.x, -ret.y, ret.z);
                    break;
            }
        }
        return ret;
    }

    void Start ()
    {
        // 入力にジャイロをONにする
        Input.gyro.enabled = true;
        // 入力にコンパスをONにする
        Input.compass.enabled = true;
    }

    void Update ()
    {
        Quaternion gorientation = changeAxis (Input.gyro.attitude);

        if (Input.compass.timestamp > old_CompassTime) {
            old_CompassTime = Input.compass.timestamp;

            Vector3 campassV = GetCompassRawVector();
            Vector3 gravityV = Input.gyro.gravity.normalized;
            Vector3 northV = campassV - Vector3.Dot (gravityV, campassV) * gravityV;

            Quaternion corientation = 
                changeAxis (Quaternion.Inverse (Quaternion.LookRotation (northV, -gravityV)));

            Quaternion tcorrection = 
                corientation * Quaternion.Inverse (gorientation) *　Quaternion.Euler (0, 0, 0);

            if (!isNaN (tcorrection)) {
                aimCorrection = tcorrection;
            }
        }

        if (Quaternion.Angle (curCorrection, aimCorrection) < 45) {
            curCorrection = Quaternion.Slerp (curCorrection, aimCorrection, 0.02f);
        } else {
            curCorrection = aimCorrection;
        }

        transform.localRotation = curCorrection * gorientation;
    }

    static Quaternion changeAxis (Quaternion q)
    {
        var euler = q.eulerAngles;
        return Quaternion.Euler(-euler.x, -euler.y, euler.z);
    }

    static bool isNaN (Quaternion q)
    {
        bool ret =
            float.IsNaN (q.x) || float.IsNaN (q.y) ||
            float.IsNaN (q.z) || float.IsNaN (q.w) ||
            float.IsInfinity (q.x) || float.IsInfinity (q.y) ||
            float.IsInfinity (q.z) || float.IsInfinity (q.w);

        return ret;
    }
}
