using System;

[Serializable]
public class Setting_B
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
