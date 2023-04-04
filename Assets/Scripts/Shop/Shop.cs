using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

// ¬есь магазин построен неправильно, с точки зрени€ дизайна, и написан настолько же неправильно. ѕо-хорошему все это надо удалить
// и написать заново, перед этим так же переписав модули вооружени€.
public class Shop : MonoBehaviour
{
    [SerializeField] private TMP_Text _titleText;
    [SerializeField] private Button _backButton;
    [SerializeField] private Button _nextLevelButton;
    [SerializeField] private Transform _shopContent;
    [SerializeField] private ShopItem _shopItemPrefab;
    [SerializeField] private List<Spaceplane> _spaceplanes = new List<Spaceplane>();
    [SerializeField] private List<Weapon> _frontWeapons = new List<Weapon>();
    [SerializeField] private List<Weapon> _rearWeapons = new List<Weapon>();
    [SerializeField] private List<Generator> _generators = new List<Generator>();
    [SerializeField] private List<Shield> _shields = new List<Shield>();

    private Player _player;
    private List<GameObject> _shopItems = new List<GameObject>();

    private void Awake()
    {
        _player = FindObjectOfType<Player>();
    }

    private void OnEnable()
    {
        DisplayPlayerModules();
    }

    private void OnDisable()
    {
        ClearShopContent();
    }

    public void DisplayPlayerModules()
    {
        ClearShopContent();

        _titleText.text = "ћодификации:";

        AddShopItem(_player.Spaceplane, DisplaySpaceplanes);
        AddShopItem(_player.FrontWeapon, DisplayFrontWeapons);
        AddShopItem(_player.RearWeapon, DisplayRearWeapons);
        AddShopItem(_player.Generator, DisplayGenerators);
        AddShopItem(_player.Shield, DisplayShields);

        _backButton.gameObject.SetActive(false);
        _backButton.onClick.RemoveAllListeners();
    }

    // ƒелегат на делегате, делегатом погон€ет.
    // Ќе придумал как это все сделать из€щней. Ќе получилось ни написать generic метод, ни нормально перегрузить. Ќужны рездельные
    // меню под разные типы модулей, а не вот это вот сваливание всего в одну кучу.
    private void DisplaySpaceplanes()
    {
        ClearShopContent();
        ActivateBackButton();

        _titleText.text = "—амолЄтики:";

        for (int i = 0; i < _spaceplanes.Count; i++)
        {
            ShopItem shopItem = AddShopItem(_spaceplanes[i]);
            shopItem.OnClick += delegate
            {
                TrySellModule(shopItem, _player.Spaceplane, delegate
                {
                    _player.SetSpaceplane((Spaceplane)shopItem.Module);
                });
            };
        }
    }

    private void DisplayFrontWeapons()
    {
        ClearShopContent();
        ActivateBackButton();

        _titleText.text = " урсовые оруди€:";

        for (int i = 0; i < _frontWeapons.Count; i++)
        {
            ShopItem shopItem = AddShopItem(_frontWeapons[i]);
            shopItem.OnClick += delegate
            {
                TrySellModule(shopItem, _player.FrontWeapon, delegate
                {
                    _player.SetFrontWeapon((Weapon)shopItem.Module);
                });
            };
        }
    }

    private void DisplayRearWeapons()
    {
        ClearShopContent();
        ActivateBackButton();

        _titleText.text = " ормовые оруди€:";

        for (int i = 0; i < _rearWeapons.Count; i++)
        {
            ShopItem shopItem = AddShopItem(_rearWeapons[i]);
            shopItem.OnClick += delegate
            {
                TrySellModule(shopItem, _player.RearWeapon, delegate
                {
                    _player.SetRearWeapon((Weapon)shopItem.Module);
                });
            };
        }
    }

    private void DisplayGenerators()
    {
        ClearShopContent();
        ActivateBackButton();

        _titleText.text = "√енераторы:";

        for (int i = 0; i < _generators.Count; i++)
        {
            ShopItem shopItem = AddShopItem(_generators[i]);
            shopItem.OnClick += delegate
            {
                TrySellModule(shopItem, _player.Generator, delegate
                {
                    _player.SetGenerator((Generator)shopItem.Module);
                });
            };
        }
    }

    private void DisplayShields()
    {
        ClearShopContent();
        ActivateBackButton();

        _titleText.text = "ўиты:";

        for (int i = 0; i < _shields.Count; i++)
        {
            ShopItem shopItem = AddShopItem(_shields[i]);
            shopItem.OnClick += delegate
            {
                TrySellModule(shopItem, _player.Shield, delegate
                {
                    _player.SetShield((Shield)shopItem.Module);
                });
            };
        }
    }

    // Ќе нравитс€ Action в параметрах, но писать еще больше почти идентичных методов под каждый тип модулей не хочетс€.
    // ѕредставл€ю сколько лишнего мусора это уродство производит.
    private void TrySellModule(ShopItem shopItem, Module equipedModule, Action playerSetModule)
    {
        int price = equipedModule.Price - shopItem.Module.Price;

        if (_player.Score + price >= 0)
        {
            playerSetModule.Invoke();

            _player.ChangeScore(price);
        }
    }

    private void ActivateBackButton()
    {
        _backButton.gameObject.SetActive(true);
        _backButton.onClick.AddListener(DisplayPlayerModules);
    }

    private void ClearShopContent()
    {
        for (int i = 0; i < _shopItems.Count; i++)
        {
            _shopItems[i].transform.SetParent(null);
            _shopItems[i].gameObject.SetActive(false);
        }

        _shopItems.Clear();
    }

    private ShopItem AddShopItem(Module module)
    {
        ShopItem shopItem = ObjectPooling.Get(_shopItemPrefab).GetComponent<ShopItem>();

        shopItem.Setup(module);
        shopItem.transform.SetParent(_shopContent);

        _shopItems.Add(shopItem.gameObject);

        return shopItem;
    }

    private void AddShopItem(Module module, UnityAction unityAction)
    {
        ShopItem shopItem = AddShopItem(module);
        shopItem.OnClick += unityAction;
    }
}
