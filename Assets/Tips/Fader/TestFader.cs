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
            // ボタンが押されたら、フェードアウト
            Fader.FadeOut(1.0f, ()=>{
                // フェードアウトが終了したら、フェードイン
                Fader.FadeIn();
            });
        });


        btnSwitchScene.onClick.AddListener(()=>{
            // ボタンが押されたら、フェードアウトして、次のシーンへ切り替える
            Fader.SwitchScene(NextScene);
        });
    }
}
