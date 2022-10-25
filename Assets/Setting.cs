using Spine.Unity;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

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
        public float volume;
        public int x;
        public int y;
        public float scale;
        public int n;
    }
}
