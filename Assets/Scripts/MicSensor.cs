using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MicSensor : MonoBehaviour
{
    public AudioSource CURR_AUDIO_CLIP;
    public int maxFreq = 0;
    public int minFreq = 0;
    public string name = null;
    public int loopTime = 1;
    private float currentUpdateTime = 0f;
    public float volume = 0f;
    public float totalTime = 0f;

    public float clipLoudness;
    private float[] clipSampleData;
    // Start is called before the first frame update
    void Start()
    {
        CURR_AUDIO_CLIP = GetComponent<AudioSource>();
        Microphone.GetDeviceCaps(name, out minFreq, out maxFreq);
    }

    // Update is called once per frame
    void Update()
    {
        if (totalTime == 0f)
        {
            CURR_AUDIO_CLIP.clip = Microphone.Start(name, false, loopTime, maxFreq);
            //Debug.Log("recording");
        }
        totalTime += Time.deltaTime;
        if (totalTime >= loopTime * 1.0f)
        {
            totalTime = 0f;
            Microphone.End(name);
            clipSampleData = new float[(CURR_AUDIO_CLIP.clip.samples * CURR_AUDIO_CLIP.clip.channels)];
            currentUpdateTime += Time.deltaTime;
            CURR_AUDIO_CLIP.clip.GetData(clipSampleData, 0);
            clipLoudness = 0f;
            for (int i = 0; i < clipSampleData.Length; ++i)
            {
                clipLoudness += Mathf.Abs(clipSampleData[i]);
            }
            clipLoudness /= clipSampleData.Length;
            clipLoudness *= 100;
            //Debug.Log("" + clipLoudness);
            CURR_AUDIO_CLIP.clip = null;
        }
        //}
    }
}