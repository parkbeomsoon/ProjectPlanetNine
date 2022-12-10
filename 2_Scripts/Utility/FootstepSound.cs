using System.Collections;
using UnityEngine;

public class FootstepSound : MonoBehaviour
{
    [SerializeField] AudioClip[] _stepSounds;
    [SerializeField] AudioSource _stepSource;

    FPSController _character;

    int nowStepIdx = 0;

    bool _nowPlaying = false;

    void Start()
    {
        _character = FindObjectOfType<FPSController>();
    }

    void Update()
    {
        if (_character._isWalk && !_nowPlaying) StartCoroutine(PlayStepSound());
    }

    public IEnumerator PlayStepSound()
    {
        _nowPlaying = true;
        while (_character._isWalk)
        {
            _stepSource.clip = _stepSounds[nowStepIdx++];
            _stepSource.Play();
            if (nowStepIdx > 1) nowStepIdx = 0;
            yield return new WaitForSeconds(_stepSource.clip.length);
        }
        _nowPlaying = false;
    }
}
