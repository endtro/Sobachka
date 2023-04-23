using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ShopItem : PoolObject<ShopItem>
{
    [SerializeField] private Button _button;
    [SerializeField] private Image _icon;
    [SerializeField] private TMP_Text _name;
    [SerializeField] private TMP_Text _price;
    [SerializeField] private TMP_Text _description;

    private Module _module;

    public Module Module => _module;

    public event UnityAction OnClick
    {
        add => _button.onClick.AddListener(value);
        remove => _button.onClick.RemoveListener(value);
    }

    private void OnDisable()
    {
        _button.onClick.RemoveAllListeners();
    }

    public void Setup(Module module)
    {
        _icon.sprite = module.Icon;
        _name.text = module.Name;
        _price.text = module.Price.ToString();
        _description.text = module.Description;
        _module = module;
    }
}
