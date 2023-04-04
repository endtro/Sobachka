using TMPro;
using UnityEngine;

[RequireComponent(typeof(TMP_Text))]
public class ScoreDisplay : MonoBehaviour
{
    private TMP_Text _text;
    private Player _player;

    private void Awake()
    {
        _text = GetComponent<TMP_Text>();
        _player = FindObjectOfType<Player>();
    }

    private void OnEnable()
    {
        _player.ScoreChanged += OnValueChanged;
        _text.text = _player.Score.ToString();
    }

    private void OnDisable()
    {
        _player.ScoreChanged -= OnValueChanged;
        _text.text = _player.Score.ToString();
    }

    private void OnValueChanged(int value)
    {
        _text.text = value.ToString();
    }
}
