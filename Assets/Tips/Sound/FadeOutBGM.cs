using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sound {
    /// <summary>
    /// BGMをフェースさせるだけ！
    /// </summary>
    public class FadeOutBGM : MonoBehaviour {

        [SerializeField] float time = 1.0f;

        // Start is called before the first frame update
        void Start ()
        {
            SoundManager.Instance.FadeOutBGM (time);
        }
    }
}
