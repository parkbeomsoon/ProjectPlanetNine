using DefineUtils;
using UnityEngine;

public class WindowManager : Singleton<WindowManager>
{
    protected override void Init()
    {
        base.Init();
    }

    #region [타이틀화면]

    public void OpenStartWnd()
    {

        if (GameObject.FindGameObjectWithTag("StartWindow") == null)
        {
            GameObject go = Instantiate(GetPrefabs(ePrefabs.StartWindow));
            go.GetComponent<Canvas>().worldCamera = Camera.main;
        }
    }
    public void OpenNewCharacterWnd()
    {
        if (GameObject.FindGameObjectWithTag("NewCharacterWindow") == null)
        {
            GameObject go = Instantiate(GetPrefabs(ePrefabs.NewCharacterWindow));
            go.GetComponent<Canvas>().worldCamera = Camera.main;
        }
    }
    #endregion

    public GameObject GetPrefabs(ePrefabs pf)
    {
        return Resources.Load<GameObject>("Prefabs/Windows/" + pf.ToString());
    }
}
