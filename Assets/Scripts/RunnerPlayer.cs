using UnityEngine;
using UnityEngine.InputSystem;

public class RunnerPlayer : MonoBehaviour
{
#region Serialized Fields
    [SerializeField] private Item playerItem;
    [SerializeField] private InputActionReference moveAction;
    [SerializeField] private InputActionReference jumpAction;

    [SerializeField] private float laneSpacing = 3f;
    [SerializeField] private float groundY = -3f;
    [SerializeField] private float jumpSpeed = 8f;
    [SerializeField] private float gravity = 20f;
    [SerializeField, Min(0.01f)] private float laneChangeDuration = 0.2f;
#endregion

#region Private Fields
    private float laneStartX;
    private float laneTargetX;
    private float laneElapsed;
    private int lane = 1;
    private bool moveHeld;
    private bool grounded = true;
    private float verticalSpeed;
#endregion

#region Unity Life Cycle
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
#endregion

#region Movement Logic
    private void UpdateLane()
    {
        float horizontal = moveAction.action.ReadValue<Vector2>().x;

        if (Mathf.Abs(horizontal) <= 0.5f)
            moveHeld = false;

        else if (!moveHeld)
        {
            int direction = horizontal > 0f ? 1 : -1;
            int nextLane = Mathf.Clamp(lane + direction, 0, 2);

            if (nextLane != lane)
            {
                lane = nextLane;
                laneStartX = playerItem.itemPosition.x;
                laneTargetX = (lane - 1) * laneSpacing;
                laneElapsed = 0f;
            }

            moveHeld = true;
        }

        laneElapsed += Time.deltaTime;

        float t = laneElapsed / Mathf.Max(0.01f, laneChangeDuration);
        float easedT = Ease.SmoothStep(t);

        playerItem.itemPosition.x = laneStartX + (laneTargetX - laneStartX) * easedT;
    }

    private void UpdateJump()
    {
        if (grounded && jumpAction.action.WasPressedThisFrame())
        {
            verticalSpeed = jumpSpeed;
            grounded = false;
        }

        if (grounded) return;

        float deltaTime = Time.deltaTime;

        playerItem.itemPosition.y += verticalSpeed * deltaTime - 0.5f * gravity * deltaTime * deltaTime;
        verticalSpeed -= gravity * deltaTime;

        if (playerItem.itemPosition.y <= groundY)
        {
            playerItem.itemPosition.y = groundY;
            verticalSpeed = 0f;
            grounded = true;
        }
    }
#endregion
}