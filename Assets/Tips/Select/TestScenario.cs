using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestScenario : MonoBehaviour {

    public MessageController messageCtrl;
    public SelectController selectCtrl;
    public SpriteRenderer backGround;


	// Use this for initialization
	void Start () {
        StartCoroutine(ScenarioMain());
	}

    // シナリオのメイン
    IEnumerator ScenarioMain()
    {
        yield return WaitMessage(
            "知っているか？\n" +
            "サザエさんのじゃんけんに「必勝法」があることを！！\n\n");

        yield return WaitSelect(
            "知りたい！", 
            "そんなものはない！", 
            "オレ知ってる！"
            );

        switch (selectCtrl.Result)
        {
            case 0:
                yield return WaitMessage(
                  "そ、それは・・\n" +
                  "はい！ここからは有料となります！\n\n");
                break;

            case 1:
                yield return WaitMessage(
                  "あるんだって！本当！\n" +
                  "はい！ここからは有料となります！\n\n");
                break;

            case 2:
                yield return WaitMessage(
                  "煙突の煙でしょ！\n" +
                  "なんだよ～！\n\n");
                break;
        }

        yield return ImageFade(backGround, Fade.Out);

        yield return ImageFade(backGround, Fade.In);

        yield return WaitMessage(
            "はい！終了です！！\n\n");

    }

    // メッセージの表示が終わるまで待つ
    public IEnumerator WaitMessage(string message)
    {
        // メッセージを表示
        messageCtrl.SetMessage(message);

        // メッセージの表示が終わるまで待つ
        while (messageCtrl.IsEndOfMessage == false)
        {
            yield return null;
        }
        yield return null;
    }

    // 選択が終了するまで待つ
    public IEnumerator WaitSelect(params string[] args)
    {
        // 選択肢オープン
        selectCtrl.Select(args);

        // 選択終了まで待つ
        while (selectCtrl.IsEndOfSelected == false) {
            yield return null;
        }

        // 選択肢クローズ
        selectCtrl.Close();
        yield return null;
    }

    public enum Fade {
        In,
        Out,
    }
    public IEnumerator ImageFade(SpriteRenderer sprite, Fade type, float fadeTime=0.5f)
    {
        float stVal, edVal;
        Color col;
        if (type == Fade.In)
        {
            stVal = 0.0f;
            edVal = 1.0f;
        }
        else
        {
            stVal = 1.0f;
            edVal = 0.0f;
        }

        if (fadeTime > 0.0f)
        {
            // 段階的に反映
            float time = 0.0f;
            while (time < fadeTime)
            {
                // フェード中のアルファ値を計算
                float val = Mathf.Lerp(stVal, edVal, time / fadeTime);
                // フェード中の値を設定
                col = sprite.color;
                col.a = val;
                sprite.color = col;
                // 時間を計算
                time += Time.deltaTime;
                yield return null;
            }
        }
        // 最終的な値は設定
        col = sprite.color;
        col.a = edVal;
        sprite.color = col;
        yield return null;
    }

}
