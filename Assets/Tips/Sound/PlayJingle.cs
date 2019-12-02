using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sound {
    /// <summary>
    /// BGMを鳴らすだけ！
    /// </summary>
    public class PlayJingle : MonoBehaviour {

        [SerializeField] SoundList.JINGLE _jingleNo;

        // Start is called before the first frame update
        void Start ()
        {

            SoundManager.Instance.PlayJingle(_jingleNo);

        }
    }
}