using System;
using System.Net;
using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;

public class HttpToUnity : MonoBehaviour
{
    [SerializeField] public string IPAddressString = "localhost";
    [SerializeField] public int Port = 5000;
    [SerializeField] public bool isStop = false;
    [SerializeField] private GameObject objPlayer = null;

    [SerializeField] private UnityEngine.UI.Button btnStart = null;
    [SerializeField] private UnityEngine.UI.Text textButton = null;
    [SerializeField] private UnityEngine.UI.Text textInfo = null;

    const string pathSeparater = "/";

    public delegate void RequestHandler(string rawUrl, HttpListenerResponse response);

    private Dictionary<System.Text.RegularExpressions.Regex, RequestHandler>
        _requestHandlers = new Dictionary<System.Text.RegularExpressions.Regex, RequestHandler>();

    private HttpListener _httpListener = null;

    // プレイヤー情報（メインスレッドで設定し、通信スレッドで利用する）
    private PositionInfo playerInfo = new PositionInfo();
    
    // IPアドレス一覧
    IPAddress[] _adrList;

    void Start()
    {
        Debug.Assert(btnStart != null);
        Debug.Assert(textButton != null);
        Debug.Assert(textInfo != null);
        
        string hostname = Dns.GetHostName();
        _adrList = Dns.GetHostAddresses(hostname);
        
        btnStart.onClick.AddListener(() =>
        {
            ServerStart();
        });
    }

    private void Update()
    {
        if (objPlayer != null)
        {
            // プレイヤーの位置を通信スレッドへ渡す
            var pos = objPlayer.transform.position;
            playerInfo.strName = objPlayer.name;
            playerInfo.px = pos.x;
            playerInfo.py = pos.y;
            playerInfo.pz = pos.z;
        }
    }

    /// <summary>
    /// サーバー開始
    /// </summary>
    void ServerStart()
    {
        if (_httpListener != null)
        {
            // 既に起動している
            return;
        }

        // １つ目のIPアドレスを利用してみる
        IPAddressString = _adrList[0].MapToIPv4().ToString();

        /*
            コマンド登録
         */
        _requestHandlers[new System.Text.RegularExpressions.Regex(@"^/start$")] = Cmd_start;
        _requestHandlers[new System.Text.RegularExpressions.Regex(@"^/info$")] = Cmd_info;

        // サーバー起動
        StartCoroutine(ServerMain());
    }

    /// <summary>
    /// コルーチン： サーバー起動：HTTPサーバーを起動する（Accept状態開始）
    /// </summary>
    IEnumerator ServerMain()
    {
        string uri = "http://" + IPAddressString + ":" + Port + pathSeparater;
        
        // サーバー起動
        _httpListener = new HttpListener();
        _httpListener.Prefixes.Add(uri);
        _httpListener.Start();
        Debug.Log("- START HTTP Server");

        // サーバーのIPアドレスを表示
        textInfo.text = uri;
        textButton.text = "リクエスト受付中";        
        
        // ここでリクエストを待っている
        while (isStop == false)
        {
            // リクエストがあった場合は、ListenerCallback 関数がコールされる
            _httpListener.BeginGetContext(new AsyncCallback(ListenerCallback), _httpListener);
            yield return new WaitForSeconds(0.1f);
        }

        _httpListener.Stop();
        _httpListener = null;
        Debug.Log("- STOP HTTP Server");
        yield return null;
    }


    /// <summary>
    /// 通信スレッド：登録されているコマンドを呼び出す
    /// </summary>
    void ListenerCallback(IAsyncResult result)
    {
        var listener = (HttpListener) result.AsyncState;
        var context = listener.EndGetContext(result);
        var request = context.Request;
        using (var response = context.Response)
        {
            // リクエストが登録されているコマンドかを判断する（正規表現で比べている）
            foreach (System.Text.RegularExpressions.Regex r in _requestHandlers.Keys)
            {
                System.Text.RegularExpressions.Match m = r.Match(request.Url.AbsolutePath);
                if (m.Success)
                {
                    // 登録されているコマンドのときは、該当のコマンドを実行
                    (_requestHandlers[r])(request.RawUrl, response);
                    return;
                }
            }

            // ERROR
            Debug.LogWarning("ERROR : " + request.Url.ToString());
            response.StatusCode = 404;
            using (var output = response.OutputStream)
            {
                using (var writer = new System.IO.StreamWriter(output))
                {
                    writer.Write("ERROR : 404\n");
                }

                output.Close();
            }
        }
    }

    /// <summary>
    /// コマンド：通信スレッド：ゲーム開始（この中でUnityのAPIをコールしてはダメ）
    /// </summary>
    void Cmd_start(string rawUrl, HttpListenerResponse response)
    {
        Debug.Log("Cmd_start : " + rawUrl);

        // ここではUnityのAPIはコールしてはダメ
        // 必要な場合は、メインスレッドへリクエストし、完了するのをここで待つ
        // GameObject のデータ参照のみであれば問題ない

        // レスポンス
        ResponceText(response, "START!!");
    }


    /// <summary>
    /// コマンド：通信スレッド：プレイヤー位置（この中でUnityのAPIをコールしてはダメ）
    /// </summary>
    void Cmd_info(string rawUrl, HttpListenerResponse response)
    {
        Debug.Log("Cmd_info : " + rawUrl);

        // ここではUnityのAPIはコールしてはダメ
        // 必要な場合は、メインスレッドへリクエストし、完了するのをここで待つ
        // GameObject のデータ参照のみであれば問題ない

        if (objPlayer != null)
        {
            // レスポンス
            ResponseAsJson(response, playerInfo);
        }
        else
        {
            // エラー
            ResponceText(response, "ERROR!!");
        }
    }

    #region テキストストリームでレスポンスする

    /// <summary>
    /// テキストメッセージを返す
    /// </summary>
    void ResponceText(HttpListenerResponse response, string messages)
    {
        using (var output = response.OutputStream)
        {
            using (var writer = new System.IO.StreamWriter(output))
            {
                writer.WriteLine(messages);
            }
            output.Close();
        }
    }

    #endregion

    #region JSONフォーマットでレスポンスする

    /// <summary>
    /// レスポンス用のデータ構造体
    /// </summary>
    [DataContract]
    public class PositionInfo
    {
        [DataMember] public string strName;
        [DataMember] public float px;
        [DataMember] public float py;
        [DataMember] public float pz;
    }

    // JSONシリアライザー
    DataContractJsonSerializer s_jsonSerializer = new DataContractJsonSerializer(typeof(PositionInfo));

    // JSON形式で返す
    void ResponseAsJson(HttpListenerResponse response, PositionInfo data)
    {
        /*
            JSON としてストリームに返信する
         */
        using (var output = response.OutputStream)
        {
            try
            {
                s_jsonSerializer.WriteObject(output, data);
            }
            catch (Exception ex)
            {
                Debug.LogError("ERROR: " + ex.Message);
            }

            output.Close();
        }
    }

    #endregion
    
    
    /// <summary>
    /// IPアドレス一覧表示
    /// </summary>
    void OnGUI()
    {
        GUIStyle myStyle = new GUIStyle(GUI.skin.textArea);
        myStyle.fontSize = 16;

        int posY = 10;
        if (_adrList != null && _adrList.Length > 0){
            for(int i=0; i<_adrList.Length; ++i)
            {
                GUI.Label(new Rect(10,posY, 300, 100), "HOST IP : " + _adrList[i].ToString(), myStyle);
                posY += 40;
            }
        }
        GUI.Label(new Rect(10, posY, 300, 100), "HOST Port : " + Port.ToString(), myStyle);
        posY += 40;
    }
}



