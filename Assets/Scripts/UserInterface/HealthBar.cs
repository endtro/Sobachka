using UnityEngine;

public class HealthBar : UiBar
{
    public override void OnEnable()
    {
        Player.HealthChanged += OnValueChanged;
        Player.MaxHealthChanged += OnMaxValueChanged;

        base.OnEnable();
    }

    private void OnDisable()
    {
        Player.HealthChanged -= OnValueChanged;
        Player.MaxHealthChanged -= OnMaxValueChanged;
    }

    private protected override void SetSliderValues()
    {
        Slider.maxValue = Player.MaxHealth;
        Slider.value = Player.Health;
    }
}
