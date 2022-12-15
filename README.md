# ba2wall

适用于大多数学生的互动壁纸

[效果预览网页](https://github.com/Tualin14/ba2wall_Demo)

# setting.json

- student 学生文件名
- debug 调试，查看触发位置
- rotation 摆正人物，可以给类似心奈的学生使用
- scale 缩放
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

---

1. 骨骼名以 debug 打开程序看左侧显示
2. 打开程序互动范围覆盖正确即可
3. 因为这些文件并不遵守一定的命名规范。

   以光环为例，一般根骨骼命名为 Halo，Halo_Root,Halo_01

4. 有左右眼命名相反情况，如白子

## 几个学生设置示例

<details>
<summary>小春</summary>
<pre>
```json
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
```</pre>
</details>

<details>
<summary>佳代子</summary>
<pre>
```json
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
```</pre>
</details>

# Credits

- [spine-unity](http://zh.esotericsoftware.com/spine-unity)
- [UnitySkipSplash](https://github.com/psygames/UnitySkipSplash)

# Licence

GPL-3.0 © [Tualin14](https://github.com/Tualin14/ba2wall)
