using System;
using UnityEngine;

public class InteractionController : MonoBehaviour, IFoodOrderService, IPickableActor
{

    private InputManager m_InputManager;

    [Header("Interaction Settings")]
    [Space()]
    private Vector3 m_TraceStartPos;
    private Vector3 m_TraceEndPos;
    [SerializeField] private Transform m_TargetTraceTransform;
    [SerializeField] private Camera m_PlayerCamera;
    [SerializeField] private float m_TraceRange = 20.0f;

    [Header("Pickup Settings")]
    [SerializeField] private Transform m_PickupHolder;
    public Transform PickupHolder { get => m_PickupHolder; set => m_PickupHolder = value; }


    [Header( "Pickup References" )]
    [Space()]
    [SerializeField] private InteractableBase m_CurrentInteractable;
    [SerializeField] private InteractablePickup m_CurrentPickup;
    [SerializeField] private Food m_CurrentPickupFood;

    // EVENTS
    public Action<Food> OnPickFood_Event { get; set; }

    private void Awake() {
        m_InputManager = GetComponent<InputManager>();
    }

    private void Start()
    {
        Food.OnPickup += PickupFood;
    }

    private void Update() 
    {
        if (m_InputManager.isInteractPressed) 
        {
            TraceForInteractables();
        }
    }
    private void OnDrawGizmos() {
        Debug.DrawLine(m_TraceStartPos, m_TraceEndPos, Color.red);
    }

    private void TraceForInteractables() 
    {
        m_TraceStartPos = m_PlayerCamera.ScreenToWorldPoint(m_TargetTraceTransform.position);
        m_TraceEndPos = m_TraceStartPos + m_PlayerCamera.transform.forward * m_TraceRange;
        if (Physics.Raycast(m_TraceStartPos, m_PlayerCamera.transform.forward, out RaycastHit hitInfo, m_TraceRange)) 
        {
            var interactable = hitInfo.collider.GetComponent<InteractableBase>();
            if (interactable != null) 
            {
                m_CurrentInteractable = interactable;
                m_CurrentInteractable.Interact(this);
            }
            else
            {
                m_CurrentInteractable = null;
            }
        }
        else
        {
            m_CurrentInteractable = null;
        }
    }

    public void PickupItem(InteractableBase pickup) 
    {
        // IF PLAYER HAS FOOD IN HANDS, HE CANNOT PICKUP ANYTHING ELSE
        if (HasPickupInHands()) 
        {
            Debug.LogError("Player already has something in his hand, cannot pickup anything else");
            return;
        }
        m_CurrentInteractable = pickup;
        m_CurrentInteractable.transform.position = PickupHolder.position;
        m_CurrentInteractable.transform.SetParent(PickupHolder);
    }

    public void DropItem(InteractableBase pickup) {
        if (m_CurrentInteractable == null) {
            Debug.LogError("Player does not have any pickup in hands");
            return;
        }

        if (m_CurrentInteractable != pickup)
        {
            Debug.LogError("[BUG] You are trying to throw something that is not in your hands.");
            return;
        }
        
        m_CurrentInteractable.transform.SetParent(null);
        m_CurrentInteractable = null;
    }

    public Vector3 GetInteractionPosition()
    {
        return transform.position;
    }

    public bool HasPickupInHands()
    {
        return m_CurrentPickup != null;
    }

    public Food GetFoodInHands()
    {
        return m_CurrentPickupFood;
    }

    public void PickupFood( Food food )
    {
        if (m_CurrentPickupFood != null) 
        {
            Debug.LogError("Already has a food in hand");
            return;
        }

        m_CurrentPickupFood = food;
        PickupItem(m_CurrentPickupFood);
        OnPickFood_Event?.Invoke( m_CurrentPickupFood );
    }
}
