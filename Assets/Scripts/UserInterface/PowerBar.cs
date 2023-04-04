using UnityEngine;

public class PowerBar : UiBar
{
    public override void OnEnable()
    {
        Player.PowerChanged += OnValueChanged;
        Player.MaxPowerChanged += OnMaxValueChanged;

        base.OnEnable();
    }

    private void OnDisable()
    {
        Player.PowerChanged -= OnValueChanged;
        Player.MaxPowerChanged -= OnMaxValueChanged;
    }

    private protected override void SetSliderValues()
    {
        Slider.maxValue = Player.MaxPower;
        Slider.value = Player.Power;
    }
}
