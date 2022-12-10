using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class BulletUI : MonoBehaviour
{
    [SerializeField] Text _remainBulltText;
    FPSController _fpsCtrl;

    void Start()
    {
        _fpsCtrl = GameObject.Find("FPSObject").GetComponent<FPSController>();
        StartCoroutine(CheckBullet());
    }

    IEnumerator CheckBullet()
    {
        while (GameObject.Find("FPSObject").GetComponent<FPSController>() != null)
        {
            _remainBulltText.text = _fpsCtrl.getLaserTime.ToString();
            yield return new WaitForSeconds(0.1f);
        }
    }
}
