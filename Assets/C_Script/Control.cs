using Spine.Unity;
using Spine;
using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;
using System.Collections.Generic;
using UnityEngine.UI;
using Unity.Mathematics;
using System;

public class Control : MonoBehaviour
{
    public GameObject spineBase;
    public Button lookBtn, patBtn, talkBtn;
    public AudioSource voice, bgm;

    SkeletonAnimation sprAnim;

    Bone patBone, lookBone;

    Vector3 eyeL, eyeR, halo, neck;
    Setting setting;

    bool isTalking = false;
    Dictionary<string, AudioClip> voiceList = new Dictionary<string, AudioClip>();
    int voiceIndex = 1;
    int secondVoiceIndex = 1;
    int totalVoice = 5;

    //Look
    bool isLooking = false;
    bool lookEnding = false;
    float lookSpeed = 4;
    float lookRange = 1f;
    Vector3 look;
    string lookA, lookM, lookEndA, lookEndM;

    //Pat
    bool isPatting = false;
    bool patEnding = false;
    float patSpeed = 2;
    float patRange = 0.5f;
    Vector3 pat;
    string patA, patM, patEndA, patEndM;

    IEnumerator Start()
    {
        string rootPath = Directory.GetParent(Application.dataPath).ToString();
        string dataFolderPath = Path.Combine(rootPath, "0Data");
        string settingPath = Path.Combine(dataFolderPath, "Setting.json");

        string settingJson;
        using (UnityWebRequest uwr = UnityWebRequest.Get(settingPath))
        {
            yield return uwr.SendWebRequest();
            settingJson = uwr.downloadHandler.text;
        }
        setting = JsonUtility.FromJson<Setting>(settingJson);

        if (setting.debug)
        {
            patBtn.GetComponent<Image>().color = new Color(0.5f, 0.5f, 0.5f, 0.5f);
            talkBtn.GetComponent<Image>().color = new Color(0.5f, 0.5f, 0.5f, 0.5f);
        }

        if (setting.bgm.enable)
        {
            bgm.volume = setting.bgm.volume;

            string bgmPath = Path.Combine(dataFolderPath, "Theme.ogg");
            using (UnityWebRequest uwr = UnityWebRequestMultimedia.GetAudioClip(bgmPath, AudioType.OGGVORBIS))
            {
                yield return uwr.SendWebRequest();
                bgm.clip = DownloadHandlerAudioClip.GetContent(uwr);
                bgm.Play();
            }
        }
        else
        {
            bgm.gameObject.SetActive(false);
        }

        string studentName = setting.student;
        string voicePath = Path.Combine(dataFolderPath, "Voice");

        //for WebGL
        //foreach (string i in setting.talk.voiceList)
        //{
        //    using (UnityWebRequest uwr = UnityWebRequestMultimedia.GetAudioClip(Path.Combine(voicePath, i + ".ogg"), AudioType.OGGVORBIS))
        //    {
        //        yield return uwr.SendWebRequest();
        //        voiceList.Add(i, DownloadHandlerAudioClip.GetContent(uwr));
        //    }
        //}

        DirectoryInfo directoryInfo = new DirectoryInfo(voicePath);
        FileInfo[] files = directoryInfo.GetFiles();
        for (int i = 0; i < files.Length; i++)
        {
            if (files[i].Name.Contains("MemorialLobby"))
            {
                using (UnityWebRequest uwr = UnityWebRequestMultimedia.GetAudioClip(files[i].FullName, AudioType.OGGVORBIS))
                {
                    yield return uwr.SendWebRequest();
                    voiceList.Add(files[i].Name.Replace(".ogg", ""), DownloadHandlerAudioClip.GetContent(uwr));
                }
            }
        }

        // Load and Add Spine
        string atlasTxt;
        byte[] imageData, skelData;

        string sprPath = Path.Combine(dataFolderPath, studentName);
        string atlasPath = sprPath + ".atlas";
        string skelPath = sprPath + ".skel";

        using (UnityWebRequest uwr = UnityWebRequest.Get(atlasPath))
        {
            yield return uwr.SendWebRequest();
            atlasTxt = uwr.downloadHandler.text;
        }
        TextAsset atlasTextAsset = new TextAsset(atlasTxt);

        List<string> imageList = setting.imageList;
        int imgCount = imageList.Count;
        Texture2D[] textures = new Texture2D[imgCount];
        Texture2D texture;
        for (int i = 0; i < imgCount; i++)
        {
            texture = new Texture2D(1, 1);
            string imgName = imageList[i];
            using (UnityWebRequest uwr = UnityWebRequest.Get(Path.Combine(dataFolderPath, imgName + ".png")))
            {
                yield return uwr.SendWebRequest();
                imageData = uwr.downloadHandler.data;
            }
            texture.LoadImage(imageData);
            texture.name = imgName;
            textures[i] = texture;
        }

        SpineAtlasAsset SprAtlasAsset = SpineAtlasAsset.CreateRuntimeInstance(atlasTextAsset, textures, Shader.Find("Custom/Shader"), true);

        AtlasAttachmentLoader attachmentLoader = new AtlasAttachmentLoader(SprAtlasAsset.GetAtlas());
        SkeletonBinary binary = new SkeletonBinary(attachmentLoader);
        binary.Scale *= 0.0115f;

        using (UnityWebRequest uwr = UnityWebRequest.Get(skelPath))
        {
            yield return uwr.SendWebRequest();
            skelData = uwr.downloadHandler.data;
        }

        SkeletonData skeletonData = binary.ReadSkeletonData(studentName, skelData);
        AnimationStateData stateData = new AnimationStateData(skeletonData);
        SkeletonDataAsset sprSkeletonDataAsset = SkeletonDataAsset.CreateSkeletonDataAsset(skeletonData, stateData);
        sprAnim = SkeletonAnimation.AddToGameObject(spineBase, sprSkeletonDataAsset);

        sprAnim.Initialize(false);
        sprAnim.Skeleton.SetSlotsToSetupPose();
        sprAnim.AnimationState.SetAnimation(0, "Idle_01", true);

        sprAnim.AnimationState.Event += HandleEvent;

        void HandleEvent(TrackEntry trackEntry, Spine.Event e)
        {
            if (setting.talk.onlyTalk)
            {
                if (e.Data.Name == "Talk")
                {
                    foreach (string k in voiceList.Keys)
                    {
                        if (k.EndsWith("MemorialLobby_" + voiceIndex))
                        {
                            voice.clip = voiceList[k];
                            voice.Play();
                            break;
                        }
                        else if (k.EndsWith("MemorialLobby_" + voiceIndex + "_" + secondVoiceIndex))
                        {
                            voice.clip = voiceList[k];
                            voice.Play();
                            secondVoiceIndex++;
                            break;
                        }
                    }
                }
            }
            else
            {
                foreach (string k in voiceList.Keys)
                {
                    if (e.Data.Name.Contains(k))
                    {
                        voice.clip = voiceList[k];
                        voice.Play();
                        break;
                    }
                }
            }
        }

        sprAnim.AnimationState.Complete += delegate (TrackEntry trackEntry)
        {
            if (trackEntry.TrackIndex == 4)
            {
                isTalking = false;
                Debug.Log("4 End: " + trackEntry.ToString());
            }
        };

        eyeL = Camera.main.WorldToScreenPoint(sprAnim.skeleton.FindBone(setting.bone.eyeL).GetWorldPosition(sprAnim.transform));
        eyeR = Camera.main.WorldToScreenPoint(sprAnim.skeleton.FindBone(setting.bone.eyeR).GetWorldPosition(sprAnim.transform));
        halo = Camera.main.WorldToScreenPoint(sprAnim.skeleton.FindBone(setting.bone.halo).GetWorldPosition(sprAnim.transform));

        SetPatAndTalkButton(eyeL, eyeR, halo);
        SetLook();

    }


