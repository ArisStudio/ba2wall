using System;
using System.Collections.Generic;

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
