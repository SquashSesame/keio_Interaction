using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scenario2 : MonoBehaviour {

    public MessageController2 msgctrl;
    public MessageController2 msgctrl_telop;

    // Use this for initialization
    void Start()
    {
        msgctrl.SetMessage(
            "空　こぼれ落ちたふたつの星が\n" +
            "光と闇の水面　吸い込まれてゆく\n" +
            "引き合うように　重なる波紋\n"
            );
        
        msgctrl_telop.SetMessage("そのとき！！");
    }
}
