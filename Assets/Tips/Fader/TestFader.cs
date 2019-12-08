using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestFader : MonoBehaviour
{
    [SerializeField] UnityEngine.UI.Button btnFadeStart = null;
    [SerializeField] UnityEngine.UI.Button btnSwitchScene = null;
    public string NextScene;

    // Start is called before the first frame update
    void Start()
    {
        btnFadeStart.onClick.AddListener(()=>{
            // ボタンが押されたら、フェードアウト　ー＞　フェードイン
            Fader.FadeOut(1.0f, ()=>{
                Debug.Log("フェードアウト終了");
                // フェードアウトが終了したら、フェードイン
                Fader.FadeIn(1.0f, ()=>{
                    // フェードインが終了
                    Debug.Log("フェードイン終了");
                });
            });
        });


        btnSwitchScene.onClick.AddListener(()=>{
            // ボタンが押されたら、フェードアウトして、次のシーンへ切り替える
            Fader.SwitchScene(NextScene);
        });
    }
}
