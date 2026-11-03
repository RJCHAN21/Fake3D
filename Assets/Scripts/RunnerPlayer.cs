using UnityEngine;
using UnityEngine.InputSystem;

public class RunnerPlayer : MonoBehaviour
{
    [SerializeField] private Item playerItem;
    [SerializeField] private InputActionReference moveAction;
    [SerializeField] private InputActionReference jumpAction;

    [SerializeField] private float laneSpacing = 3f;
    [SerializeField] private float groundY = -3f;
    [SerializeField] private float jumpSpeed = 8f;
    [SerializeField] private float gravity = 20f;

    private int lane = 1;
    private bool moveHeld;
    private bool grounded = true;
    private float verticalSpeed;

    private void Awake()
    {
        playerItem.itemPosition = new Vector3(0f, groundY, 0f);
    }

    private void OnEnable()
    {
        moveHeld = false;
        moveAction.action.Enable();
        jumpAction.action.Enable();
    }

    private void OnDisable()
    {
        moveAction.action.Disable();
        jumpAction.action.Disable();
    }

    private void Update()
    {
        UpdateLane();
        UpdateJump();
    }

    private void UpdateLane()
    {
        float horizontal = moveAction.action.ReadValue<Vector2>().x;

        if (Mathf.Abs(horizontal) <= 0.5f)
        {
            moveHeld = false;
        }
        else if (!moveHeld)
        {
            int direction = horizontal > 0f ? 1 : -1;
            lane = Mathf.Clamp(lane + direction, 0, 2);
            moveHeld = true;
        }

        playerItem.itemPosition.x = (lane - 1) * laneSpacing;
    }

    private void UpdateJump()
    {
        if (grounded && jumpAction.action.WasPressedThisFrame())
        {
            verticalSpeed = jumpSpeed;
            grounded = false;
        }

        if (grounded)
        {
            return;
        }

        float deltaTime = Time.deltaTime;

        playerItem.itemPosition.y +=
            verticalSpeed * deltaTime -
            0.5f * gravity * deltaTime * deltaTime;

        verticalSpeed -= gravity * deltaTime;

        if (playerItem.itemPosition.y <= groundY)
        {
            playerItem.itemPosition.y = groundY;
            verticalSpeed = 0f;
            grounded = true;
        }
    }
}