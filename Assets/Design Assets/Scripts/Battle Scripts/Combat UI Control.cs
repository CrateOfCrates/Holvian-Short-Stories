using UnityEngine;
using UnityEngine.InputSystem;

public class CombatUIControl : MonoBehaviour
{

    [SerializeField] private InputActionAsset controls;
    private InputAction panTargetingLeft;

    private void Awake()
    {
        // Find the action within the specific map
        panTargetingLeft = controls.FindActionMap("Battle").FindAction("BattlePanLeft");
    }

    private void OnEnable()
    {
        // Subscribe your custom methods to the input events
        panTargetingLeft.started += OnBattlePanLeftPressed; 
        
        panTargetingLeft.Enable();
    }

    private void OnDisable()
    {
        // Unsubscribe to avoid memory leaks
        panTargetingLeft.started -= OnBattlePanLeftPressed;
        
        panTargetingLeft.Disable();
    }

    private void OnBattlePanLeftPressed(InputAction.CallbackContext context)
    {
        Debug.Log("key pressed!");
    }

    void Update()
    {
        if (Pointer.current != null && Pointer.current.press.wasPressedThisFrame)
        {
            //Debug.Log("mouse down detected");
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Pointer.current.position.ReadValue());
            Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);
            //Debug.Log(mousePos2D);

            RaycastHit2D hit = Physics2D.Raycast(mousePos2D, Vector2.zero);

            if (hit.collider != null)
            {
                SingleEnemyManager singleEnemyManager = hit.collider.gameObject.GetComponent<SingleEnemyManager>();
                singleEnemyManager.OnClick();
            }
        }
    }
}
