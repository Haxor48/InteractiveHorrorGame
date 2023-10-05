using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class AudioHandler : MonoBehaviour
{
    private GameObject player;
    private MicSensor mic;
    private Dictionary<int, Dictionary<int, int>> volumes = new Dictionary<int, Dictionary<int, int>>();
    private Dictionary<int, Dictionary<int, float>> noiseStart = new Dictionary<int, Dictionary<int, float>>();
    private Dictionary<int, Dictionary<int, float>> clipLengths = new Dictionary<int, Dictionary<int, float>>();

    private double distanceTo(int[] loc1, int[] loc2)
    {
        return Math.Sqrt(Math.Pow(1.0 * (loc1[0] - loc2[0]), 2.0) + Math.Pow(1.0 * (loc1[1] - loc2[1]), 2.0));
    }
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("PlayerCapsule");
        mic = player.GetComponent<MicSensor>();
    }

    // Update is called once per frame
    void Update()
    {
        getPlayerVol();
        updateVolumes();
    }

    public float playerVol()
    {
        return mic.clipLoudness;
    }

    public int[] getMaxVolPos(int[] pos)
    {
        int[] maxLoc = { -1, -1 };
        foreach (KeyValuePair<int, Dictionary<int, int>> i in volumes)
        {
            foreach (KeyValuePair<int, int> c in i.Value)
            {
                int[] temp = { i.Key, c.Key };
                if (maxLoc[0] == -1 || volumes[i.Key][c.Key] > volumes[maxLoc[0]][maxLoc[1]] ||
                    (volumes[i.Key][c.Key] == volumes[maxLoc[0]][maxLoc[1]] && distanceTo(pos, maxLoc) > distanceTo(pos, temp)))
                {
                    maxLoc[0] = i.Key;
                    maxLoc[1] = c.Key;
                }
            }
        }
        return maxLoc;
    }

    public void addSound(int[] location, int volume, float length)
    {
        volumes.Add(location[0], new Dictionary<int, int>());
        noiseStart.Add(location[0], new Dictionary<int, float>());
        clipLengths.Add(location[0], new Dictionary<int, float>());
        bool canAdd = true;
        try
        {
            volumes[location[0]].Add(location[1], volume);
        }
        catch (ArgumentException)
        {
            canAdd = false;
        }
        if (!canAdd)
        {
            volumes[location[0]][location[1]] = volume;
            noiseStart[location[0]][location[1]] = 0.0f;
            clipLengths[location[0]][location[1]] = length;
        }
        else
        {
            noiseStart[location[0]].Add(location[1], 0.0f);
            clipLengths[location[0]].Add(location[1], length);
        }
    }

    public void getPlayerVol()
    {
        if (!Microphone.IsRecording(mic.name) && mic.CURR_AUDIO_CLIP != null)
        {
            int level = (int)(5 * mic.CURR_AUDIO_CLIP.volume);
            int[] location = { (int)player.transform.position.x, (int)player.transform.position.z };
            addSound(location, level, 1.0f);
        }
    }

    private void updateVolumes()
    {
        foreach (KeyValuePair<int, Dictionary<int, int>> i in volumes)
        {
            foreach (KeyValuePair<int, int> c in i.Value)
            {
                noiseStart[i.Key][c.Key] += Time.deltaTime;
                if (noiseStart[i.Key][c.Key] >= 3.0f)
                {
                    volumes[i.Key].Remove(c.Key);
                    noiseStart[i.Key].Remove(c.Key);
                    if (volumes[i.Key].Count <= 0)
                    {
                        volumes.Remove(i.Key);
                        noiseStart.Remove(i.Key);
                    }
                }
            }
        }
    }
}