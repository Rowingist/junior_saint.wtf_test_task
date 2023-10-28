using Codebase.CameraLogic;
using Codebase.Infrastructure;
using Codebase.Services.Input;
using UnityEngine;

namespace Codebase.Hero
{
  public class HeroMove : MonoBehaviour
  {
    [SerializeField] private CharacterController _characterController;
    [SerializeField] private float _movementSpeed;
    
    private IInputService _inputService;
    private Camera _camera;

    private void Awake() => 
      _inputService = GameBootstrapper.InputService;

    private void Start()
    {
      _camera = Camera.main;

      CameraFollow();
    }

    private void CameraFollow() => 
      _camera.GetComponent<CameraFollow>().Follow(gameObject);

    private void Update()
    {
      Vector3 movementVector = Vector3.zero;

      if (_inputService.Axis.sqrMagnitude > Constants.Epsilon)
      {
        movementVector = _camera.transform.TransformDirection(_inputService.Axis);
        movementVector.y = 0;
        movementVector.Normalize();

        transform.forward = movementVector;
      }

      movementVector += Physics.gravity;
      
      _characterController.Move(movementVector * (Time.deltaTime * _movementSpeed));
    }
  }
}