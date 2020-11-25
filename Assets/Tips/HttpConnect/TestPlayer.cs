using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestPlayer : MonoBehaviour
{
    [SerializeField] float SPEED = 1.0f;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        var pos = transform.position;
        // プレイヤーの場合：移動操作＆送信
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            pos.x -= SPEED * Time.deltaTime;
        }

        if (Input.GetKey(KeyCode.RightArrow))
        {
            pos.x += SPEED * Time.deltaTime;
        }

        if (Input.GetKey(KeyCode.UpArrow))
        {
            pos.z += SPEED * Time.deltaTime;
        }

        if (Input.GetKey(KeyCode.DownArrow))
        {
            pos.z -= SPEED * Time.deltaTime;
        }
        
        // 反映
        transform.position = pos;
    }
}
