using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sound {
    /// <summary>
    /// BGMを鳴らすだけ！
    /// </summary>
    public class PlayBGM : MonoBehaviour {

        [SerializeField] SoundList.BGM _bgmNo;

        // Start is called before the first frame update
        void Start ()
        {

            SoundManager.Instance.PlayBGM(_bgmNo);

        }
    }
}