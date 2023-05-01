using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelSwitcher : MonoBehaviour
{
    [SerializeField] private GameObject _combatGUI;
    [SerializeField] private GameObject _shopGUI;
    [SerializeField] private Level _shopLevel;
    [SerializeField] private List<Level> _levels = new List<Level>();
    [SerializeField] private Image _blackScreen;
    // _fadeTime должно работать как длительность затемнения экрана, однако значения ниже 1f увеличивают время, а выше - уменьшают.
    // Сходу не придумал, как это исправить, и решил отложить на когда-нибудь напотом.
    [SerializeField] private float _fadeTime = 1f;
    [SerializeField] private float _blackScreenPause = 0.1f;

    private static LevelSwitcher _instance;

    private int _levelIndex = 0;
    private Level _loadedLevel;
    private WaitForSeconds _fadeTimeWaitForSeconds;

    public static LevelSwitcher Instance => _instance;

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
            StartCoroutine(Fade(1f));
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
            StartCoroutine(Fade(0f));
        }
    }

    public void LoadNextLevel()
    {
        StartCoroutine(LoadLevel(_levelIndex));
    }

    public void BackToShop()
    {
        Player.Instance.Setup();

        StartCoroutine(LoadShop());
    }

    private IEnumerator LoadLevel(int index)
    {
        StartCoroutine(Fade(1f));
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

        StartCoroutine(Fade(0f));
    }

    public void CompleteLevel()
    {
        _levelIndex = (_levelIndex + 1) % _levels.Count;

        StartCoroutine(LoadShop());
    }

    private IEnumerator Fade(float targetAlpha)
    {
        if (targetAlpha != 0f)
        {
            _blackScreen.gameObject.SetActive(true);
        }

        Color color = Color.black;

        color.a = (targetAlpha == 0f) ? 1f : 0f;

        while (color.a != targetAlpha)
        {
            color.a = Mathf.MoveTowards(color.a, targetAlpha, Time.deltaTime * _fadeTime);
            _blackScreen.color = color;

            yield return null;
        }

        if (targetAlpha == 0f)
        {
            _blackScreen.gameObject.SetActive(false);
        }
    }
}
