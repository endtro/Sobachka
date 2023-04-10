using UnityEngine;
using UnityEngine.Events;

public class Cursor : MonoBehaviour
{
    private static Cursor _instance;

    private Player _player;
    private Camera _camera;
    private CursorBounds _viewportBounds = new CursorBounds(0f, 1f, 0f, 1f, CursorBounds.OutOfBoundsBehavior.ClampCoordinates);
    private CursorBounds _screenBounds = new CursorBounds();
    private Vector3 _defaultPosition = new Vector3();
    private UnityEvent<Vector3> _positionChanged = new UnityEvent<Vector3>();

    public static Cursor Instance => _instance;

    public event UnityAction<Vector3> PositionChanged
    {
        add => _positionChanged.AddListener(value);
        remove => _positionChanged.RemoveListener(value);
    }

    private void Awake()
    {
        _instance = this;

        _player = FindObjectOfType<Player>();
        _camera = Camera.main;
    }

    private void Start()
    {
        ConvertViewportToScreenBounds();
    }

    private void Update()
    {
        Vector3 mousePosition = Input.mousePosition;

        if (_screenBounds.Behavior == CursorBounds.OutOfBoundsBehavior.ClampCoordinates)
        {
            mousePosition.x = Mathf.Clamp(mousePosition.x, _screenBounds.MinX, _screenBounds.MaxX);
            mousePosition.y = Mathf.Clamp(mousePosition.y, _screenBounds.MinY, _screenBounds.MaxY);
        }
        else if (_screenBounds.Behavior == CursorBounds.OutOfBoundsBehavior.DefaultPosition)
        {
            if (mousePosition.x < _screenBounds.MinX || mousePosition.x > _screenBounds.MaxX
                || mousePosition.y < _screenBounds.MinY || mousePosition.y > _screenBounds.MaxY)
            {
                if (_player.ShootingEnabled)
                {
                    _player.EnableShooting(false);
                }

                transform.position = _camera.ScreenToWorldPoint(_defaultPosition);
                _positionChanged.Invoke(transform.position);

                return;
            }

            if (_player.ShootingEnabled == false)
            {
                _player.EnableShooting(true);
            }
        }

        Vector3 mouseWorldPosition = _camera.ScreenToWorldPoint(mousePosition);
        transform.position = mouseWorldPosition;

        _positionChanged.Invoke(mouseWorldPosition);
    }

    public void Initialize(CursorBounds cursorBounds)
    {
        _viewportBounds = cursorBounds;

        ConvertViewportToScreenBounds();

        _player.EnableShooting(true);
        _player.SetPosition(_camera.ScreenToViewportPoint(_defaultPosition));
    }

    private void ConvertViewportToScreenBounds()
    {
        const float Middle = 2f;
        const float BottomOneThird = 3f;

        _screenBounds = new CursorBounds(_viewportBounds.MinX * Screen.width, _viewportBounds.MaxX * Screen.width,
            _viewportBounds.MinY * Screen.height, _viewportBounds.MaxY * Screen.height, _viewportBounds.Behavior);

        _defaultPosition.x = (_screenBounds.MinX + _screenBounds.MaxX / Middle);
        _defaultPosition.y = (_screenBounds.MinY + _screenBounds.MaxY / BottomOneThird);
    }
}
