using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MessageController2 : MonoBehaviour
{
    // 表示するエリア
    public UnityEngine.UI.Text textArea;
    // 表示するメッセージ
    public string Message;
    // 1文字の表示時間
    public float timeCharDisplay = 0.1f;

    // 表示区切りの文字コード
    const char SPLIT_CODE = '\n';

    int topMessage;
    int idxMessage;
    int cntMessage;
    float waitMessage;
    char lastChar;

    // 表示メッセージを設定
    public void SetMessage(string msg)
    {
        textArea.text = string.Empty;
        this.Message = msg;

        topMessage = 0;
        idxMessage = 0;
        cntMessage = msg.Length;
        waitMessage = 0.0f;
        lastChar = ' ';
    }

    // 終了判定
    public bool IsEndOfMessage
    {
        get { return (topMessage + idxMessage) >= cntMessage; }
    }

    // 表示メッセージを更新
    void UpdateMessage()
    {
        textArea.text = this.Message.Substring(topMessage, idxMessage + 1);
        lastChar = this.Message[topMessage + idxMessage];
    }

    // はじめの処理
    void Awake()
    {
        SetMessage(string.Empty);
    }

    // Update is called once per frame
    void Update()
    {
        if (IsEndOfMessage == false)
        {
            switch (lastChar)
            {
                case SPLIT_CODE:
                    // ボタンが押されるまで待つ
                    if (CheckInputKey())
                    {
                        // 改行送り
                        topMessage += idxMessage + 1;
                        idxMessage = 1;
                        textArea.text = string.Empty;
                        if (IsEndOfMessage == false)
                        {
                            UpdateMessage();
                        }
                    }
                    break;

                default:
                    // クリックされたら全表示
                    if (CheckInputKey())
                    {
                        // 次の改行または文の終了まで
                        while ((topMessage + idxMessage + 1) < cntMessage 
                            && this.Message[topMessage + idxMessage + 1] != SPLIT_CODE)
                        {
                            idxMessage++;
                        }
                        waitMessage = timeCharDisplay;
                    }

                    // １文字表示する
                    waitMessage += Time.deltaTime;
                    if (waitMessage >= timeCharDisplay)
                    {
                        waitMessage = 0.0f;
                        UpdateMessage();
                        if (lastChar != SPLIT_CODE)
                        {
                            idxMessage++;
                        }
                    }
                    break;
            }
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
