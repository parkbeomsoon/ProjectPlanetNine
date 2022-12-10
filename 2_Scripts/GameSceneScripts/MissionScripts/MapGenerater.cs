using UnityEngine;
using UnityEngine.AI;
using DefineUtils;

public class MapGenerater : MonoBehaviour
{
    [SerializeField] GameObject[] _maps;
    [SerializeField] GameObject[] _objects;
    [SerializeField] GameObject _sky;
    [SerializeField] MissionExplainWindow _exWindow;

    bool _isGen = false;
    FPSController _player;

    void Awake()
    {
        _player = GameObject.FindGameObjectWithTag("PlayerController").GetComponent<FPSController>();
    }

    void Update()
    {
        if(!_isGen)
        {
            GenerateMap();
        }    
    }

    void GenerateMap()
    {
        int progress = DataManager._instance.dataClass._progress;
        _exWindow.SetMissionContent(progress);
        GameObject go = Instantiate(_maps[progress], transform);

        _player.transform.position = GameObject.FindGameObjectWithTag("PlayerSpawnPos").transform.position;
        _player.transform.rotation = GameObject.FindGameObjectWithTag("PlayerSpawnPos").transform.rotation;
        gameObject.GetComponent<NavMeshSurface>().RemoveData();
        gameObject.GetComponent<NavMeshSurface>().BuildNavMesh();

        int enemyCnt = DataManager._instance._mapList[progress][eMapObjectKind.Enemys];
        int repairCnt = DataManager._instance._mapList[progress][eMapObjectKind.Repairs];
        int researchCnt = DataManager._instance._mapList[progress][eMapObjectKind.Research];

        GameObject.FindObjectOfType<MissionManager>().SetTarget(enemyCnt, repairCnt, researchCnt);
        _isGen = true;
        GenerateObject(go, enemyCnt, repairCnt, researchCnt);
    }

    void GenerateObject(GameObject mapObject, int enemyCnt, int repairCnt, int researchCnt)
    {
        GameObject enemyPosObj = mapObject.transform.Find(eMapObjectKind.Enemys.ToString()).gameObject;
        GameObject repairPosObj = mapObject.transform.Find(eMapObjectKind.Repairs.ToString()).gameObject;
        GameObject researchPosObj = mapObject.transform.Find(eMapObjectKind.Research.ToString()).gameObject;

        GameObject go = Resources.Load<GameObject>("Prefabs/Objects/Enemys/"+ eEnemyKind.Mutant.ToString());
        for (int i = 0; i < enemyCnt; i++)
            Instantiate(go, enemyPosObj.transform.GetChild(i));

        go = Resources.Load<GameObject>("Prefabs/Objects/Repairs/" + eRepairKind.BrokenDrone.ToString());
        for (int i = 0; i < repairCnt; i++)
            Instantiate(go, repairPosObj.transform.GetChild(i));

        go = Resources.Load<GameObject>("Prefabs/Objects/Research/" + eResearchKind.Blood.ToString());
        for (int i = 0; i < researchCnt; i++)
            Instantiate(go, researchPosObj.transform.GetChild(i));

        _sky.SetActive(true);
    }

}
