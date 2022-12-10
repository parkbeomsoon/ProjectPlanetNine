using UnityEngine;

public class AttackSound : MonoBehaviour
{
    [SerializeField] AudioSource _audio;

    FPSController _character;

    void Start()
    {
        _character = FindObjectOfType<FPSController>();
    }

    void Update()
    {
        if (_character._isAttack)
        {
            if (!_audio.isPlaying) 
            {
                float startTime = _audio.clip.length / 2;
                _audio.time = startTime;
                _audio.Play(); 
            }

        }
        else
        {
            if(_audio.isPlaying) _audio.Stop();
        }
    }
}
