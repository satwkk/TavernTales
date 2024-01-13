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
    [SerializeField] private Transform m_FoodHolder;
    public Transform PickupHolder { get => m_PickupHolder; set => m_PickupHolder = value; }
    public Transform FoodHolder { get => m_FoodHolder; set => m_FoodHolder = value; }


    [Header( "Pickup References" )]
    [Space()]
    [SerializeField] private InteractableBase m_CurrentInteractable;
    [SerializeField] private IPickable m_CurrentPickup;
    [SerializeField] private Food m_CurrentPickupFood;
    public Food CurrentPickupFood { get => m_CurrentPickupFood; set => m_CurrentPickupFood = value; }
    

    // EVENTS
    public Action<Food> OnPickFood_Event { get; set; }

    private void Awake() {
        m_InputManager = GetComponent<InputManager>();
    }

    private void Start()
    {
        m_InputManager.OnDropButtonPressed += OnDropButtonPressed_Callback;
    }

    private void Update() 
    {
        if (m_InputManager.isInteractPressed) 
        {
            TraceForInteractable();
        }
    }
    private void OnDrawGizmos() {
        Debug.DrawLine(m_TraceStartPos, m_TraceEndPos, Color.red);
    }

    private void OnDropButtonPressed_Callback()
    {
        if (m_CurrentPickup == null)
        {
            Debug.LogError("No pickup in hands to drop");
            return;
        }
        // m_CurrentPickupFood.Throw();
        DropItem(m_CurrentPickup);
    }

    private void TraceForInteractable() 
    {
        m_TraceStartPos = m_PlayerCamera.ScreenToWorldPoint(m_TargetTraceTransform.position);
        m_TraceEndPos = m_TraceStartPos + m_PlayerCamera.transform.forward * m_TraceRange;
        if (Physics.Raycast(m_TraceStartPos, m_PlayerCamera.transform.forward, out RaycastHit hitInfo, m_TraceRange)) 
        {
            var interactable = hitInfo.collider.GetComponent<InteractableBase>();
            if (interactable is not null)
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

    public void PickupItem(IPickable pickup) 
    {
        if (HasPickupInHands()) 
        {
            Debug.LogError("Player already has something in his hand, cannot pickup anything else");
            return;
        }

        m_CurrentPickup = pickup;
        m_CurrentPickup.Owner = this;
        m_CurrentPickup.PickUp(PickupHolder);
    }

    public void DropItem(IPickable pickup)
    {
        m_CurrentPickup.Drop(PickupHolder);
        m_CurrentPickup.Owner = null;
        m_CurrentPickup = null;
    }

    public Vector3 GetInteractionPosition()
    {
        return transform.position;
    }

    public bool HasPickupInHands()
    {
        return CurrentPickupFood != null;
    }

    // ======================================= IFOODSERVICE ==============================================
    public Food GetFoodInHands()
    {
        return CurrentPickupFood;
    }

    public void PlaceFood(IFoodPlacer foodPlacer)
    {
        CurrentPickupFood.transform.SetParent(null);
        CurrentPickupFood.transform.position = foodPlacer.GetFoodPlacingLocation();
        CurrentPickupFood = null;
    }

    public void PickupFood( Food food )
    {
        if (GetFoodInHands() != null) 
        {
            Debug.LogError("Already has a food in hand");
            return;
        }

        CurrentPickupFood = food;
        CurrentPickupFood.transform.position = FoodHolder.position;
        CurrentPickupFood.transform.SetParent(FoodHolder);
        OnPickFood_Event?.Invoke( CurrentPickupFood );
    }
}
