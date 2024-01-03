using System;
using UnityEngine;

public enum PlayerState {

    // When player is performing basic actions
    IDLE,

    // When player has picked up an item
    PICKUP
}

[RequireComponent(typeof(CharacterController), typeof(InputManager))]
public class PlayerController : MonoBehaviour {
    CharacterController m_CharacterController;

    InputManager m_InputManager;

    [SerializeField] Transform m_CameraHolder;

    [Header("PLAYER SETTINGS")]
    public float m_MovementSpeed = 20f;

    public float m_Gravity = 9.8f;

    public float m_Sensitivity = 10.0f;

    public bool m_bYawInverted = true;

    public PlayerState m_PlayerState;

    void Awake() {
        m_CharacterController = GetComponent<CharacterController>();
        m_InputManager = GetComponent<InputManager>();
    }

    void Start() {
        m_PlayerState = PlayerState.IDLE;
    }

    void Update() {
        UpdateMovement();
        UpdateCamera();
    }

    void UpdateMovement() {
        var movementVec = new Vector3(m_InputManager.hMoveInputValue, 0f, m_InputManager.vMoveInputValue).normalized;
        if (!m_CharacterController.isGrounded) {
            movementVec.y -= m_Gravity;
        }
        movementVec = transform.TransformDirection(movementVec);
        m_CharacterController.Move(m_MovementSpeed * Time.deltaTime * movementVec);
    }

    void UpdateCamera() {
        transform.rotation = Quaternion.Slerp(
            transform.rotation, 
            transform.rotation * Quaternion.Euler(0f, m_InputManager.xMouseInputValue, 0f), 
            1f
        );

        m_CameraHolder.rotation = Quaternion.Slerp(
            m_CameraHolder.rotation, 
            m_CameraHolder.rotation * Quaternion.Euler(m_InputManager.yMouseInputValue * (m_bYawInverted ? -1 : 1), 0f, 0f), 
            1f
        );
    }

    public PlayerState GetPlayerState() {
        return m_PlayerState;
    }
}
