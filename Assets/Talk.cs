using Spine;
using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Networking;
using UnityEngine.UI;
using static Setting;

public class Talk : MonoBehaviour, IPointerClickHandler
{
    public AudioSource audioSource, bgm;
    public Button button;
    public Text text;

    SkeletonAnimation skeletonAnimation;

    bool find = false;

    Dictionary<string, AudioClip> audioClips = new Dictionary<string, AudioClip>();

    int totalSound = 0;
    int soundIndex = 0;
    int secondIndex = 1;
    bool isTalk = false;

    string dataPath, jsonPath, voicePath, soundPath;
    Setting setting;
    IEnumerator Start()
    {
        dataPath = Path.Combine(Directory.GetParent(Application.dataPath).ToString(), "data");

        jsonPath = Path.Combine(dataPath, "setting.json");
        string json = File.ReadAllText(jsonPath);
        setting = JsonUtility.FromJson<Setting>(json);

        button.transform.localScale *= setting.talk.scale;
        button.transform.localPosition = new Vector2(setting.talk.x, setting.talk.y);
        if (setting.debug)
        {
            button.image.color = new Color(200 / 255f, 200 / 255f, 200 / 255f, 200 / 255f);
            text.color = Color.black;
            File.WriteAllText(Path.Combine(dataPath, "debug.log"), System.DateTime.Now + "\n" + setting.student + "\n");
        }

        voicePath = Path.Combine(dataPath, "Voice");

        totalSound = setting.talk.n;

        foreach (string iPath in Directory.GetDirectories(voicePath))
        {
            if (iPath.ToLower().EndsWith(setting.student.Replace("_home", "").Replace("_", "").ToLower()))
            {
                soundPath = iPath;
                if (setting.debug)
                {
                    File.AppendAllText(Path.Combine(dataPath, "debug.log"), soundPath + "\n");
                }
                break;
            }
        }


        DirectoryInfo directoryInfo = new DirectoryInfo(soundPath);
        FileInfo[] files = directoryInfo.GetFiles();
        for (int i = 0; i < files.Length; i++)
        {
            if (files[i].Name.Contains("MemorialLobby"))
            {
                using (UnityWebRequest uwr = UnityWebRequestMultimedia.GetAudioClip("file://" + files[i].FullName, AudioType.OGGVORBIS))
                {
                    yield return uwr.SendWebRequest();
                    if (setting.debug)
                    {
                        File.AppendAllText(Path.Combine(dataPath, "debug.log"), files[i].FullName + "\n");
                    }
                    audioClips.Add(files[i].Name.Replace(".ogg", ""), DownloadHandlerAudioClip.GetContent(uwr));
                }
            }
        }
        audioSource.volume = setting.talk.volume;

        using (UnityWebRequest uwr = UnityWebRequestMultimedia.GetAudioClip("file://" + Path.Combine(dataPath, "BGM", setting.bgm.name + ".ogg"), AudioType.OGGVORBIS))
        {
            yield return uwr.SendWebRequest();
            bgm.clip = DownloadHandlerAudioClip.GetContent(uwr);
            bgm.loop = true;
            bgm.volume = setting.bgm.volume;
            bgm.Play();
        }
    }

    void Update()
    {
        if ((GameObject.Find("New Spine GameObject") && find == false))
        {
            find = true;
            skeletonAnimation = GameObject.Find("New Spine GameObject").GetComponent<SkeletonAnimation>();

            if (setting.debug)
            {
                foreach (Spine.Animation i in skeletonAnimation.skeleton.Data.Animations)
                {
                    File.AppendAllText(Path.Combine(dataPath, "debug.log"), i.Name + "\n");
                }
            }

            skeletonAnimation.AnimationState.Event += HandleEvent;

            skeletonAnimation.AnimationState.Start += delegate (TrackEntry trackEntry)
            {
                if (trackEntry.TrackIndex == 4)
                {
                    isTalk = true;
                }
            };

            skeletonAnimation.AnimationState.Complete += delegate (TrackEntry trackEntry)
            {
                if (trackEntry.TrackIndex == 4)
                {
                    isTalk = false;
                }
            };

            void HandleEvent(TrackEntry trackEntry, Spine.Event e)
            {
                if (setting.talk.onlyTalk)
                {
                    if (e.Data.Name == "Talk")
                    {
                        foreach (string k in audioClips.Keys)
                        {
                            if (k.EndsWith("MemorialLobby_" + soundIndex))
                            {
                                audioSource.PlayOneShot(audioClips[k]);
                                break;
                            }
                            else if (k.EndsWith("MemorialLobby_" + soundIndex + "_" + secondIndex))
                            {
                                audioSource.PlayOneShot(audioClips[k]);
                                secondIndex++;
                                break;
                            }
                        }
                    }
                }
                else
                {
                    foreach (string k in audioClips.Keys)
                    {
                        if (e.Data.Name.Contains(k))
                        {
                            audioSource.PlayOneShot(audioClips[k]);
                            break;
                        }
                    }
                }
            }
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (soundIndex == totalSound && !isTalk)
        {
            soundIndex = 0;
        }
        else if (soundIndex < totalSound && !isTalk)
        {
            soundIndex++;
            secondIndex = 1;
            skeletonAnimation.AnimationState.AddAnimation(3, "Talk_0" + soundIndex + "_A", false, 0);
            skeletonAnimation.AnimationState.AddAnimation(4, "Talk_0" + soundIndex + "_M", false, 0);
        }
        skeletonAnimation.AnimationState.AddEmptyAnimation(3, 0.5f, 0);
        skeletonAnimation.AnimationState.AddEmptyAnimation(4, 0.5f, 0);
    }
}
