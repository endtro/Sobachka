using UnityEngine;

public class EnergyBar : UiBar
{
    public override void OnEnable()
    {
        Player.EnergyChanged += OnValueChanged;
        Player.MaxEnergyChanged += OnMaxValueChanged;

        base.OnEnable();
    }

    private void OnDisable()
    {
        Player.EnergyChanged -= OnValueChanged;
        Player.MaxEnergyChanged -= OnMaxValueChanged;
    }

    private protected override void SetSliderValues()
    {
        Slider.maxValue = Player.MaxEnergy;
        Slider.value = Player.Energy;
    }
}
