using UnityEngine;

[RequireComponent(typeof(RectTransform))]
public class CursorArea : MonoBehaviour
{
    [SerializeField] private CursorBounds.OutOfBoundsBehavior _behaviour;

    private Cursor _cursor;
    private RectTransform _rectTransform;
    private CursorBounds _cursorBounds;

    private void Awake()
    {
        _cursor = Cursor.Instance;

        _rectTransform = GetComponent<RectTransform>();

        _cursorBounds = new CursorBounds(_rectTransform.anchorMin.x, _rectTransform.anchorMax.x,
            _rectTransform.anchorMin.y, _rectTransform.anchorMax.y, _behaviour);
    }

    private void OnEnable()
    {
        // Важно чтобы все юзер интерфейсы, на старте, были выключены. По какой-то непонятной причине, что бы я ни делал, этот OnEnable
        // запускается раньше, чем Awake скритпа Cursor. Люди на форуме говорят, что Юнити запускает Awake->OnEnable на каждом скрипте
        // поочередно, в зависимости от их иерархии в сцене, однако у меня это так не работает.
        // То, что OnEnable на одних скриптах запускается раньше, чем Awake на других - это какой-то тупизм.
        _cursor.Initialize(_cursorBounds);
    }
}
