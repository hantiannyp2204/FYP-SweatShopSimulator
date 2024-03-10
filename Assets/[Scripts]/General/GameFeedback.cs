using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


//Inspired by Raqib
public class GameFeedback : MonoBehaviour
{
    [SerializeField] private List<FeedbackEventData> feedbackDatas = new List<FeedbackEventData>();
    [Header("AUDIO POOLING")]
    [SerializeField] private AudioPool[] AudioPooling = new AudioPool[(int)AUDIO_TYPE.NONE];
    [Header("EFFECT POOLING")]
    [SerializeField] private EffectPool[] EffectsPooling = new EffectPool[(int)EFFECT_TYPE.NONE];
    string poolTag = "Audio Pool";

    public void InIt()
    {
        // Initialize pool
        foreach (AudioPool audioP in AudioPooling)
            audioP.pool.Initialize(poolTag);
        foreach (EffectPool effectP in EffectsPooling)
            effectP.pool.Initialize(poolTag);

        // subscribe events for feedback datas
        foreach (FeedbackEventData feedback in feedbackDatas)
        {
            //Debug.Log(feedback.feedbackName);
            feedback.SubscribeEvent(() => Callback(feedback));
        }
    }

    void Callback(FeedbackEventData data)
    {
        // Store the nessecery data in variable
        List<AudioClip> audioClips = data.Audios;
        int maxAudioClip = data.Audios.Count;

        // parentData not null
        if (data.GetParentData() != null)
        {
            AudioSource[] sources = data.GetParentData().gameObject.GetComponentsInChildren<AudioSource>();
            foreach (var source in sources)
            {
                if (!source.gameObject.CompareTag(poolTag)) continue;

                if (data.stopAudioInChildren)
                {
                    source.Stop();
                    source.clip = null;
                    source.gameObject.SetActive(false);
                    source.transform.SetParent(null);
                }
                if (data.stopLoopingInChildren)
                {
                    source.loop = false;
                }
            }
        }

        // Play audio
        if (maxAudioClip > 0)
        {
            data.musicPosition = AudioPooling[(int)data.audioType].pool.Index;
            GameObject audioObj = AudioPooling[(int)data.audioType].GetAudio(data);
            // Get the audio source
            AudioSource audioS = audioObj.GetComponent<AudioSource>();
            // play audio at random
            switch (data.audioType)
            {
                case AUDIO_TYPE.SFX:
                    audioS.clip = audioClips[UnityEngine.Random.Range(0, maxAudioClip)];
                    audioS.Play();
                    break;
                case AUDIO_TYPE.MUSIC:
                    audioS.Stop();
                    audioS.clip = (audioClips[UnityEngine.Random.Range(0, maxAudioClip)]);
                    audioS.Play();
                    break;
                case AUDIO_TYPE.GLOBAL:
                    audioS.spatialBlend = 0;
                    audioS.clip = audioClips[UnityEngine.Random.Range(0, maxAudioClip)];
                    audioS.Play();
                    break;
                case AUDIO_TYPE.NONE:
                    break;
                default:
                    break;
            }
            if (!data.loop)
                StartCoroutine(DisableAudio(audioS, data.fadeoutAudio));
        }
        // Play Effects
        foreach (var effect in data.Effects)
        {
            GameObject effectObj = EffectsPooling[(int)effect].GetEffect(data);
        }
    }

    // Disable audio
    IEnumerator DisableAudio(AudioSource audioS, bool fade)
    {
        // eg 3 and length = 10
        // time = (3-3) / (10-3)
        float beginTime = audioS.time;
        while (audioS.isPlaying)
        {
            if (audioS == null)
                break;
            // fade
            if (fade)
                audioS.volume = 1 - ((audioS.time - beginTime) / (audioS.clip.length - beginTime));
            //Debug.Log("is playing");
            yield return null;
        }
        audioS.gameObject.SetActive(false);
        audioS.transform.SetParent(null);
    } 
}
[System.Serializable]
public class EffectPool
{
    [SerializeField] private EFFECT_TYPE effectType;
    [SerializeField] private Pool effectPool;
    public Pool pool => effectPool;

    public GameObject GetEffect(Vector3 position, Quaternion rotation, Transform parent = null)
    {
        return effectPool.GetObj(position, rotation, parent);
    }
    public GameObject GetEffect(FeedbackEventData data)
    {
        return effectPool.GetObj(data.GetPositionData(), data.GetRotationData(), data.GetParentData());
    }
}
[System.Serializable]
public class AudioPool
{
    [SerializeField] private AUDIO_TYPE audio_type;
    [SerializeField] private Pool audioPool;
    public Pool pool => audioPool;

    public GameObject GetAudio(Vector3 position, Quaternion rotation, Transform parent = null)
    {
        return audioPool.GetObj(position, rotation, parent);
    }
    public GameObject GetAudio(FeedbackEventData data)
    {
        return audioPool.GetObj(data.GetPositionData(), data.GetRotationData(), data.GetParentData());
    }
}
[System.Serializable]
public class Pool
{
    public GameObject obj;
    [SerializeField] private int maxPool = 5;
    [SerializeField] private List<GameObject> list_of_obj_stored = new List<GameObject>();
    int index = 0;
    public int Index => index;

    public void Initialize(string tag = "")
    {
        // Instante obj and set inactive
        for (int i = 0; i < maxPool; i++)
        {
            GameObject objPool = null;
            objPool = GameObject.Instantiate(obj, Vector3.zero, Quaternion.identity);
            list_of_obj_stored.Add(objPool);
            objPool.gameObject.SetActive(false);
            objPool.gameObject.tag = tag;
        }
    }
    // Get objectfrom pool
    public GameObject GetObj(Vector3 position, Quaternion rotation, Transform parent = null)
    {
        int tmp = 0;
        GameObject objToGet = null;
        while (objToGet == null)
        {
            if (tmp >= maxPool)
                break;

            objToGet = list_of_obj_stored[index];

            tmp++;
            index++;
        }
        if (objToGet == null)
        {
            list_of_obj_stored.RemoveAt(index);
        }
        if (objToGet != null)
        {
            objToGet.gameObject.SetActive(true);

            objToGet.gameObject.transform.position = position;
            objToGet.gameObject.transform.rotation = rotation;

            if (parent)
                objToGet.transform.SetParent(parent);

        }
        else
        {
            Debug.Log("object sound empty " + index);
        }

        if (index >= maxPool)
            index = 0;

        return objToGet;
    }
}
