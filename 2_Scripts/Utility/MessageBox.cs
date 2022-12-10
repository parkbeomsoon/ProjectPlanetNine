using UnityEngine;
using UnityEngine.UI;

public class MessageBox : MonoBehaviour
{
    [SerializeField] Text _contentText;

    public bool _isConfirm
    {
        get;set;
    }
    public bool _clicked
    {
        get;set;
    }

    void Start()
    {
        _isConfirm = false;
        _clicked = false;
    }
    public void SetText(string text)
    {
        _contentText.text = text;
    }

    public void OnClickCancelButton()
    {
        _clicked = true;
        _isConfirm = false;
    }
    public void OnClickConfirmButton()
    {
        _clicked = true;
        _isConfirm = true;
    }
}
