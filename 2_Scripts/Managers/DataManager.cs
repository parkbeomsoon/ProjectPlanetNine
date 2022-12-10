using DefineUtils;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class DataManager : Singleton<DataManager>
{
    public float _endMissionLevel = 3;
    #region 캐릭터 데이터


    #region[파일 변수]

    string path = "/PlanetNineAppData.dat";
    public DataClass dataClass;

    public string GetDataPath
    {
        get { return path; }
    }
    #endregion

    #region[기본 세팅]

    protected override void Init()
    {
        base.Init();
        InitDefaultMapData();
    }
    public void MakeData(string name, int characterId)
    {
        dataClass._hp = 10;
        dataClass._name = name;
        dataClass._progress = 0;
        dataClass._characterNumber = characterId;
        dataClass._characterPosZ = -4360f;
        SaveData();
    }
    void InitKeyOptionsSet()
    {
        KeyOption tempOption = new KeyOption();

        //기본값
        tempOption._option = eKeyOption.Interact;
        tempOption._keyCode = KeyCode.F;
        dataClass.options.keyOptions.Add(tempOption);

        tempOption = new KeyOption();
        tempOption._option = eKeyOption.SwapWeapon1;
        tempOption._keyCode = KeyCode.Alpha1;
        dataClass.options.keyOptions.Add(tempOption);

        tempOption = new KeyOption();
        tempOption._option = eKeyOption.SwapWeapon2;
        tempOption._keyCode = KeyCode.Alpha2;
        dataClass.options.keyOptions.Add(tempOption);

    }
    void InitSoundOptionsSet()
    {
        SoundOption tempOption = new SoundOption();

        tempOption._option = eSoundOption.Background;
        tempOption._value = 0.1f;
        dataClass.options.soundOptions.Add(tempOption);

        tempOption = new SoundOption();
        tempOption._value = 0.1f;
        tempOption._option = eSoundOption.Effect;
        dataClass.options.soundOptions.Add(tempOption);
    }
    #endregion

    #region 데이터 클래스
    [System.Serializable]
    public class DataClass
    {
        public string _name = string.Empty;
        public int _characterNumber = 0;
        public int _hp = 0;
        public int _progress = 0;
        public float _characterPosZ = 0;
        public OptionsData options = new OptionsData();
    }
    [System.Serializable]
    public class OptionsData
    {
        public List<KeyOption> keyOptions = new List<KeyOption>();
        public List<SoundOption> soundOptions = new List<SoundOption>();
    }
    [System.Serializable]
    public class KeyOption
    {
        public eKeyOption _option;
        public KeyCode _keyCode;
    }
    [System.Serializable]
    public class SoundOption
    {
        public eSoundOption _option;
        public float _value;
    }
    #endregion

    #region[데이터 저장]

    public void SaveData()
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream fs = File.Open(path,FileMode.OpenOrCreate);

        bf.Serialize(fs, dataClass);
        fs.Close();
    }

    #endregion

    #region[데이터 불러오기]

    public void LoadData()
    {
        if (File.Exists(path))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream fs = File.Open(path, FileMode.Open);
            DataClass data = (DataClass)bf.Deserialize(fs);
            fs.Close();

            dataClass =  data;
        }
        else
        {
            DataClass data = new DataClass();
            dataClass = data;
            
            //기본세팅실행
            InitKeyOptionsSet();
            InitSoundOptionsSet();
        }
    }

    #endregion

    #region [데이터 삭제]

    #region 캐릭터데이터 삭제

    public void RemoveCharacterData()
    {
        OptionsData prevData = dataClass.options;
        File.Delete(path);

        DataClass newData = new DataClass();
        newData.options = prevData;
    }

    #endregion

    #endregion

    public bool DataExist()
    {
        if (File.Exists(path))
        {
            return true;
        }
        else return false;
    }

    #endregion

    #region 맵 데이터

    public List<Dictionary<eMapObjectKind, int>> _mapList;
    public Dictionary<eMapObjectKind, int> _mapObjectDic;

    void InitDefaultMapData()
    {
        _mapList = new List<Dictionary<eMapObjectKind, int>>();

        _mapObjectDic = new Dictionary<eMapObjectKind, int>();
        _mapObjectDic.Add(eMapObjectKind.Enemys,4);
        _mapObjectDic.Add(eMapObjectKind.Repairs, 0);
        _mapObjectDic.Add(eMapObjectKind.Research, 0);

        _mapList.Add(_mapObjectDic);

        _mapObjectDic = new Dictionary<eMapObjectKind, int>();
        _mapObjectDic.Add(eMapObjectKind.Enemys, 1);
        _mapObjectDic.Add(eMapObjectKind.Repairs, 3);
        _mapObjectDic.Add(eMapObjectKind.Research, 0);

        _mapList.Add(_mapObjectDic);

        _mapObjectDic = new Dictionary<eMapObjectKind, int>();
        _mapObjectDic.Add(eMapObjectKind.Enemys, 2);
        _mapObjectDic.Add(eMapObjectKind.Repairs, 0);
        _mapObjectDic.Add(eMapObjectKind.Research, 3);

        _mapList.Add(_mapObjectDic);

    }

    #endregion
}
