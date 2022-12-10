using DefineUtils;
using System.Collections;
using UnityEngine;

public class AudioManager : Singleton<AudioManager>
{
    protected override void Init()
    {
        base.Init();
    }

    AudioSource _audioSoruce;

    public void PlayAudioClipBGM()
    {
        _audioSoruce = Camera.main.GetComponent<AudioSource>();
        AudioClip clip = Resources.Load<AudioClip>("Sounds/" + SceneControlManager._instance._curScene.ToString() + "BGM");
        _audioSoruce.clip = clip;
        _audioSoruce.volume = DataManager._instance.dataClass.options.soundOptions[(int)eSoundOption.Background]._value;
        _audioSoruce.Play();
    }
    public void PlayEndingBGM()
    {
        _audioSoruce = Camera.main.GetComponent<AudioSource>();
        AudioClip clip = Resources.Load<AudioClip>("Sounds/EndingBGM");
        _audioSoruce.clip = clip;
        _audioSoruce.volume = DataManager._instance.dataClass.options.soundOptions[(int)eSoundOption.Background]._value;
        _audioSoruce.Play();
    }
    public void SetVolumeBGM(float volume)
    {
        _audioSoruce = Camera.main.GetComponent<AudioSource>();
        _audioSoruce.volume = volume;
    }
    public void LoadVolumeBGM()
    {
        _audioSoruce = Camera.main.GetComponent<AudioSource>();
        _audioSoruce.volume = DataManager._instance.dataClass.options.soundOptions[(int)eSoundOption.Background]._value;
    }
    public void LoadVolumeSFX()
    {
        SFXSettingManager[] goList = FindObjectsOfType<SFXSettingManager>();
        for(int i = 0; i < goList.Length; i++)
        {
            AudioSource[] _audioSources = goList[i].gameObject.GetComponents<AudioSource>();
            for(int j = 0; j < _audioSources.Length; j++)
            {
                _audioSoruce = _audioSources[j];
                _audioSoruce.volume = DataManager._instance.dataClass.options.soundOptions[(int)eSoundOption.Effect]._value;
            }
        }
    }

}
