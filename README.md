# ba2wall

适用于大多数学生的互动壁纸

[网页版](https://github.com/Tualin14/ba2wall_Demo)

## setting.json

若关闭 bgm， 可将 enable 改为 false 节约一点内存

示例

```json
{
  "student": "Yuuka_home",
  "debug": true,
  "imageList": ["Yuuka_home", "Yuuka_home2"],
  "bgm": {
    "enable": false,
    "volume": 0.2
  },
  "talk": {
    "volume": 1,
    "onlyTalk": false,
    "maxIndex": 6
  },
  "bone": {
    "eyeL": "L_eye_01_2",
    "eyeR": "R_eye_01_2",
    "halo": "Halo_Root",
    "neck": "Neck"
  }
}
```

# Credits

- [spine-unity](http://zh.esotericsoftware.com/spine-unity)
- [UnitySkipSplash](https://github.com/psygames/UnitySkipSplash)

# Licence

GPL-3.0 © [Tualin14](https://github.com/Tualin14/ba2wall)
