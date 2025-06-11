using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class RigidbodyMovement : MonoBehaviour
{
    [SerializeField] private Transform _feet;
    [SerializeField] private CharacterControllerMovement _target;
    
    [SerializeField] private float _speed = 8.7f;
    [SerializeField] private float _appliedForce = 4f;
    
    [SerializeField] private float _minInsurmountableStepHeight = 0.8f;
    [SerializeField] private float _checkDistance = 0.5f;
    [SerializeField] private float _slopeMaxAngle = 45f;
   
    private Rigidbody _rigidbody;
    private Vector3 _direction;
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
        _direction = new Vector3(directionToTarget.x, 0, directionToTarget.z);

        RotateTowardsTatget();
        
        if (Physics.Raycast(_feet.position, transform.forward, out RaycastHit hitInfo, _checkDistance))
            DealWithObstacle(hitInfo);

        _rigidbody.velocity = _direction * _speed + Physics.gravity;
    }
    
    private void RotateTowardsTatget()
    {
        Quaternion targetRotation = Quaternion.LookRotation(_direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 10f * Time.deltaTime);
    }
    
    private void DealWithObstacle(RaycastHit hit)
    {
        float slopeAngle = Vector3.Angle(hit.normal, Vector3.up);

        if (slopeAngle == _rightAngle)
        {
            Vector3 pointOfNoGo = _feet.position + Vector3.up * _minInsurmountableStepHeight;

            if (Physics.Raycast(pointOfNoGo, transform.forward, _checkDistance))
                _direction = Vector3.zero;

            else
                _direction += Vector3.up * _appliedForce;
        }
        else if (slopeAngle > 0 && slopeAngle < _slopeMaxAngle)
        {
            _direction.z *= _appliedForce;
        }
        else
            _direction = Vector3.zero;
    }
}