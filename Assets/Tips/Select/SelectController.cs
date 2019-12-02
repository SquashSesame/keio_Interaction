using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectController : MonoBehaviour {

    public GameObject SelectPosition;
    public UnityEngine.UI.Image prefabSelectItem;

    // 項目リスト
    List<SelectingItem> list = new List<SelectingItem>();

    const float ITEM_HEIGHT = -80;  // 次の項目までの加算位置
    bool hasBegun = false;          // 選択を開始したか？
    bool isSelected = false;        // 選択されたか？
    int result = -1;                // 選択された結果

    #region シングルトン

    // シングルトン用
    static SelectController s_instance = null;
    public static SelectController Instance
    {
        get { return s_instance; }
    }

    // Use this for initialization
    void Awake()
    {
        if (s_instance == null) {
            s_instance = this;
        } else {
            Destroy(this.gameObject);
        }
        isSelected = false;
        result = -1;
    }

    #endregion

    // 全選択項目を登録
    public void Select(params string[] items)
    {
        // 項目登録
        for (int i = 0; i < items.Length; i++){
            AddItem(items[i]);
        }

        // 選択用ワークを初期化
        hasBegun = true;
        isSelected = false;
        result = -1;
    }

    // 選択項目を追加する
    void AddItem(string text)
    {
        // 項目用のGameObjectを生成
        var obj = GameObject.Instantiate(prefabSelectItem);

        // 項目を表示すべき位置を設定（SelectPositionを基準）
        obj.transform.parent = SelectPosition.transform;
        obj.transform.localPosition =
            new Vector3(0.0f, ITEM_HEIGHT * list.Count, 0.0f);
        obj.transform.localScale = Vector3.one;

        // SelectControllerを項目側にも伝える
        var item = obj.GetComponent<SelectingItem>();
        item.SetText(text);         // <- 項目名を伝える
        item.controller = this;     // <- コントローラー自身を伝える
        item.selectNo = list.Count; // <- 選択されたときの番号を伝える

        // 項目リストへ登録
        list.Add(item);
    }

    // 現在選択中か？
    public bool IsExceptSelection
    {
        get { return (hasBegun == false); }
    }

    // 選択が終了したか？
    public bool IsEndOfSelected
    {
        get { return isSelected; }
    }

    // 選択された結果
    public int Result
    {
        get { return result; }
    }

    // どの項目が選択されたかを項目側からもらう
    public void SelectedItem(SelectingItem item)
    {
        isSelected = true;
        result = item.selectNo;
    }

    // 全項目を閉じる
    public void Close()
    {
        foreach (var item in list){
            item.Close();
        }
        list.Clear();

        hasBegun = false;
        isSelected = false;
    }
}
