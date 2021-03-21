using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine;
using System;
using UnityEngine.UI;
public class AudioManager : MonoBehaviour
{
    public Sound[] sounds;
    string mainMusic = "MainSceneMusic";
    bool isMainScene = false;
    bool isPause = false;

    static GameObject go;

    public float value = 1f;

    void Awake()
    {
       // Application.targetFrameRate = 60;
        //rm = FindObjectOfType<ReklamManager>();
        DontDestroyOnLoad(gameObject);

        if (go == null)
            go = gameObject;
        else
            Destroy(gameObject);

        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.loop = s.loop;
        }

    }

    private void FixedUpdate()
     {
           

        if(SceneManager.GetActiveScene().buildIndex == 0 && !isMainScene)
        {
            Play("MainSceneMusic");
            Stop("SoundTrack");
            isMainScene = true;
        } else if(SceneManager.GetActiveScene().buildIndex != 0 && (isMainScene || isPause) )
        {
            Play("SoundTrack");
            Stop("MainSceneMusic");
            isMainScene = false;
            isPause = false;
        }
     }
    
    public void Play(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        s.source.Play();
    }

    public void Stop(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        s.source.Stop();
    }

    public void Pause(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        s.source.Pause();
        isPause = true;
    }

    public void Volume(Slider slider)
    {
        value = slider.value;
        foreach (Sound s in sounds)
        {
            if(s.source != null)
                s.source.volume = s.volume * value;
        }
    }





}
