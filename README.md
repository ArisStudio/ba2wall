# ba2wall

还原学生大厅互动 的 互动壁纸

- [纯框架](https://github.com/Tualin14/ba2wall/releases)
- [wall engine 合集](https://steamcommunity.com/sharedfiles/filedetails/?id=2875378435)

# 0Data

- 0Data 素材存放文件夹（用框架需在根目录新建此文件夹
  - Voice 语音位置
  - Setting.json 设置文件
  - Theme.ogg 背景音乐（如有必要需重命名为 Theme.ogg
  - 其余素材

# Setting.json

- student 学生文件名
- debug 调试，查看触发位置
- rotation 摆正人物，可以给类似心奈的学生使用
- scale 缩放
- lookRange 注释范围，形状为边与两眼平行的正方形
- pat
    - range 摸头范围，形状为与两眼平行的线
    - somethingWrong 如果摸头没有跟随鼠标移动，则设置此为true
- imageList 图片列表，有多少写多少
- bgm
  - enable 若想静音 bgm 可直接关闭，节约一点内存
  - volume 音量 0.0~1.0
- se 音效，少数壁纸有环境音，如佳代子
  - enable 启用音效
  - name 音效文件名
  - volume 音量 0.0~1.0
- talk
    - volume 音量 0.0~1.0
    - onlyTalk 有些学生声音事件没有具体指明，都为 Talk 事件时开启。false 没声音改成 true 就行
    - maxIndex 语音动画数
- bone
  - eyeL 左眼根骨骼名
  - eyeR 右眼根骨骼名
  - halo 光环根骨骼名
  - neck 脖子根骨骼名
- bg 背景如果也是动画的设置，如星野，柚子
  - isSpine 背景是否也为动画
  - name 背景图片名
  - state
    - more 除默认状态外，是否还有其它状态。如星野背景还有鲸鱼运动的动画
    - name 其它状态名
  - imageList 背景图片列表，有多少写多少

<details>
<summary>设置类型</summary>
<pre>
[Serializable]
public class Setting
{
    public string student;
    public bool debug;
    public bool rotation;
    public float scale;
    public float lookRange;
    public Pat pat;
    public Bgm bgm;
    public Se se;
    public Talk talk;
    public Bone bone;
    public Bg bg;

    public List<string> imageList = new List<string>();

    [Serializable]
    public class Bgm
    {
        public bool enable;
        public float volume;
    }

    [Serializable]
    public class Pat
    {
        public float range;
        public bool somethingWrong;
    }

    [Serializable]
    public class Se
    {
        public bool enable;
        public string name;
        public float volume;
    }

    [Serializable]
    public class Talk
    {
        public float volume;
        public bool onlyTalk;
        public int maxIndex;
        public List<string> voiceList = new List<string>();
    }

    [Serializable]
    public class Bone
    {
        public string eyeL;
        public string eyeR;
        public string halo;
        public string neck;
    }

    [Serializable]
    public class Bg
    {
        public bool isSpine;
        public string name;
        public State state;
        public List<string> imageList = new List<string>();
    }

    [Serializable]
    public class State
    {
        public bool more;
        public string name;
    }
}
</pre>
</details>

---

1. 骨骼名以 debug 打开程序看左侧显示
2. 打开程序互动范围覆盖正确即可
3. 因为这些文件并不遵守一定的命名规范。

   以光环为例，一般根骨骼命名为 Halo，Halo_Root,Halo_01

4. 有左右眼命名相反情况，如白子

## 几个学生设置示例

<details>
<summary>小春（人物摆正</summary>
<pre>
{
    "student": "Koharu_home",
    "debug": false,
    "rotation":true,
    "scale":1,
    "imageList": [
        "Koharu_home",
        "Koharu_home2"
    ],
    "bgm": {
        "enable": true,
        "volume": 0.2
    },
    "talk": {
        "volume": 1,
        "onlyTalk": true,
        "maxIndex": 5
    },
    "bone": {
        "eyeL": "L_Eye_1_01",
        "eyeR": "R_Eye_1_01",
        "halo": "Halo_Root",
        "neck": "Neck_01"
    }
}
</pre>
</details>

<details>
<summary>佳代子（背景有雨声</summary>
<pre>
{
    "student": "Kayoko_home",
    "debug": false,
    "rotation": false,
    "scale": 1,
    "imageList": [
        "Kayoko_home",
        "Kayoko_home2"
    ],
    "bgm": {
        "enable": true,
        "volume": 0.2
    },
    "se": {
        "enable": true,
        "name": "Rain.wav",
        "volume": 0.4
    },
    "talk": {
        "volume": 1,
        "onlyTalk": true,
        "maxIndex": 5
    },
    "bone": {
        "eyeL": "L_Eye_01",
        "eyeR": "R_Eye_01",
        "halo": "Halo_Root",
        "neck": "Neck"
    }
}
</pre>
</details>

<details>
<summary>星野（背景也是动画</summary>
<pre>
{
    "student": "Hoshino_home",
    "debug": true,
    "rotation": false,
    "scale": 1,
    "imageList": [
        "Hoshino_home"
    ],
    "bgm": {
        "enable": true,
        "volume": 0.2
    },
    "talk": {
        "volume": 1,
        "onlyTalk": false,
        "maxIndex": 3
    },
    "bone": {
        "eyeL": "L_Eye",
        "eyeR": "R_Eye",
        "halo": "Halo_01",
        "neck": "Neck"
    },
    "bg": {
        "isSpine": true,
        "name": "Hoshino_home_background",
        "state": {
            "more": true,
            "name": "WhaleMove_01_R"
        },
        "imageList": [
            "Hoshino_home_background",
            "Hoshino_home_background2"
        ]
    }
}
</pre>
</details>

<details>
<summary>日步美（解决摸头不随鼠标移动</summary>
<pre>
{
    "student": "Hihumi_home",
    "debug": true,
    "rotation": false,
    "scale": 1,
    "lookRange": 0.5,
    "pat": {
        "range": 0.3,
        "somethingWrong": true
    },
    "imageList": [
        "Hihumi_home",
        "Hihumi_home2"
    ],
    "bgm": {
        "enable": true,
        "volume": 0.3
    },
    "talk": {
        "volume": 1,
        "onlyTalk": false,
        "maxIndex": 6
    },
    "bone": {
        "eyeL": "L_Eye_01",
        "eyeR": "R_Eye_01",
        "halo": "Halo_01",
        "neck": "Neck"
    }
}</pre>
</details>

# Credits

- [spine-unity](http://zh.esotericsoftware.com/spine-unity)
- [UnitySkipSplash](https://github.com/psygames/UnitySkipSplash)

# Licence

GPL-3.0 © [Tualin14](https://github.com/Tualin14/ba2wall)
