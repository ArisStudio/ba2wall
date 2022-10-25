using Spine;
using Spine.Unity;
using System.IO;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Pat : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public Button button;
    public Text text;

    Camera cam;
    Bone bone;
    SkeletonAnimation skeletonAnimation;

    string patA, patM, patEndA, patEndM = null;

    float oBoneX, oBoneY;
    bool find = false;
    bool isDown = false;
    int isTalk = 0;

    string jsonPath;
    Setting setting;

    void Start()
    {
        cam = Camera.main;

        jsonPath = Application.streamingAssetsPath + "/setting.json";
        string json = File.ReadAllText(jsonPath);
        setting = JsonUtility.FromJson<Setting>(json);


        button.transform.localScale *= setting.pat.scale;
        button.transform.localPosition = new Vector2(setting.pat.x, setting.pat.y);
        if (setting.debug)
        {
            button.image.color = new Color(200 / 255f, 200 / 255f, 200 / 255f, 200 / 255f);
            text.color = Color.black;
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        isDown = true;
        if (patA != null)
        {
            var Track = skeletonAnimation.AnimationState.SetAnimation(1, patA, false);
            Track.AttachmentThreshold = 1f;
        }
        if (patM != null)
        {
            var Track = skeletonAnimation.AnimationState.SetAnimation(2, patM, false);
            Track.AttachmentThreshold = 1f;
        }
    }
    public void OnPointerUp(PointerEventData eventData)
    {
        if (patEndA != null)
        {
            var Track = skeletonAnimation.AnimationState.SetAnimation(1, patEndA, false);
            Track.AttachmentThreshold = 1f;
        }
        if (patEndM != null)
        {
            var Track = skeletonAnimation.AnimationState.SetAnimation(2, patEndM, false);
            Track.AttachmentThreshold = 1f;
        }

        bone.SetToSetupPose();
        isDown = false;
    }

    void Update()
    {
        if ((GameObject.Find("New Spine GameObject") && find == false))
        {
            find = true;
            skeletonAnimation = GameObject.Find("New Spine GameObject").GetComponent<SkeletonAnimation>();

            bone = skeletonAnimation.skeleton.FindBone("Touch_Point");

            oBoneX = bone.WorldX;
            oBoneY = bone.WorldY;


            skeletonAnimation.AnimationState.Start += delegate (TrackEntry trackEntry)
            {
                if (trackEntry.TrackIndex == 3 || trackEntry.TrackIndex == 4)
                {
                    isTalk++;
                }
            };

            skeletonAnimation.AnimationState.End += delegate (TrackEntry trackEntry)
            {
                if (trackEntry.TrackIndex == 3 || trackEntry.TrackIndex == 4)
                {
                    isTalk--;
                }
            };


            foreach (Spine.Animation i in skeletonAnimation.skeleton.Data.Animations)
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
        }


        if (isDown && isTalk == 0)
        {
            var mousePosition = Input.mousePosition;
            var worldMousePosition = cam.ScreenToWorldPoint(mousePosition);
            var skeletonSpacePoint = skeletonAnimation.transform.InverseTransformPoint(worldMousePosition);

            if (skeletonSpacePoint.x - oBoneX >= setting.pat.rangex)
            {
                skeletonSpacePoint.x = (float)(oBoneX + setting.pat.rangex);
            }
            else if (skeletonSpacePoint.x - oBoneX <= setting.pat.range_x)
            {
                skeletonSpacePoint.x = (float)(oBoneX + setting.pat.range_x);
            }

            skeletonSpacePoint.y = oBoneY;

            bone.SetPositionSkeletonSpace(skeletonSpacePoint);
        }

    }
}
