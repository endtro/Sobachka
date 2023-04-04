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
        // ����� ����� ��� ���� ����������, �� ������, ���� ���������. �� �����-�� ���������� �������, ��� �� � �� �����, ���� OnEnable
        // ����������� ������, ��� Awake ������� Cursor. ���� �� ������ �������, ��� ����� ��������� Awake->OnEnable �� ������ �������
        // ����������, � ����������� �� �� �������� � �����, ������ � ���� ��� ��� �� ��������.
        // ��, ��� OnEnable �� ����� �������� ����������� ������, ��� Awake �� ������ - ��� �����-�� ������.
        _cursor.Initialize(_cursorBounds);
    }
}
