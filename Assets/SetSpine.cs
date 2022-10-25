using SharpJson;
using Spine;
using Spine.Unity;
using System.Collections;
using System.IO;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.Networking;

public class SetSpine : MonoBehaviour
{
    SpineAtlasAsset runtimeAtlasAsset;
    SkeletonDataAsset runtimeSkeletonDataAsset;
    SkeletonAnimation runtimeSkeletonAnimation;

    string atlasPath;
    string skelPath;
    string[] pngPath = new string[2];
    string jsonPath;

    Setting setting;
    void Start()
    {
        string spineDataPath = Application.streamingAssetsPath + "/";

        jsonPath = Application.streamingAssetsPath + "/setting.json";
        string json = File.ReadAllText(jsonPath);
        setting = JsonUtility.FromJson<Setting>(json);

        spineInitialize(spineDataPath);
    }

    void Update()
    {

    }

    void spineInitialize(string spineDataPath)
    {
        DirectoryInfo directoryInfo = new DirectoryInfo(spineDataPath);
        FileInfo[] files = directoryInfo.GetFiles();
        for (int i = 0; i < files.Length; i++)
        {
            if (files[i].Name.EndsWith(".atlas"))
            {
                atlasPath = files[i].FullName;
            }
            else if (files[i].Name.EndsWith(".skel"))
            {
                skelPath = files[i].FullName;
            }
            else if (files[i].Name.EndsWith("home.png"))
            {
                pngPath[0] = files[i].Name;
            }
            else if (files[i].Name.EndsWith("home2.png"))
            {
                pngPath[1] = files[i].Name;
            }
        }

        string atlasTxt = File.ReadAllText(atlasPath);
        TextAsset atlasTextAsset = new TextAsset(atlasTxt);

        Texture2D[] textures = new Texture2D[pngPath.Length];
        for (int i = 0; i < pngPath.Length; i++)
        {
            textures[i] = ReadImg(spineDataPath, pngPath[i]);
        }

        runtimeAtlasAsset = SpineAtlasAsset.CreateRuntimeInstance(atlasTextAsset, textures, Shader.Find("Custom/Shader"), true);

        AtlasAttachmentLoader attachmentLoader = new AtlasAttachmentLoader(runtimeAtlasAsset.GetAtlas());
        SkeletonBinary binary = new SkeletonBinary(attachmentLoader);

        binary.Scale *= 0.01f;
        binary.Scale *= setting.scale;
        SkeletonData skeletonData = binary.ReadSkeletonData(skelPath);

        AnimationStateData stateData = new AnimationStateData(skeletonData);

        runtimeSkeletonDataAsset = SkeletonDataAsset.CreateSkeletonDataAsset(skeletonData, stateData);
        runtimeSkeletonAnimation = SkeletonAnimation.NewSkeletonAnimationGameObject(runtimeSkeletonDataAsset);

        runtimeSkeletonAnimation.Initialize(false);
        runtimeSkeletonAnimation.Skeleton.SetSlotsToSetupPose();
        runtimeSkeletonAnimation.AnimationState.SetAnimation(0, "Idle_01", true);
        runtimeSkeletonAnimation.transform.Translate(Vector3.down * setting.y);
    }

    Texture2D ReadImg(string path, string name)
    {
        Texture2D texture = new Texture2D(1, 1);
        texture.LoadImage(File.ReadAllBytes(path + name));
        texture.name = name.Replace(".png", "");
        return texture;
    }
}
