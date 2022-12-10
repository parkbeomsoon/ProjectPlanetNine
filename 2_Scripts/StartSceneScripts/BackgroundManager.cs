using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class BackgroundManager : MonoBehaviour
{
    [SerializeField] Text _titleText;
    [SerializeField] GameObject _btnObj;

    bool _titleRevealed = false;

    Color _titleColor;

    void Start()
    {
        _titleColor = _titleText.color;
        _titleText.color = new Color(_titleColor.r, _titleColor.g, _titleColor.b, 0);
        _btnObj.SetActive(false);
        StartCoroutine(FadeTitle(3));
    }

    void Update()
    {
    }

    IEnumerator FadeTitle(float delayTime)
    {
        float alpha = 0;
        float revealSpeed = 3f;
        _titleText.GetComponent<Outline>().enabled = false;
        StartCoroutine(FadeOutline());
        while (!_titleRevealed)
        {
            if (delayTime > 0)
            {
                delayTime -= Time.deltaTime;
            }
            else
            {
                alpha += Time.deltaTime / revealSpeed;
                _titleText.color = new Color(_titleColor.r, _titleColor.g, _titleColor.b, alpha);
                if (alpha >= 1)
                {
                    _titleText.color = _titleColor;
                    _titleRevealed = true;
                }
            }
            yield return new WaitForEndOfFrame();
        }
        yield return null;
    }
    IEnumerator FadeOutline()
    {
        float val = 35;
        float minVal = 5f;
        Outline ol = _titleText.GetComponent<Outline>();
        bool isDone = false;
        float appearSpeed = 10f;
        ol.enabled = true;
        while (!isDone)
        {
            val -= Time.deltaTime * appearSpeed;
            ol.effectDistance = new Vector2(val, val);
            if (val <= minVal)
            {
                ol.effectDistance = new Vector2(minVal, minVal);
                _btnObj.SetActive(true);
                isDone = true;
            }
            yield return new WaitForEndOfFrame();
        }
    }
}
