using UnityEngine;

public class SkyFlow : MonoBehaviour
{
    public float _rotateSpeed = 1;
    public float _rotateDistance = 1;
    public Transform player;

    Vector2 ssgVector;
    Transform tr;

    void Start()
    {
        ssgVector = Vector2.zero;
        tr = transform;
    }

    // Update is called once per frame
    void Update()
    {
        ssgVector = Quaternion.AngleAxis(Time.time * _rotateSpeed, Vector3.forward) * Vector2.one * _rotateDistance;
        Shader.SetGlobalFloat("_SkyShaderUvX", ssgVector.x);
        Shader.SetGlobalFloat("_SkyShaderUvZ", ssgVector.y);

        if (player != null) tr.position = new Vector3(player.position.x, tr.position.y, player.position.z);
    }
}
