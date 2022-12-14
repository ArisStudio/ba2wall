using System;
using System.Collections.Generic;

[Serializable]
public class Setting
{
    public string student;
    public bool debug;
    public float bgmVolume;
    public Talk talk;

    public List<string> imageList = new List<string>();

    [Serializable]
    public class Talk
    {
        public float volume;
        public bool onlyTalk;
        public int maxIndex;
    }
}
