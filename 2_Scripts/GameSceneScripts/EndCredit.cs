using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class EndCredit : MonoBehaviour
{
    [SerializeField] Text _endText;
    float _alphaVal = 0;
    bool _alphaOn = false;

    void Update()
    {
        if (gameObject.activeSelf && !_alphaOn)
        {
            if(_alphaVal >= 1)
            {
                StartCoroutine(Ending());
                _alphaOn = true;
            }
            FindObjectOfType<PlayerController>().transform.position += Vector3.up;
            GetComponent<Image>().color = new Color(0, 0, 0, _alphaVal += Time.deltaTime/5);
        }
    }

    IEnumerator Ending()
    {
        _endText.gameObject.SetActive(true);
        DataManager._instance.RemoveCharacterData();
        yield return new WaitForSeconds(2f);
        SceneControlManager._instance.StartTitleScene();
    }
}