    void Update()
    {
        var mousePosition = Input.mousePosition;
        var worldMousePosition = Camera.main.ScreenToWorldPoint(mousePosition);
        var downPoint = patBtn.transform.InverseTransformPoint(worldMousePosition);

        if (!isTalking)
        {
            if (isPatting)
            {
                if (downPoint.x - pat.x >= patRange)
                {
                    downPoint.x = pat.x + patRange;
                }
                else if (downPoint.x - pat.x <= -patRange)
                {
                    downPoint.x = pat.x - patRange;
                }
                downPoint.y = pat.y;

                downPoint = patBtn.transform.TransformPoint(downPoint);
                patBone.SetPositionSkeletonSpace(downPoint);
            }
            else if (patEnding)
            {
                if (math.abs(patBone.X - pat.x) <= 0.1f)
                {
                    patEnding = false;
                    patBone.SetToSetupPose();
                }
                else
                {
                    Vector3 tmpP = Vector3.MoveTowards(patBone.GetWorldPosition(sprAnim.transform), patBtn.transform.TransformPoint(pat), patSpeed * Time.deltaTime);
                    patBone.SetPositionSkeletonSpace(tmpP);
                }
            }

            if (isLooking)
            {
                var sx = (downPoint.y - look.y) / (downPoint.x - look.x);
                var sy = (downPoint.x - look.x) / (downPoint.y - look.y);

                if (downPoint.x - look.x >= lookRange && Math.Abs(sx) <= 1)
                {
                    downPoint.y = look.y + lookRange * sx;
                    downPoint.x = look.x + lookRange;
                }
                else if (downPoint.x - look.x <= -lookRange && Math.Abs(sx) <= 1)
                {
                    downPoint.y = look.y - lookRange * sx;
                    downPoint.x = look.x - lookRange;
                }

                else if (downPoint.y - look.y >= lookRange && Math.Abs(sx) > 1)
                {
                    downPoint.y = look.y + lookRange;
                    downPoint.x = look.x + lookRange * sy;
                }
                else if (downPoint.y - look.y <= -lookRange && Math.Abs(sx) > 1)
                {
                    downPoint.y = look.y - lookRange;
                    downPoint.x = look.x - lookRange * sy;
                }

                downPoint = patBtn.transform.TransformPoint(downPoint);
                lookBone.SetPositionSkeletonSpace(downPoint);
            }
            else if (lookEnding)
            {
                if (math.abs(lookBone.X - look.x) <= 0.1f)
                {
                    lookEnding = false;
                    lookBone.SetToSetupPose();
                }
                else
                {
                    Vector3 tmpP = Vector3.MoveTowards(lookBone.GetWorldPosition(sprAnim.transform), patBtn.transform.TransformPoint(look), lookSpeed * Time.deltaTime);
                    lookBone.SetPositionSkeletonSpace(tmpP);
                }
            }
        }
    }

