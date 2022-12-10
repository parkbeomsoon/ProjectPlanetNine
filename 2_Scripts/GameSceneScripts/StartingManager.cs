using UnityEngine;
using UnityStandardAssets.ImageEffects;

public class StartingManager : MonoBehaviour
{
    public float _blurSpeed = 1;

    private int _chromaticSpeed = 100;
    private float _chromaticVal = -100f;
    private float _blurVal = 1f;
    private int _endVal = 0;


    void Start()
    {
        Camera.main.GetComponent<VignetteAndChromaticAberration>().SetBlur(_blurVal);
        Camera.main.GetComponent<VignetteAndChromaticAberration>().SetChromaticAberration(_chromaticVal);
        AudioManager._instance.PlayAudioClipBGM();
    }

    void Update()
    {
        if (_chromaticVal < _endVal)
        {
            _chromaticVal += Time.deltaTime * _chromaticSpeed;
            Camera.main.GetComponent<VignetteAndChromaticAberration>().SetChromaticAberration(_chromaticVal);
        }
        else
        {
            _chromaticVal = _endVal;
            Camera.main.GetComponent<VignetteAndChromaticAberration>().SetChromaticAberration(_chromaticVal);
        }

        if(_chromaticVal == _endVal)
        {
            if (_blurVal > 0)
            {
                _blurVal -= Time.deltaTime * _blurSpeed;
                Camera.main.GetComponent<VignetteAndChromaticAberration>().SetBlur(_blurVal);
            }
            else 
            {
                _blurVal = _endVal;
                Camera.main.GetComponent<VignetteAndChromaticAberration>().SetBlur(_blurVal);
            }

        }

        if (_chromaticVal == _endVal && _blurVal == _endVal)
        {
            Destroy(gameObject);
        }
    }
}
