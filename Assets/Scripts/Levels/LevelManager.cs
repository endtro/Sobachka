using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private GameObject _combatGUI;
    [SerializeField] private GameObject _shopGUI;
    [SerializeField] private Level _shopLevel;
    [SerializeField] private List<Level> _levels = new List<Level>();
    [SerializeField] private Image _blackScreen;
    // _fadeTime работает неправильно. Подробней - в методе FadeIn.
    [SerializeField] private float _fadeTime = 1f;
    [SerializeField] private float _blackScreenPause = 0.1f;

    private static LevelManager _instance;

    private int _levelIndex = 0;
    private Level _loadedLevel;
    private WaitForSeconds _fadeTimeWaitForSeconds;

    public static LevelManager Instance => _instance;

    private void Awake()
    {
        _instance = this;

        _combatGUI.SetActive(false);
        _shopGUI.SetActive(false);

        _shopLevel.gameObject.SetActive(false);

        for (int i = 0; i < _levels.Count; i++)
        {
            _levels[i].gameObject.SetActive(false);
        }

        _fadeTimeWaitForSeconds = new WaitForSeconds(_fadeTime + _blackScreenPause);
    }

    private void Start()
    {
        StartCoroutine(LoadShop(false, true));
    }

    private IEnumerator LoadShop(bool fadeIn = true, bool fadeOut = true)
    {
        if (fadeIn)
        {
            StartCoroutine(FadeIn());
            yield return _fadeTimeWaitForSeconds;
        }

        if (_levels[_levelIndex].gameObject.activeInHierarchy)
        {
            _levels[_levelIndex].gameObject.SetActive(false);
            _combatGUI.SetActive(false);
        }

        _shopLevel.gameObject.SetActive(true);
        _shopGUI.SetActive(true);

        if (fadeOut)
        {
            StartCoroutine(FadeOut());
        }
    }

    public void LoadNextLevel()
    {
        StartCoroutine(LoadLevel(_levelIndex));
    }

    public void BackToShop()
    {
        StartCoroutine(LoadShop());
    }

    private IEnumerator LoadLevel(int index)
    {
        StartCoroutine(FadeIn());
        yield return _fadeTimeWaitForSeconds;

        if (_shopLevel.gameObject.activeInHierarchy)
        {
            _shopLevel.gameObject.SetActive(false);
            _shopGUI.SetActive(false);
        }

        if (_loadedLevel != null)
        {
            _loadedLevel.DeactivateAllEnemies();
            _loadedLevel.gameObject.SetActive(false);
        }

        _levels[index].gameObject.SetActive(true);
        _combatGUI.SetActive(true);

        StartCoroutine(FadeOut());
    }

    public void CompleteLevel()
    {
        _levelIndex = (_levelIndex + 1) % _levels.Count;

        StartCoroutine(LoadShop());
    }

    private IEnumerator FadeIn()
    {
        _blackScreen.gameObject.SetActive(true);

        Color color = Color.black;
        color.a = 0f;

        while (color.a < 1f)
        {
            // _fadeTime должно работать как длительность затемнения экрана, однако значения ниже 1f увеличивают время, а выше - уменьшают.
            // Сходу не придумал, как это исправить, и решил отложить на когда-нибудь напотом.
            color.a += Time.deltaTime * _fadeTime;
            _blackScreen.color = color;

            yield return null;
        }
    }

    private IEnumerator FadeOut()
    {
        Color color = Color.black;
        color.a = 1f;

        while (color.a > 0f)
        {
            // То же, что и в FadeIn.
            color.a -= Time.deltaTime * _fadeTime;
            _blackScreen.color = color;

            yield return null;
        }

        _blackScreen.gameObject.SetActive(false);
    }
}