    public void SetTalking()
    {
        if (!isTalking)
        {
            isTalking = true;
            if (voiceIndex > totalVoice)
            {
                voiceIndex = 1;
            }

            secondVoiceIndex = 1;
            sprAnim.AnimationState.SetAnimation(3, "Talk_0" + voiceIndex + "_A", false);
            sprAnim.AnimationState.SetAnimation(4, "Talk_0" + voiceIndex + "_M", false);
            sprAnim.AnimationState.AddEmptyAnimation(3, 0.2f, 0);
            sprAnim.AnimationState.AddEmptyAnimation(4, 0.2f, 0);
            voiceIndex++;
        }
    }

    public void SetPatting(bool b)
    {
        if (!isTalking)
        {
            Debug.Log("isPatting: " + b);
            isPatting = b;
            if (b)
            {
                if (patA != null)
                {
                    sprAnim.AnimationState.SetAnimation(1, patA, false);
                }
                if (patM != null)
                {
                    sprAnim.AnimationState.SetAnimation(2, patM, false);
                }
            }
            else
            {
                if (patEndA != null)
                {
                    sprAnim.AnimationState.AddAnimation(1, patEndA, false, 0);
                }
                if (patEndM != null)
                {
                    sprAnim.AnimationState.AddAnimation(2, patEndM, false, 0);
                }
                sprAnim.AnimationState.AddEmptyAnimation(1, 0.5f, 0);
                sprAnim.AnimationState.AddEmptyAnimation(2, 0.5f, 0);

                patEnding = true;
            }
        }
    }

