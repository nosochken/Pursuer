using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class CharacterControllerMovement : MonoBehaviour
{
    private const string HorizontalAxis = "Horizontal";
    private const string VerticallAxis = "Vertical";

    [SerializeField] private float _speed = 3f;
    [SerializeField] private float _slowdown = 0.3f;
    [SerializeField] private float _acceleration = 1.5f;

    private CharacterController _characterController;

    private Vector3 _lastPosition;
    private float _vertical;

    private void Awake()
    {
        _characterController = GetComponent<CharacterController>();
    }

    private void Update()
    {
        Move();
    }

    private void Move()
    {
        float horizontal = Input.GetAxis(HorizontalAxis);
        _vertical = Input.GetAxis(VerticallAxis);

        Vector3 position = _characterController.transform.position;
        Vector3 velocity = (position - _lastPosition) / Time.deltaTime;

        ChangeMovementBasedOnSurface(SurfaceDeterminant.GetSurfaceState(position, velocity));
        _lastPosition = position;

        Vector3 movement = new Vector3(horizontal, 0, _vertical);
        _characterController.Move((movement * _speed + Physics.gravity) * Time.deltaTime);
    }

    private void ChangeMovementBasedOnSurface(SurfaceState surfaceState)
    {
        if (surfaceState == SurfaceState.Rise)
            _vertical *= _slowdown;
        else if (surfaceState == SurfaceState.Descent)
            _vertical *= _acceleration;
    }
}