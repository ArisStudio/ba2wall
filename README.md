# ba2wall all in one

[Wallpaper Engine](https://steamcommunity.com/sharedfiles/filedetails/?id=2879635700)

理论上除原始星野及都可以使用(她有两个骨骼文件,幸运的是似乎只有她一个这样)

## setting.json

壁纸中心坐标为(0,0)

- student - 学生 name
- debug - 调试模式
- scale 壁纸缩放
- y 壁纸垂直下移距离
- bgm 背景音乐
  - name 背景音乐名字
  - volume 背景音乐音量
- look 注视
  - range 眼睛移动最大距离
- pat 触摸
  - x 触摸区域中心横坐标
  - y 触摸区域中心纵坐标
  - scale 触摸区域缩放
  - rangex 头向左移最大距离
  - range_x 头向右移最大距离
- talk 对话
  - onlyTalk 选择 true 就行
  - volume 对话音量(0 - 1.0)
  - x 对话区域中心横坐标
  - y 对话区域中心纵坐标
  - scale 对话区域缩放
  - n 对话数量，debug 时看 debug.log 文件，MemorialLobby 后面第一个最大数就是对话数量,或者 Talk 后面最大数字，这俩一样。

### 变量相应类型如下

```csharp
[Serializable]
public class Setting
{
    public string student;
    public bool debug;
    public float scale;
    public float y;
    public Bgm bgm;
    public Look look;
    public Pat pat;
    public Talk talk;

    [Serializable]
    public class Bgm
    {
        public string name;
        public float volume;
    }

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
