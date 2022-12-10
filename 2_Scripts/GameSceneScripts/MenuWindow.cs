using System.Collections;
using UnityEngine;

public class MenuWindow : MonoBehaviour
{
    public void OnClickResumeButton()
    {
        if(SceneControlManager._instance._curScene == DefineUtils.eSceneType.TpvGameScene)
        {
            PlayerController pc = GameObject.FindGameObjectWithTag("PlayerController").GetComponent<PlayerController>();
            pc.SetMove();
        }
        else if (SceneControlManager._instance._curScene == DefineUtils.eSceneType.FpvGameScene)
        {
            FPSController pc = GameObject.FindGameObjectWithTag("PlayerController").GetComponent<FPSController>();
            pc.SetMove();
            pc._isPaused = false;
        }
        
        Destroy(gameObject);
    }
    public void OnClickSettingsButton()
    {
        Instantiate(WindowManager._instance.GetPrefabs(DefineUtils.ePrefabs.SettingsWindow));
    }
    public void OnClickExitButton()
    {
        if (SceneControlManager._instance._curScene == DefineUtils.eSceneType.TpvGameScene)
        {
            StartCoroutine(ExitMessage("시작화면으로 이동하시겠습니까?",1));
        }
        else if (SceneControlManager._instance._curScene == DefineUtils.eSceneType.FpvGameScene)
        {
            StartCoroutine(ExitMessage("미션을 종료하시겠습니까?",2));
        }
    }

    IEnumerator ExitMessage(string content, int sceneNo)
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
            if(sceneNo == 1)
                SceneControlManager._instance.StartTitleScene();
            else if (sceneNo == 2)
                SceneControlManager._instance.StartTpvGameScene();

        }
        else
        {
            Destroy(go);
        }
    }
}
