using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Events;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;

public class UnityToHttp : MonoBehaviour
{
    [SerializeField] UnityEngine.UI.InputField input;
    [SerializeField] UnityEngine.UI.Text serverURL;
    [SerializeField] UnityEngine.UI.Button  btnPost;
    [SerializeField] UnityEngine.UI.Button  btnGet;
    [SerializeField] UnityEngine.UI.Button  btnPostToDB;
    
    [SerializeField] public string ServerURL = "http://localhost:880";
    [SerializeField] private GameObject Player = null;

    /// <summary>
    /// レスポンス用のデータ構造体
    /// </summary>
    [DataContract]
    public class PositionPacket
    {
        [DataMember] public string message;
        [DataMember] public string strName;
        [DataMember] public float px;
        [DataMember] public float py;
        [DataMember] public float pz;

        public PositionPacket(string msg, GameObject player)
        {
            message = msg;
            strName = player.name;
            var pos = player.transform.position;
            px = pos.x;
            py = pos.y;
            pz = pos.z;
        }
    }
    
    
    // Start is called before the first frame update
    void Start()
    {
        Debug.Assert(input != null);
        Debug.Assert(serverURL != null);
        Debug.Assert(btnPost != null);
        Debug.Assert(btnGet != null);
        Debug.Assert(btnPostToDB != null);
        Debug.Assert(Player != null);

        // サーバーURLの表示
        serverURL.text = ServerURL;
        
        // 各ボタンの処理
        btnPost.onClick.AddListener(() =>
        {
            string text = input.text;
            if (string.IsNullOrEmpty(text))
            {
                text = "-"; //<- なにか入れる
            }
            
            StartCoroutine(SendFormPOST(ServerURL + "/log", text,
                () =>
                {
                    // 成功時
                    Debug.Log("OK");
                }, (code, err) =>
                {
                    // エラー時
                    Debug.LogWarning($"WARN : {code.ToString()} : {err}");
                }));
        });
        
        btnGet.onClick.AddListener(()=>{
            StartCoroutine(SendGET(ServerURL + "/log?data=" + input.text));
        });
        
        btnPostToDB.onClick.AddListener(()=>{
            string text = input.text;
            if (string.IsNullOrEmpty(text))
            {
                text = "-"; //<- なにか入れる
            }
            
            // パケットデータ生成
            var packet = new PositionPacket(text, Player);
            // データをJSON化
            var data = JsonUtility.ToJson (packet);
            // データ送信
            StartCoroutine(SendJsonPOST(ServerURL + "/set_pos", data));
        });
    }

    /// <summary>
    /// データ送信：Form を使ってPOSTする
    /// </summary>
    IEnumerator SendFormPOST(string send_url, string data
        , UnityAction onCompleted=null, UnityAction<long, string> onFailed=null)
    {
        WWWForm form = new WWWForm();
        form.AddField("data", data);
        
        using (var req = UnityWebRequest.Post (send_url, form)) {
            // 送信
            yield return req.SendWebRequest();
            while(!req.isDone){
                yield return null;
            }
            if (req.isHttpError || req.isNetworkError){
                // 何かエラー発生
                Debug.Log("ERROR" + req.responseCode.ToString() + " : " + req.error);
                if (onFailed != null)
                {
                    onFailed.Invoke(req.responseCode, req.error);
                }
            } else {
                // 成功（戻りをログ）
                Debug.Log(req.downloadHandler.text);
                if (onCompleted != null)
                {
                    onCompleted.Invoke();
                }
            }
        }
    }
    
    
    /// <summary>
    /// データ送信：Json を使ってPOSTする
    /// </summary>
    IEnumerator SendJsonPOST(string send_url, string data
        , UnityAction onCompleted=null, UnityAction<long, string> onFailed=null)
    {
        using (var req = new UnityWebRequest (send_url, "POST")) {
            // バイト化
            byte[] postData = System.Text.Encoding.UTF8.GetBytes (data);
            req.uploadHandler = (UploadHandler) new UploadHandlerRaw (postData);
            req.downloadHandler = (DownloadHandler) new DownloadHandlerBuffer ();
            req.SetRequestHeader ("Content-Type", "application/json");
            // 送信
            yield return req.SendWebRequest();
            while(!req.isDone){
                yield return null;
            }
            if (req.isHttpError || req.isNetworkError){
                // 何かエラー発生
                Debug.Log("ERROR" + req.responseCode.ToString() + " : " + req.error);
                if (onFailed != null)
                {
                    onFailed.Invoke(req.responseCode, req.error);
                }
            } else {
                // 成功（戻りをログ）
                Debug.Log(req.downloadHandler.text);
                if (onCompleted != null)
                {
                    onCompleted.Invoke();
                }
            }
        }
    }

    /// <summary>
    /// データ送信：GETする
    /// </summary>
    IEnumerator SendGET(string send_url
            , UnityAction onCompleted=null, UnityAction<long, string> onFailed=null)
    {
        using(var req = UnityWebRequest.Get(send_url)){
            // 送信
            req.SendWebRequest();
            while(!req.isDone){
                yield return null;
            }
            if (req.isHttpError || req.isNetworkError){
                // 何かエラー発生
                Debug.Log("ERROR" + req.responseCode.ToString() + " : " + req.error);
                if (onFailed != null)
                {
                    onFailed.Invoke(req.responseCode, req.error);
                }
            } else {
                // 成功（戻りをログ）
                Debug.Log(req.downloadHandler.text);
                if (onCompleted != null)
                {
                    onCompleted.Invoke();
                }
            }
        }
    }    
    
}
