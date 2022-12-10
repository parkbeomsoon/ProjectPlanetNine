using UnityEngine;
using DefineUtils;

public class ObjectStatus : MonoBehaviour
{
    public eInteractKind _objectKind;
    public bool _isSuccess = false;

    void Awake()
    {
        if (gameObject.tag.Equals("RepairObj"))
            _objectKind = eInteractKind.수리;
        else if (gameObject.tag.Equals("ResearchObj"))
            _objectKind = eInteractKind.조사;
    }
}
