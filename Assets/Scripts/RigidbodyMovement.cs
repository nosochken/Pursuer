using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class RigidbodyMovement : MonoBehaviour
{
    [SerializeField] private Transform _feet;
    [SerializeField] private CharacterControllerMovement _target;

    [SerializeField] private float _speed = 8.7f;
    [SerializeField] private float _acceleration = 1.5f;

    [SerializeField] private float _minInsurmountableStepHeight = 0.8f;
    [SerializeField] private float _checkDistance = 0.5f;
    [SerializeField] private float _surfaceMaxAngle = 45f;

    private Rigidbody _rigidbody;
    private Vector3 _velocity;
    private float _rightAngle = 90f;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        MoveTowardsTarget();
    }

    private void MoveTowardsTarget()
    {
        Vector3 directionToTarget = (_target.transform.position - transform.position).normalized;
        Vector3 horizontalDirection = new Vector3(directionToTarget.x, 0f, directionToTarget.z);

        _velocity = _rigidbody.velocity;
        _velocity.x = horizontalDirection.x * _speed;
        _velocity.z = horizontalDirection.z * _speed;

        RotateTowardsTatget(horizontalDirection);

        ChangeVelocityBasedOnSurface(SurfaceDeterminant.GetSurfaceState(_rigidbody.position, _rigidbody.velocity));

        if (Physics.Raycast(_feet.position, transform.forward, out RaycastHit hitInfo, _checkDistance))
            DealWithObstacle(hitInfo);

        _rigidbody.velocity = _velocity;
    }

    private void RotateTowardsTatget(Vector3 horizontalDirection)
    {
        Quaternion targetRotation = Quaternion.LookRotation(horizontalDirection);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 10f * Time.deltaTime);
    }

    private void ChangeVelocityBasedOnSurface(SurfaceState surfaceState)
    {
        if (surfaceState == SurfaceState.Rise)
        {
            if (SurfaceDeterminant.CanOvercomeSurface(transform.position, _surfaceMaxAngle))
                _velocity.z *= _acceleration;
            else
                _velocity = Vector3.zero;
        }
        else if (surfaceState == SurfaceState.Descent)
        {
            _velocity.z *= _acceleration;
        }
    }

    private void DealWithObstacle(RaycastHit hit)
    {
        float slopeAngle = Vector3.Angle(hit.normal, Vector3.up);

        if (slopeAngle == _rightAngle)
        {
            Vector3 pointOfNoGo = _feet.position + Vector3.up * _minInsurmountableStepHeight;

            if (Physics.Raycast(pointOfNoGo, transform.forward, _checkDistance))
                _velocity = Vector3.zero;
            else
                _velocity = Vector3.up * _acceleration;
        }
    }
}