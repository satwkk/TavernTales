using UnityEngine;

public class InteractionController : MonoBehaviour, IFoodCharacter {

    private InputManager m_InputManager;
    private InteractablePickup m_CurrentPickup;
    private PlayerFoodManager m_FoodManager;
    private Vector3 m_TraceStartPos;
    private Vector3 m_TraceEndPos;
    public Transform m_TargetTraceTransform;
    public Camera m_PlayerCamera;
    public float m_TraceRange = 20.0f;

    [Header("Pickup Settings")]
    public Transform m_PickupHolder;

    private void Awake() {
        m_InputManager = GetComponent<InputManager>();
        m_FoodManager = GetComponent<PlayerFoodManager>();
    }

    private void Update() {
        if (m_InputManager.isInteractPressed) {
            TraceForInteractables();
        }
    }
    private void OnDrawGizmos() {
        Debug.DrawLine(m_TraceStartPos, m_TraceEndPos, Color.red);
    }
    public PlayerFoodManager GetPlayerFoodManager() => m_FoodManager;

    private void TraceForInteractables() {
        m_TraceStartPos = m_PlayerCamera.ScreenToWorldPoint(m_TargetTraceTransform.position);
        m_TraceEndPos = m_TraceStartPos + m_PlayerCamera.transform.forward * m_TraceRange;
        if (Physics.Raycast(m_TraceStartPos, m_PlayerCamera.transform.forward, out RaycastHit hitInfo, m_TraceRange)) {
            var interactable = hitInfo.collider.GetComponent<InteractableBase>();
            if (interactable != null) {
                interactable.Interact(this);
            }
        }
    }

    public void PickupItem(InteractablePickup pickup) {
        // IF PLAYER HAS FOOD IN HANDS, HE CANNOT PICKUP ANYTHING ELSE
        if (m_CurrentPickup != null) {
            Debug.LogError("Player already has something in his hand, cannot pickup anything else");
            return;
        }
        m_CurrentPickup = pickup;
        m_CurrentPickup.transform.position = m_PickupHolder.position;
        m_CurrentPickup.transform.SetParent(m_PickupHolder.transform);
    }

    public void DropItem() {
        if (m_CurrentPickup == null) {
            Debug.LogError("Player does not have any pickup in hands");
            return;
        }
        m_CurrentPickup.transform.SetParent(null);
        m_CurrentPickup = null;
    }

    public Food GetFoodInHands() {
        return m_FoodManager.GetCurrentFood();
    }
}
