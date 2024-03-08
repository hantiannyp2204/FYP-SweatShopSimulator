using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Event Data", fileName = "E_")]
public class FeedbackEventData : ScriptableObject
{
    public string feedbackName;
    public AUDIO_TYPE audioType;
    [Header("AUDIO SETTINGS")]
    [SerializeField] private List<AudioClip> audios;
    public List<AudioClip> Audios => audios;
    public bool sendToAll = false;
    public bool fadeoutAudio = false;
    public bool loop = false;
    public bool stopMusic = false;
    public bool stopAudioInChildren = false;
    public bool stopLoopingInChildren = false;
    public int musicPosition = -1;

    [Header("EFFECT SETTINGS")]
    [SerializeField] private List<EFFECT_TYPE> effects;
    public List<EFFECT_TYPE> Effects => effects;
    [SerializeField] private bool effectSetParent; // set effect on position

    public System.Action eventToInvoke;
    Vector3 posData = Vector3.zero;
    Quaternion rotData = Quaternion.identity;
    Transform parentData = null;
    public void InvokeEvent()
    {
        eventToInvoke?.Invoke();
    }

    public void InvokeEvent(Vector3 posData, Quaternion rotData, Transform parentData = null)
    {
        this.posData = posData;
        this.rotData = rotData;
        this.parentData = parentData;

        eventToInvoke?.Invoke();
    }

    public void InvokeEvent(AUDIO_TYPE audioType, Vector3 posData, Quaternion rotData, Transform parentData = null)
    {
        this.audioType = audioType;
        InvokeEvent(posData, rotData, parentData);
    }

    public void InvokeEvent(AUDIO_TYPE audioType)
    {
        this.audioType = audioType;
        eventToInvoke?.Invoke();
        InvokeEvent(posData, rotData, parentData);
    }

    public void SubscribeEvent(System.Action action)
    {
        eventToInvoke += action;
    }
    public void UnsubscribeEvent(System.Action action)
    {
        eventToInvoke -= action;
    }

    public Vector3 GetPositionData()
    {
        return posData;
    }

    public Quaternion GetRotationData()
    {
        return rotData;
    }

    public Transform GetParentData()
    {
        return parentData;
    }
}

public enum AUDIO_TYPE
{
    SFX,
    MUSIC,
    GLOBAL,
    NONE,
}
public enum EFFECT_TYPE
{
    // Add effects here
    NONE,
}