using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using DefineUtils;

public class MinigameWindow : MonoBehaviour
{
    [SerializeField] GameObject[] _minigameObject;
    [SerializeField] Text _resultText;

    public bool _isSuccess = false;
    public bool minigameEnd = false;
    public bool _wndStart = false;
    bool _isStart = false;
    float _spacebarDelayTime = 0f;
    bool _spacebar = false;
    int _directionKey = 0; //1 : 왼쪽 2 : 아래 3 : 오른쪽 4 : 위

    public eInteractKind _interactKind = eInteractKind.None;

    ParticleSystem _keyEffect;
    PlayerStatus _playerStat;
    
    void Start()
    {
        _playerStat = GameObject.FindObjectOfType<PlayerStatus>();
        _resultText.text = string.Empty;
    }
    void Update()
    {
        if(_wndStart && !_isStart && (_interactKind != eInteractKind.None))
        {
            if(_interactKind == eInteractKind.수리)
            {
                int random = Random.Range(0, 2);
                StartCoroutine(MiniGame(random));
            }
            else if (_interactKind == eInteractKind.조사)
            {
                StartCoroutine(MiniGame(2));
            }
        }

        _spacebarDelayTime -= Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.Space) && _spacebarDelayTime <= 0)
        {
            _spacebar = true;
            _spacebarDelayTime = 1f;
        }
        if (Input.GetKeyUp(KeyCode.Space)) _spacebar = false;

        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            _directionKey = 1;
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            _directionKey = 2;
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            _directionKey = 3;
        }
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            _directionKey = 4;
        }
    }

    IEnumerator MiniGame(int gameNo)
    {
        _isStart = true;
        _minigameObject[gameNo].SetActive(true);

        if(gameNo == 0)
        {
            _keyEffect = GameObject.FindGameObjectWithTag("MinigameEffect").GetComponent<ParticleSystem>();
            float barWidth = 520f;
            float randVal = Random.Range(0.2f, 0.8f);
            float randomPos = barWidth * randVal;
            
            GameObject.Find("RandomPos").GetComponent<RectTransform>().anchoredPosition = new Vector2(randomPos,0);
            float offsetXY = 0.03f;

            float minPerfectVal = (randVal - offsetXY);
            float maxPerfectVal = (randVal + offsetXY);
            
            Slider slider = _minigameObject[gameNo].transform.GetChild(0).GetComponent<Slider>();
            int upDownVal = 1;
            while (!minigameEnd)
            {
                slider.value += Time.deltaTime * (2-_playerStat._repairStat) * upDownVal;
                if (slider.value >= 1) upDownVal = -1;
                else if (slider.value <= 0) upDownVal = 1;

                if (_spacebar)
                {
                    _keyEffect.Play();
                    StartCoroutine(ColorChange(slider.transform.GetChild(1).GetChild(0).GetComponent<Image>()));
                    
                    float nowVal = slider.value;
                    if (nowVal >= minPerfectVal && nowVal <= maxPerfectVal)
                    {
                        minigameEnd = true;
                        _isSuccess = true;
                        _resultText.text = string.Format("성공!");
                    }
                    else
                    {
                        //실패
                        _resultText.text = string.Format("실패!");
                    }
                }
                yield return new WaitForEndOfFrame();
            }

            yield return new WaitForSeconds(1f);
            gameObject.SetActive(false);
        }
        else if (gameNo == 1)
        {
            _keyEffect = GameObject.FindGameObjectWithTag("MinigameEffect").GetComponent<ParticleSystem>();
            int noteCnt = 5;
            bool end = false;
            while(!end)
            {
                float[] directionArr = { 0,90, 180, 270 }; //왼쪽 아래 오른쪽 위 
                int nowNote = 0;
                for(int i = 0; i < noteCnt; i++)
                {
                    int rand = Random.Range(0, 4);
                    _minigameObject[gameNo].transform.GetChild(i).gameObject.SetActive(true);
                    _minigameObject[gameNo].transform.GetChild(i).GetComponent<Image>().color = Color.white;
                    _minigameObject[gameNo].transform.GetChild(i).eulerAngles = Vector3.forward * directionArr[rand];
                }

                bool reset = false;
                while (noteCnt > 0 && !reset)
                {
                    _minigameObject[gameNo].transform.GetChild(nowNote).GetComponent<Image>().color = Color.green;

                    switch (_directionKey)
                    {
                        case 1:
                            reset = !NoteCheck(_minigameObject[gameNo].transform.GetChild(nowNote).gameObject, 1);
                            break;
                        case 2:
                            reset = !NoteCheck(_minigameObject[gameNo].transform.GetChild(nowNote).gameObject, 2);
                            break;
                        case 3:
                            reset = !NoteCheck(_minigameObject[gameNo].transform.GetChild(nowNote).gameObject, 3);
                            break;
                        case 4:
                            reset = !NoteCheck(_minigameObject[gameNo].transform.GetChild(nowNote).gameObject, 0);
                            break;
                    }
                    if (_directionKey != 0 && reset) //틀림 리셋
                    {
                        _directionKey = 0;
                        noteCnt = 5;
                        break;
                    }
                    else if (_directionKey != 0) //맞춤
                    {
                        nowNote++;
                        noteCnt--;
                        _directionKey = 0;
                        _keyEffect.Play();
                    }
                    else // 미입력
                    {
                        _directionKey = 0;
                    }
                    yield return new WaitForSeconds(0.1f);
                }

                if (noteCnt == 0) end = true;
                yield return new WaitForSeconds(0.1f);
            }
            minigameEnd = true;
            _isSuccess = true;
            _resultText.text = string.Format("성공!");
        }
        else if (gameNo == 2)
        {
            Slider slider = _minigameObject[gameNo].transform.GetChild(0).GetComponent<Slider>();
            while(slider.value < 1)
            {
                if (_spacebar)
                {
                    slider.value += Time.deltaTime * _playerStat._researchStat;
                }
                else
                {
                    slider.value -= Time.deltaTime * _playerStat._researchStat;
                }
                yield return new WaitForEndOfFrame();
            }
            minigameEnd = true;
            _isSuccess = true;
            _resultText.text = string.Format("성공!");
        }
        yield return new WaitForSeconds(1f);
        gameObject.SetActive(false);
    }

    bool NoteCheck(GameObject note, int directionNo)
    {
        int offSet = 5;
        int minVal = 90 * directionNo - offSet;
        int maxVal = 90 * directionNo + offSet;

        int angle = (int)note.transform.eulerAngles.z;
        if (angle <= maxVal && angle >= minVal)
        {
            note.SetActive(false);
            return true;
        }
        else
        {
            return false;
        }
    }
    IEnumerator ColorChange(Image img)
    {
        img.color = Color.blue;
        yield return new WaitForSeconds(0.5f);

        if (minigameEnd) yield return null;
        else img.color = Color.white;
    }
}