    void SetPatAndTalkButton(Vector3 l, Vector3 r, Vector3 h)
    {
        // PatButton
        float patAngle = GetAngle(l, r);
        patBtn.transform.localEulerAngles = new Vector3(0, 0, patAngle);

        Vector3 betweenPoint = GetBetweenPoint(l, r);
        float weight = GetDistance(l, r);
        float hight = GetDistance(h, betweenPoint);
        patBtn.transform.GetComponent<RectTransform>().sizeDelta = new Vector2(weight * 3, hight);
        patBtn.transform.position = betweenPoint;

        // TalkButton
        neck = Camera.main.WorldToScreenPoint(sprAnim.skeleton.FindBone(setting.bone.neck).GetWorldPosition(sprAnim.transform));
        talkBtn.transform.localEulerAngles = new Vector3(0, 0, patAngle);
        talkBtn.transform.GetComponent<RectTransform>().sizeDelta = new Vector2(weight * 3, hight);
        talkBtn.transform.position = neck;

        foreach (Spine.Animation i in sprAnim.skeleton.Data.Animations)
        {
            if (i.Name.StartsWith("Pat_0"))
            {
                if (i.Name.EndsWith("A"))
                {
                    patA = i.Name;
                }
                else if (i.Name.EndsWith("M"))
                {
                    patM = i.Name;
                }
            }
            else if (i.Name.StartsWith("PatEnd"))
            {
                if (i.Name.EndsWith("A"))
                {
                    patEndA = i.Name;
                }
                else if (i.Name.EndsWith("M"))
                {
                    patEndM = i.Name;
                }
            }
        }

        patBone = sprAnim.skeleton.FindBone("Touch_Point");
        pat = patBtn.transform.InverseTransformPoint(patBone.GetWorldPosition(sprAnim.transform));
    }

    public void SetLooking(bool b)
    {
        if (!isTalking)
        {
            isLooking = b;
            Debug.Log("isLooking: " + b);
            if (b)
            {
                if (lookA != null)
                {
                    sprAnim.AnimationState.SetAnimation(1, lookA, false);
                }
                if (lookM != null)
                {
                    sprAnim.AnimationState.SetAnimation(2, lookM, false);
                }
            }
            else
            {
                if (lookEndA != null)
                {
                    sprAnim.AnimationState.AddAnimation(1, lookEndA, false, 0);
                }
                if (lookEndM != null)
                {
                    sprAnim.AnimationState.AddAnimation(2, lookEndM, false, 0);
                }
                sprAnim.AnimationState.AddEmptyAnimation(1, 0.5f, 0);
                sprAnim.AnimationState.AddEmptyAnimation(2, 0.5f, 0);

                lookEnding = true;
            }
        }
    }

    void SetLook()
    {
        foreach (Spine.Animation i in sprAnim.skeleton.Data.Animations)
        {
            if (i.Name.StartsWith("Look_0"))
            {
                if (i.Name.EndsWith("A"))
                {
                    lookA = i.Name;
                }
                else if (i.Name.EndsWith("M"))
                {
                    lookM = i.Name;
                }
            }
            else if (i.Name.StartsWith("LookEnd"))
            {
                if (i.Name.EndsWith("A"))
                {
                    lookEndA = i.Name;
                }
                else if (i.Name.EndsWith("M"))
                {
                    lookEndM = i.Name;
                }
            }
        }

        lookBone = sprAnim.skeleton.FindBone("Touch_Eye");
        look = patBtn.transform.InverseTransformPoint(lookBone.GetWorldPosition(sprAnim.transform));
    }

    float GetAngle(Vector3 l, Vector3 r)
    {
        Vector3 dir = l - r;
        dir = dir.normalized;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        angle = angle < 0 ? angle + 360 : angle;
        return angle;
    }

    Vector3 GetBetweenPoint(Vector3 l, Vector3 r)
    {
        float x2 = l.x + r.x;
        float y2 = l.y + r.y;
        return new Vector3(x2, y2, 0) / 2;
    }

    float GetDistance(Vector3 a, Vector3 b)
    {
        float distance = (a - b).magnitude;
        return distance;
    }
}
