using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MessageController : MonoBehaviour
{
    // 表示するエリア
    public UnityEngine.UI.Text textArea;
    // 表示するメッセージ
    public string Message;

    int topMessage;
    int idxMessage;
    int cntMessage;
    char lastChar;

    // 表示メッセージを設定
    public void SetMessage(string msg)
    {
        textArea.text = string.Empty;
        this.Message = msg;

        topMessage = 0;
        idxMessage = 0;
        cntMessage = msg.Length;
        // 更新
        UpdateMessage();
    }

    // 終了判定
    public bool IsEndOfMessage
    {
        get { return (topMessage + idxMessage) >= cntMessage; }
    }

    // 表示区切りの文字コード
    const char SPLIT_CODE = '\n';

    // 表示メッセージを更新
    void UpdateMessage()
    {
        lastChar = ' ';
        // 改行までの文字数を計算
        while (lastChar != SPLIT_CODE && IsEndOfMessage == false)
        {
            lastChar = this.Message[topMessage + idxMessage];
            idxMessage++;
        }
        // 表示
        if (cntMessage > 0)
        {
            // topMessage　文字目から、idxMessage 分を表示する
            textArea.text = this.Message.Substring(topMessage, idxMessage);
        }
        else
        {
            textArea.text = string.Empty;
        }
    }

    // はじめの処理
    void Awake()
    {
        SetMessage(string.Empty);
    }

    // Update is called once per frame
    void Update()
    {
        // マウスボタンが押されるまで待つ
        if (CheckInputKey())
        {
            // 改行までの文章を更新
            topMessage += idxMessage;
            idxMessage = 0;
            UpdateMessage();
        }
    }

    // キー入力がされたか？
    bool CheckInputKey()
    {
        return (
            // マウスがクリックされたか？
            Input.GetMouseButtonDown(0)
             // 選択中は無視する
             && (SelectController.Instance == null
             || SelectController.Instance.IsExceptSelection));
    }
}
