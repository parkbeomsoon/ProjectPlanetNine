using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class NewCharacterWindow : MonoBehaviour
{
    [SerializeField] RenderTexture[] _characterRenderTextures;
    [SerializeField] RawImage _characterRawImg;
    [SerializeField] Animator[] _characters;
    [SerializeField] InputField _nameText;
    [SerializeField] Text _characterExplainText;

    int _characterId = 0;

    void Start()
    {
        SetExplainText(_characterId);
        StartCoroutine(SceneControlManager._instance.FadeWindow());
    }

    void Update()
    {
    }
    public void OnClickNextCharacterButton()
    {
        _characterId++;
        if (_characterId > _characterRenderTextures.Length - 1) _characterId = 0;

        _characterRawImg.texture = _characterRenderTextures[_characterId];
        _characters[_characterId].Rebind();
        SetExplainText(_characterId);
    }
    public void OnClickStartButton()
    {
        StartCoroutine(StartMessage("시작하시겠습니까?"));
    }
    public void OnClickPrevButton()
    {
        StartCoroutine(PrevMessage("생성을 취소하시겠습니까?"));
    }
    void SetExplainText(int characterNo)
    {
        string totalText = string.Empty;

        totalText += string.Format($"코드명 : {DefineUtils.CharacterInfo.name[characterNo]}\n");
        totalText += string.Format($"나이 : {DefineUtils.CharacterInfo.age[characterNo]}\n");
        totalText += string.Format($"특화 : {DefineUtils.CharacterInfo.specialize[characterNo]}\n");
        totalText += string.Format($"설명 : {DefineUtils.CharacterInfo.explain[characterNo]}\n");

        _characterExplainText.text = totalText;
    }
    IEnumerator StartMessage(string content)
    {
        GameObject go = Instantiate(WindowManager._instance.GetPrefabs(DefineUtils.ePrefabs.MessageBox));
        MessageBox msg = go.GetComponent<MessageBox>();

        msg.SetText(string.Format($"{content}"));

        while (!msg._clicked)
        {
            yield return new WaitForSeconds(0.1f);
        }
        if (msg._isConfirm)
        {
            Destroy(go);
            DataManager._instance.MakeData(_nameText.text, _characterId);
            SceneControlManager._instance.StartTpvGameScene();
        }
        else
        {
            Destroy(go);
        }
    }

    IEnumerator PrevMessage(string content)
    {
        GameObject go = Instantiate(WindowManager._instance.GetPrefabs(DefineUtils.ePrefabs.MessageBox));
        MessageBox msg = go.GetComponent<MessageBox>();

        msg.SetText(string.Format($"{content}"));

        while (!msg._clicked)
        {
            yield return new WaitForSeconds(0.1f);
        }
        if (msg._isConfirm)
        {
            Destroy(go);
            Destroy(gameObject);
        }
        else
        {
            Destroy(go);
        }
    }
}
