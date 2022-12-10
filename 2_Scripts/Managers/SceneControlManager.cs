using DefineUtils;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneControlManager : Singleton<SceneControlManager>
{
    #region 맵 정보 데이터
    int _mapNo = 0;
    #endregion

    eSceneType _nowSceneType;
    eSceneType _oldSceneType;

    public eSceneType _curScene
    {
        get { return _nowSceneType; }
    }

    protected override void Init()
    {
        base.Init();
        _oldSceneType = _nowSceneType = eSceneType.TitleScene;
    }

    public void StartTitleScene()
    {
        _oldSceneType = _nowSceneType;
        _nowSceneType = eSceneType.TitleScene;

        StartCoroutine(LoadingScene(_nowSceneType));
    }

    public void StartTpvGameScene()
    {
        _oldSceneType = _nowSceneType;
        _nowSceneType = eSceneType.TpvGameScene;
        
        StartCoroutine(LoadingScene(_nowSceneType));
    }

    public void StartFpvGameScene()
    {
        _oldSceneType = _nowSceneType;
        _nowSceneType = eSceneType.FpvGameScene;

        StartCoroutine(LoadingScene(_nowSceneType));
    }

    IEnumerator LoadingScene(eSceneType scene)
    {
        GameObject go = Instantiate(WindowManager._instance.GetPrefabs(ePrefabs.LoadingWindow));
        LoadingWindow lw = go.GetComponent<LoadingWindow>();
        lw.InitWindow();

        AudioSource[] audios = FindObjectsOfType<AudioSource>();
        foreach (AudioSource a in audios) a.Stop();

        AsyncOperation aOper = SceneManager.LoadSceneAsync(scene.ToString());
        aOper.allowSceneActivation = false;

        float timer = 0f;
        float defaultLoadingSpeed = 2f;

        while (!aOper.isDone)
        {
            yield return null;
            lw.SetProgress(timer += (Time.deltaTime/ defaultLoadingSpeed));
            if (aOper.progress >= 0.9f && lw.GetProgress() == 1) aOper.allowSceneActivation = true;
        }
    }
    public IEnumerator FadeWindow()
    {
        GameObject go = Instantiate(WindowManager._instance.GetPrefabs(ePrefabs.FadeWindow));
        go.GetComponent<Canvas>().worldCamera = Camera.main;
        Image shadow = go.transform.GetChild(0).GetComponent<Image>();

        float alphaVal = 1;
        float offset = 0.01f;

        while (alphaVal > 0)
        {
            alphaVal -= offset;
            shadow.color = new Color(0, 0, 0, alphaVal);
            yield return new WaitForEndOfFrame();
        }

        Destroy(go);

        yield return null;
    }
    public IEnumerator WindowGrow(RectTransform bg)
    {
        float size = 0;
        float offset = 0.1f;
        bg.localScale = Vector3.zero;
        while (bg.localScale.x < 1)
        {
            size += offset;
            bg.localScale = new Vector3(size, size, size);
            yield return new WaitForEndOfFrame();
        }
        yield return null;
    }

    public void SetMap(int mapNo)
    {
        _mapNo = mapNo;
    }
}
