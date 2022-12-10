using DefineUtils;
using UnityEngine;

public class ButtonSound : MonoBehaviour
{
    public void PlayButtonEnterSound()
    {
        AudioSource _as = GetComponent<AudioSource>();
        _as.clip = Resources.Load<AudioClip>("Sounds/SFX/ButtonEnter");
        _as.volume = DataManager._instance.dataClass.options.soundOptions[(int)eSoundOption.Effect]._value;
        _as.Play();
    }
    public void PlayButtonClickSound()
    {
        AudioSource _as = GetComponent<AudioSource>();
        _as.clip = Resources.Load<AudioClip>("Sounds/SFX/ButtonClick");
        _as.volume = DataManager._instance.dataClass.options.soundOptions[(int)eSoundOption.Effect]._value;
        _as.Play();
    }
}
