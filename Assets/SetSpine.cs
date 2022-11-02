using Spine;
using Spine.Unity;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SetSpine : MonoBehaviour
{
    SpineAtlasAsset runtimeAtlasAsset;
    SkeletonDataAsset runtimeSkeletonDataAsset;
    SkeletonAnimation runtimeSkeletonAnimation;

    string atlasPath;
    string skelPath;
    Dictionary<int, string> pngPath = new Dictionary<int, string>();
    string dataPath, studentPath, studentsPath, jsonPath;

    Setting setting;
    void Start()
    {
        dataPath = Path.Combine(Directory.GetParent(Application.dataPath).ToString(), "data");

        jsonPath = Path.Combine(dataPath, "setting.json");
        string json = File.ReadAllText(jsonPath);
        setting = JsonUtility.FromJson<Setting>(json);

        studentsPath = Path.Combine(dataPath, "Student");
        studentPath = Path.Combine(studentsPath, setting.student);

        atlasPath = studentPath + ".atlas.prefab";
        skelPath = studentPath + ".skel.prefab";

        int imgn = 0;
        string homeTmp = "";
        while (File.Exists(studentPath + homeTmp + ".png"))
        {
            pngPath.Add(imgn, setting.student + homeTmp);
            imgn++;
            homeTmp = (imgn + 1).ToString();
        }

        string atlasTxt = File.ReadAllText(atlasPath);
        TextAsset atlasTextAsset = new TextAsset(atlasTxt);

        Texture2D[] textures = new Texture2D[pngPath.Count];
        for (int i = 0; i < pngPath.Count; i++)
        {
            textures[i] = ReadImg(studentsPath, pngPath[i]);
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

    void Update()
    {

    }

    Texture2D ReadImg(string path, string name)
    {
        Texture2D texture = new Texture2D(1, 1);
        texture.LoadImage(File.ReadAllBytes(Path.Combine(path, name + ".png")));
        texture.name = name;
        return texture;
    }
}
