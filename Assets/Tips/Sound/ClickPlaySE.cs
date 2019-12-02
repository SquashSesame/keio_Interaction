using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sound {
    /// <summary>
    /// このクラスはButtonコンポーネントと併用して試用することを想定
    /// </summary>
    public class ClickPlaySE : MonoBehaviour {

        [SerializeField] SoundList.SE _seNo;

        // Start is called before the first frame update
        void Start ()
        {
            var button = GetComponent<UnityEngine.UI.Button> ();
            if (button != null) {
                button.onClick.AddListener (() => {

                    SoundManager.Instance.PlaySE(_seNo);

                });
            }
        }
    }
}
