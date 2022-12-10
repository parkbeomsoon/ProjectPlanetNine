using System.Collections;
using UnityEngine;

public class UtilManager : Singleton<UtilManager>
{
    protected override void Init()
    {
        base.Init();
    }

    public IEnumerator MessageBox(GameObject prefab, string contentText, Transform parent)
    {
        Debug.Log(1);
        GameObject go = Instantiate(prefab);
        Debug.Log(2);
        MessageBox msg = go.GetComponent<MessageBox>();
        Debug.Log(3);
        msg.SetText(contentText);
        Debug.Log(4);
        while (!msg._clicked)
        {
            yield return new WaitForSeconds(0.1f);
        }
        Debug.Log(5);
    }
}
