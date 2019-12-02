using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectingItem : MonoBehaviour {

    public UnityEngine.UI.Text uiText;
    public Animator animItem;
    public SelectController controller;

    // 選択された番号
    public int selectNo;

    // 選択されたときにコントローラーへ知らせる
    public void OnSelected(){
        controller.SelectedItem(this);
    }

    // 閉じる
    public void Close(){
        animItem.SetTrigger("close");
        // 自動で削除される
    }

    //　項目名を設定
    public void SetText(string msg){
        uiText.text = msg;
    }
}
