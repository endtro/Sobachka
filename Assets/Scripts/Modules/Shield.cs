using UnityEngine;

[CreateAssetMenu(fileName = "NewShield", menuName = "Module/Shield", order = 51)]
public class Shield : Module
{
    [SerializeField] private float _maxEnergy;
    [SerializeField] private float _energyUsageEfficiency;

    public float MaxEnergy => _maxEnergy;
    public float EnergyUsageEfficiency => _energyUsageEfficiency;
}
