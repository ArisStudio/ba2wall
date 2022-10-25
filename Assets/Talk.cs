using Spine;
using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Networking;
using UnityEngine.UI;

public class Talk : MonoBehaviour, IPointerClickHandler
{
    public AudioSource audioSource,bgm;
    public Button button;
    public Text text;

    SkeletonAnimation skeletonAnimation;

    bool find = false;

    Dictionary<string, AudioClip> audioClips = new Dictionary<string, AudioClip>();

    string soundPath;

    int totalSound = 0;
    int soundIndex = 0;

    string jsonPath;
    Setting setting;
    IEnumerator Start()
    {
        jsonPath = Application.streamingAssetsPath + "/setting.json";
        string json = File.ReadAllText(jsonPath);
        setting = JsonUtility.FromJson<Setting>(json);

        button.transform.localScale *= setting.talk.scale;
        button.transform.localPosition = new Vector2(setting.talk.x, setting.talk.y);
        if (setting.debug)
        {
            button.image.color = new Color(200 / 255f, 200 / 255f, 200 / 255f, 200 / 255f);
            text.color = Color.black;
        }

        soundPath = Application.streamingAssetsPath + "/Sound/";

        totalSound = setting.talk.n;

        DirectoryInfo directoryInfo = new DirectoryInfo(soundPath);
        FileInfo[] files = directoryInfo.GetFiles();
        for (int i = 0; i < files.Length; i++)
        {
            if (files[i].Name.EndsWith(".ogg"))
            {
                using (UnityWebRequest uwr = UnityWebRequestMultimedia.GetAudioClip("file://" + files[i].FullName, AudioType.OGGVORBIS))
                {
                    yield return uwr.SendWebRequest();

                    audioClips.Add(files[i].Name.Replace(".ogg", ""), DownloadHandlerAudioClip.GetContent(uwr));
                }
            }
        }
        audioSource.volume = setting.talk.volume;

        using (UnityWebRequest uwr = UnityWebRequestMultimedia.GetAudioClip("file://" + Application.streamingAssetsPath + "/Theme.ogg", AudioType.OGGVORBIS))
        {
            yield return uwr.SendWebRequest();
            bgm.clip = DownloadHandlerAudioClip.GetContent(uwr);
            bgm.loop = true;
            bgm.volume = setting.bgmv;
            bgm.Play();
        }
    }

    void Update()
    {
        if ((GameObject.Find("New Spine GameObject") && find == false))
        {
            find = true;
            skeletonAnimation = GameObject.Find("New Spine GameObject").GetComponent<SkeletonAnimation>();

            skeletonAnimation.AnimationState.Event += HandleEvent;

            void HandleEvent(TrackEntry trackEntry, Spine.Event e)
            {
                foreach (string k in audioClips.Keys)
                {
                    if (e.Data.Name.Contains(k))
                    {
                        audioSource.PlayOneShot(audioClips[k]);
                    }
                }
            }
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (soundIndex == totalSound && !audioSource.isPlaying)
        {
            soundIndex = 0;
        }
        else if (soundIndex < totalSound && !audioSource.isPlaying)
        {
            soundIndex++;
            skeletonAnimation.AnimationState.AddAnimation(3, "Talk_0" + soundIndex + "_A", false, 0);
            skeletonAnimation.AnimationState.AddAnimation(4, "Talk_0" + soundIndex + "_M", false, 0);
        }

        skeletonAnimation.AnimationState.AddEmptyAnimation(3, 0, 0);
        skeletonAnimation.AnimationState.AddEmptyAnimation(4, 0, 0);
    }
}
