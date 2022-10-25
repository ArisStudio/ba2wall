using Spine;
using Spine.Unity;
using System;
using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Look : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    Camera cam;
    Bone bone;
    SkeletonAnimation skeletonAnimation;

    string lookA, lookM, lookEndA, lookEndM = null;

    float oBoneX, oBoneY;
    bool find = false;
    bool isDown = false;
    float lookXY;

    int isTalk = 0;

    string jsonPath;
    Setting setting;
    void Start()
    {
        cam = Camera.main;

        jsonPath = Application.streamingAssetsPath + "/setting.json";
        string json = File.ReadAllText(jsonPath);
        setting = JsonUtility.FromJson<Setting>(json);

        lookXY = setting.look.range;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        isDown = true;
        if (lookA != null)
        {
            var Track = skeletonAnimation.AnimationState.SetAnimation(1, lookA, false);
            Track.AttachmentThreshold = 1f;
        }
        if (lookM != null)
        {
            var Track = skeletonAnimation.AnimationState.SetAnimation(2, lookM, false);
            Track.AttachmentThreshold = 1f;
        }
    }
    public void OnPointerUp(PointerEventData eventData)
    {
        if (lookEndA != null)
        {
            var Track = skeletonAnimation.AnimationState.SetAnimation(1, lookEndA, false);
            Track.AttachmentThreshold = 1f;
        }
        if (lookEndM != null)
        {
            var Track = skeletonAnimation.AnimationState.SetAnimation(2, lookEndM, false);
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

            bone = skeletonAnimation.skeleton.FindBone("Touch_Eye");

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
            };
        }

        if (isDown && isTalk == 0)
        {
            var mousePosition = Input.mousePosition;
            var worldMousePosition = cam.ScreenToWorldPoint(mousePosition);
            var skeletonSpacePoint = skeletonAnimation.transform.InverseTransformPoint(worldMousePosition);

            var sx = (skeletonSpacePoint.y - oBoneY) / (skeletonSpacePoint.x - oBoneX);
            var sy = (skeletonSpacePoint.x - oBoneX) / (skeletonSpacePoint.y - oBoneY);

            if (skeletonSpacePoint.x - oBoneX >= lookXY && Math.Abs(sx) <= 1)
            {
                skeletonSpacePoint.y = oBoneY + lookXY * sx;
                skeletonSpacePoint.x = (float)(oBoneX + lookXY);
            }
            else if (skeletonSpacePoint.x - oBoneX <= -lookXY && Math.Abs(sx) <= 1)
            {
                skeletonSpacePoint.y = (float)(oBoneY - lookXY * sx);
                skeletonSpacePoint.x = (float)(oBoneX - lookXY);
            }

            else if (skeletonSpacePoint.y - oBoneY >= lookXY && Math.Abs(sx) > 1)
            {
                skeletonSpacePoint.y = (float)(oBoneY + lookXY);
                skeletonSpacePoint.x = (float)(oBoneX + lookXY * sy);
            }
            else if (skeletonSpacePoint.y - oBoneY <= -lookXY && Math.Abs(sx) > 1)
            {
                skeletonSpacePoint.y = (float)(oBoneY - lookXY);
                skeletonSpacePoint.x = (float)(oBoneX - lookXY * sy);
            }

            bone.SetPositionSkeletonSpace(skeletonSpacePoint);
        }
    }
}

