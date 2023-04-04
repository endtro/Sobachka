using UnityEngine;

public abstract class Module : ScriptableObject
{
    [SerializeField] private Sprite _icon;
    [SerializeField] private string _name;
    [SerializeField] private int _price;
    [SerializeField] private string _description;

    public Sprite Icon => _icon;
    public string Name => _name;
    public int Price => _price;
    public string Description => _description;
}
