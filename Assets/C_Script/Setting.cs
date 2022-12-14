using System;
using System.Collections.Generic;

[Serializable]
public class Setting
{
    public string student;
    public bool debug;
    public Bgm bgm;
    public Talk talk;
    public Bone bone;

    public List<string> imageList = new List<string>();

    [Serializable]
    public class Bgm
    {
        public bool enable;
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
}
