using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class CharacterControllerMovement : MonoBehaviour
{
    [SerializeField] private Transform _finishPoint;
    [SerializeField] private float _speed = 3f;

    private CharacterController _characterController;

    private bool _didReach = false;

    private void Awake()
    {
        _characterController = GetComponent<CharacterController>();
    }

    private void Update()
    {
        if (!_didReach)
            _characterController.Move((-Vector3.forward * _speed + Physics.gravity) * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        _didReach = true;
    }
}