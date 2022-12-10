using UnityEngine;
using UnityEngine.UI;

public class LoadingWindow : MonoBehaviour
{
    [SerializeField] Slider _loadingSlider;
    [SerializeField] Sprite[] _backgroundSprite;
    [SerializeField] Text _tipText;

    public string[] tips = { "탭키를 통해 미션정보를 확인할 수 있습니다.", "안전구역에 진입하여 적의 탐색을 피할 수 있습니다.", "생존을 위해 적과의 거리조절은 필수입니다." };

    public void InitWindow()
    {
        transform.GetChild(1).GetComponent<Image>().sprite = _backgroundSprite[Random.Range(0, _backgroundSprite.Length)];
        _tipText.text = tips[Random.Range(0, tips.Length)];
    }
    public void SetProgress(float progress)
    {
        _loadingSlider.value = progress;
    }
    public float GetProgress()
    {
        return _loadingSlider.value;
    }
}
