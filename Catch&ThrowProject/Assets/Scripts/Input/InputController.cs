using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputController : MonoBehaviour
{
    [SerializeField] private PlayerController player;
    [SerializeField] private int controllerNumber = 1;

    [Header("Assigned Buttons")]

    [SerializeField] private string horizontalAxis;
    [SerializeField] private string verticalAxis;
    [SerializeField] private string rightHorizontalAxis;
    [SerializeField] private string rightVerticalAxis;
    [SerializeField] private string jumpButton;
    [SerializeField] private string dashButton;
    [SerializeField] private string fireButton;

    public float Horizontal { get; private set; }
    public float Vertical { get; private set; }

    public float RightHorizontal { get; private set; }
    public float RightVertical { get; private set; }

    public Vector2 Direction { get; private set; }
    public Vector2 RightDirection { get; private set; }

    public enum Button
    {
        JUMP,
        DASH,
        FIRE
    }

    private void Start()
    {
        if (!player) player = GetComponent<PlayerController>();

        AssignButtons();
    }

    private void Update()
    {
        SetMovementAxis();
    }

    private void SetMovementAxis()
    {
        if (controllerNumber > 0)
        {
            Horizontal = Input.GetAxis(horizontalAxis);
            Vertical = -Input.GetAxis(verticalAxis);
            Direction = new Vector2(Horizontal, Vertical);

            RightHorizontal = Input.GetAxis(rightHorizontalAxis);
            RightVertical = -Input.GetAxis(rightVerticalAxis);
            RightDirection = new Vector2(RightHorizontal, RightVertical);
        }
    }

    public void AssignButtons()
    {
        horizontalAxis = "Horizontal" + controllerNumber;
        verticalAxis = "Vertical" + controllerNumber;
        rightHorizontalAxis = "RHorizontal" + controllerNumber;
        rightVerticalAxis = "RVertical" + controllerNumber;
        jumpButton = "Jump" + controllerNumber;
        dashButton = "Dash" + controllerNumber;
        fireButton = "Fire" + controllerNumber;
    }

    public bool ButtonDown(Button button)
    {
        switch (button)
        {
            case Button.JUMP: return Input.GetButtonDown(jumpButton);
            case Button.DASH: return Input.GetButtonDown(dashButton);
            case Button.FIRE: return Input.GetButtonDown(fireButton);
        }

        return false;
    }

    public bool ButtonIsPressed(Button button)
    {
        switch (button)
        {
            case Button.JUMP: return Input.GetButton(jumpButton);
            case Button.DASH: return Input.GetButton(dashButton);
            case Button.FIRE: return Input.GetButton(fireButton);
        }

        return false;
    }
}
