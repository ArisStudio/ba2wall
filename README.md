# ba2wall

一个方便自制互动壁纸框架

适用于大多数人物

[Wallpaper Engine](https://steamcommunity.com/sharedfiles/filedetails/?id=2879635700)

# How to use

[视频演示](https://www.bilibili.com/video/BV1yR4y1Q7bf)

使用前记得备份

1. 将 `skel` 和 `atlas` 及相应图片文件放入 `./ba2wall_Data/StreamingAssets` 文件夹
2. 将相应背景音乐也放入 `./ba2wall_Data/StreamingAssets` 文件夹 并更名为 `Theme.ogg`
3. 将相应声音文件放入 `./ba2wall_Data/StreamingAssets/Sound` 文件夹
4. 修改 `./ba2wall_Data/StreamingAssets/setting.json` 配置文件

已发现无法使用人物及原因，请确保文件结构与示例相似(大多数都相似)

- 星野 有两个骨骼文件
- 其余未知，若发现其他无法使用人物，可提 Issues，

## setting.json

壁纸中心坐标为(0,0)

- debug 开启会显示互动区域
- scale 壁纸缩放
- y 壁纸垂直下移距离
- bgmv bgm 音量(0 - 1.0)
- look 注视
  - range 眼睛移动最大范围
- pat 触摸
  - x 触摸区域中心横坐标
  - y 触摸区域中心纵坐标
  - scale 触摸区域缩放
  - rangex 头向左移最大距离
  - range_x 头向右移最大距离
- talk 对话
  - onlyTalk 有些人物对话仅为 Talk 事件，没有详细为具体哪一句话。false 不行就换 true 试试.
  - volume 对话音量(0 - 1.0)
  - x 对话区域中心横坐标
  - y 对话区域中心纵坐标
  - scale 对话区域缩放
  - n 对话事件数量(可根据声音文件中最大数字得出)

### 变量相应类型如下

```csharp
[Serializable]
public class Setting
{
    public bool debug;
    public float scale;
    public float y;
    public float bgmv;
    public Look look;
    public Pat pat;
    public Talk talk;

    [Serializable]
    public class Look
    {
        public float range;
    }

    [Serializable]
    public class Pat
    {
        public int x;
        public int y;
        public float scale;
        public float rangex;
        public float range_x;
    }

    [Serializable]
    public class Talk
    {
        public bool onlyTalk;
        public float volume;
        public int x;
        public int y;
        public float scale;
        public int n;
    }
}
```

# Credits

- [spine-unity](http://zh.esotericsoftware.com/spine-unity)
- [UnitySkipSplash](https://github.com/psygames/UnitySkipSplash)

# Licence

GPL-3.0 © [Tualin14](https://github.com/Tualin14/ba2wall)
