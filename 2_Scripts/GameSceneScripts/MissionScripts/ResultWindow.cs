using UnityEngine;
using UnityEngine.UI;

public class ResultWindow : MonoBehaviour
{
    Text _resultText;

    void Awake()
    {
        _resultText = transform.GetChild(0).GetComponent<Text>();
    }

    public void SetResultText(string text)
    {
        _resultText.text = text;
    }

}
