using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Slider))]
public abstract class UiBar : MonoBehaviour
{
    private protected Slider Slider;
    private protected Player Player;

    private void Awake()
    {
        Slider = GetComponent<Slider>();
        Player = FindObjectOfType<Player>();
    }

    // Одного OnEnable недостаточно для установки начальных значений, и, без Start, полоски активируются со значениями по умолчанию.
    // Я не понимаю почему.
    private void Start()
    {
        SetSliderValues();
    }

    public virtual void OnEnable()
    {
        SetSliderValues();
    }

    private protected void OnValueChanged(float value)
    {
        Slider.value = value;
    }

    private protected void OnMaxValueChanged(float maxValue)
    {
        Slider.maxValue = maxValue;
    }

    private protected abstract void SetSliderValues();
}
