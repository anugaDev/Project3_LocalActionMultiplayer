using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputController : MonoBehaviour
{
    [SerializeField] private PlayerController player;

    public int controllerNumber = 1;

    [Header("Assigned Buttons")]

    [SerializeField] private string horizontalAxis;
    [SerializeField] private string verticalAxis;
    [SerializeField] private string rightHorizontalAxis;
    [SerializeField] private string rightVerticalAxis;
    [SerializeField] private string jumpButton;
    [SerializeField] private string dashButton;
    [SerializeField] private string fireButton;
    [SerializeField] private string pauseButton;

    public float Horizontal { get; private set; }
    public float Vertical { get; private set; }

    public Vector2 Direction { get; private set; }

    public bool keyboardAndMouse = false;

    public enum Button
    {
        JUMP,
        DASH,
        FIRE,
        PAUSE
    }

    private void Start()
    {
        if (!player) player = GetComponent<PlayerController>();

        AssignButtons();
    }

    private void Update()
    {
        SetMovementAxis();
        
        print(Direction);
        
    }

    private void SetMovementAxis()
    {
        if (controllerNumber > 0)
        {
            Horizontal = Input.GetAxis(horizontalAxis);
            Vertical = Input.GetAxis(verticalAxis);
            if (!keyboardAndMouse) Direction = new Vector2(Horizontal, Vertical).normalized;
            else Direction = PlayerToMouseDirection();
        }
    }

    public void AssignButtons()
    {
        horizontalAxis = "Horizontal" + controllerNumber;
        verticalAxis = "Vertical" + controllerNumber;

        jumpButton = "Jump" + controllerNumber;
        dashButton = "Dash" + controllerNumber;
        fireButton = "Fire" + controllerNumber;
        pauseButton = "Start" + controllerNumber;
    }

    public bool ButtonDown(Button button)
    {
        switch (button)
        {
            case Button.JUMP: return Input.GetButtonDown(jumpButton);
            case Button.DASH: return Input.GetButtonDown(dashButton);
            case Button.FIRE: return Input.GetButtonDown(fireButton);
            case Button.PAUSE: return Input.GetButtonDown(pauseButton);
        }

        return false;
    }

    public bool ButtonIsPressed(Button button)
    {
        switch (button)
        {
            case Button.JUMP: return Input.GetButton(jumpButton);
            case Button.FIRE: return Input.GetButton(fireButton);
        }

        return false;
    }

    public bool ButtonIsUp(Button button)
    {
        switch (button)
        {
            case Button.JUMP: return Input.GetButtonUp(jumpButton);
            case Button.DASH: return Input.GetButtonUp(dashButton);
            case Button.FIRE: return Input.GetButtonUp(fireButton);
        }

        return false;
    }

    public Vector2 PlayerToMouseDirection()
    {
        print("takingDir");
        Vector3 playerPosition = transform.position;
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        Vector2 normalizedVector = new Vector2(0, 0);

        normalizedVector.x = mousePosition.x - playerPosition.x;
        normalizedVector.y = mousePosition.y - playerPosition.y;

        return normalizedVector.normalized;
    }
}

/*
    [SerializeField] private float triggerDownThreshold;
    private bool triggerInUse;

    private void TriggerToButton()
    {
        if (Input.GetAxis(dashButton) > triggerDownThreshold) triggerInUse = !triggerInUse ? true : triggerInUse;
        if (!(Input.GetAxis(dashButton) < triggerDownThreshold)) return;

        triggerInUse = false;
    }
 */
