using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sound
{
    public static class SoundList
    {
        /*
            SoundManager のBGM/Jingle/SEのリスト登録順にあわせて定義を追加する。 
        */
        public enum BGM {
            BGM00CH0,
            BGM00CH1,
            BGM00CH2,
            BGM00CH3,
        }

        public enum JINGLE {
            START,
            GOAL,
            HIGHSCORE,
            SUPERGOAL,
        }

        public enum SE {
            KKT_UP,
            KKT_DOWN,
            BUTTON_SELECT,
            BUTTON_BACK,
        }

        public enum VOICE {
            //<summary>1-7	Voice	「カカトを上げてください」		チュートリアル（ゆっくりわかりやすく）</summary>
            KKT_UP,
            //<summary>1-8	Voice	「カカトを下げてください」		チュートリアル（ゆっくりわかりやすく）</summary>
            KKT_DOWN,
            //<summary>1-9	Voice	「画面をさわってください」		チュートリアル（ゆっくりわかりやすく）</summary>
            TOUCH_SCREEN,
            //<summary>1-10	Voice	「顔を枠の中に入れてください」	チュートリアル（ゆっくりわかりやすく）</summary>
            FACE_INTO_BOX,
            //<summary>1-11	Voice	「このまま動かないでください」	チュートリアル（ゆっくりわかりやすく）</summary>
            DONT_MOVE,
            //<summary>1-12	Voice	「ありがとうございます！」		チュートリアル（ゆっくりわかりやすく）</summary>
            THANK_YOU,
            //<summary>1-13	Voice	「椅子などに捕まりながら、行ってください」		    チュートリアル（ゆっくりわかりやすく）</summary>
            HOLD_CHIAR,
            //<summary>1-14	Voice	「バランスを崩さないように注意してください」		チュートリアル（ゆっくりわかりやすく）</summary>
            PAY_ATTENTION,
            //<summary>1-15	Voice	「すごい！」		    ゲーム中の掛け声</summary>
            GREATE,
            //<summary>1-16	Voice	「その調子！」		    ゲーム中の掛け声</summary>
            KEEP_GOING,
            //<summary>1-17	Voice	「さすが！」		    ゲーム中の掛け声</summary>
            NICE,
            //<summary>1-18	Voice	「がんばって！」		ゲーム中の掛け声</summary>
            DO_YOUR_BEST,
            //<summary>1-19	Voice	「終了です！」         ゲーム中の掛け声</summary>
            FINISH,
            //<summary>1-20	Voice	「お疲れ様でした！」    ゲーム中の掛け声</summary>
            GOOD_JOB,
        }
    }

}
