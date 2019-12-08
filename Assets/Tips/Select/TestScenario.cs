using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestScenario : MonoBehaviour {

    public MessageController2 messageCtrl;
    public SelectController selectCtrl;


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

}
