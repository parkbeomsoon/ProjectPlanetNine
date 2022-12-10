using UnityEngine;

public class BGMSettingManager : MonoBehaviour
{
    void Start()
    {
        AudioManager._instance.LoadVolumeBGM();
    }

}
