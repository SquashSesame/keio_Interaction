using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sound {
    /// <summary>
    /// ランダムでSEを鳴らすだけ！
    /// </summary>
    public class PlaySE : MonoBehaviour {

        [SerializeField] SoundList.SE[] _seNos;

        // Start is called before the first frame update
        void Start ()
        {
            if (_seNos.Length > 0) {
                int idx = UnityEngine.Random.Range (0, _seNos.Length);
                SoundManager.Instance.PlaySE (_seNos [idx]);
            }
        }
    }
}
