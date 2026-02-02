using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] PlayerInput playerInput;

    [SerializeField] float moveSpeed = 12f;
    [SerializeField] float interpolationSpeed = 4f;

    Vector2 targetPosition;

    void Start()
    {
        targetPosition = new Vector2(transform.position.x, transform.position.z);
    }

    void Update()
    {
        targetPosition += moveSpeed * Time.deltaTime * GetMovementInput();

        if (MovementBoundaries.Instance != null) targetPosition = new Vector2(Mathf.Clamp(targetPosition.x, -MovementBoundaries.Instance.maxBounds.x, -MovementBoundaries.Instance.minBounds.x), Mathf.Clamp(targetPosition.y, -MovementBoundaries.Instance.maxBounds.y, -MovementBoundaries.Instance.minBounds.y));
        else Debug.LogWarning("No MovementBoundaries instance found in scene. Player movement will not be clamped.");

        Vector2 currentPosition = new Vector2(transform.position.x, transform.position.z);

        Vector2 newPosition = targetPosition + (currentPosition - targetPosition) * Mathf.Exp(-interpolationSpeed * Time.deltaTime);

        transform.position = new Vector3(newPosition.x, transform.position.y, newPosition.y);
    }

    public void SetTargetPosition(Vector2 target)
    {
        targetPosition = target;
    }

    Vector2 GetMovementInput()
    {
        Vector2 keyboardInput = playerInput.actions["Move"].ReadValue<Vector2>();
        Vector2 mouseInput = Vector2.zero;

        float mouseMaxMovementBoundary = Mathf.Min(Screen.width, Screen.height) / 12f;
        float mouseMinMovementBoundary = Mathf.Min(Screen.width, Screen.height) / 4f;

        Vector2 mousePosition = Mouse.current.position.ReadValue();
        Vector2 inverseMousePosition = new Vector2(Screen.width, Screen.height) - mousePosition;

        Vector2 scaledMousePosition = (Vector2.one * mouseMinMovementBoundary - mousePosition) / (mouseMinMovementBoundary - mouseMaxMovementBoundary);
        Vector2 inverseScaledMousePosition = (Vector2.one * mouseMinMovementBoundary - inverseMousePosition) / (mouseMinMovementBoundary - mouseMaxMovementBoundary);

        mouseInput.x = Mathf.Clamp01(inverseScaledMousePosition.x) - Mathf.Clamp01(scaledMousePosition.x);
        mouseInput.y = Mathf.Clamp01(inverseScaledMousePosition.y) - Mathf.Clamp01(scaledMousePosition.y);
        
        if (keyboardInput.sqrMagnitude > 1f)
        {
            keyboardInput.Normalize();
        }
        if (mouseInput.sqrMagnitude > 1f)
        {
            mouseInput.Normalize();
        }

        return keyboardInput;
    }
}
