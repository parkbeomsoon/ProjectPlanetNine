using UnityEngine;

public class SFXSettingManager : MonoBehaviour
{
    
    void Start()
    {
        AudioManager._instance.LoadVolumeSFX();
    }
    
}
