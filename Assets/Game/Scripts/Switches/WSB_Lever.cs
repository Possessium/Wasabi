using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class WSB_Lever : MonoBehaviour
{
    [SerializeField] InputActionAsset inputBan = null;
    [SerializeField] InputActionAsset inputLux = null;

    [SerializeField] bool active = false;
    [SerializeField] UnityEvent onActivate = null;
    [SerializeField] UnityEvent onDeactivate = null;
    [SerializeField] Vector2 characterPosition = Vector2.zero;
    [SerializeField] Animator animator = null;

    [SerializeField] float cooldown = .2f;
    bool canPress = true;

    private void Start()
    {
        TryGetComponent(out animator);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = active ? Color.green : Color.red;
        Gizmos.DrawSphere((Vector2)transform.position + characterPosition, .2f);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // If ban enter this trigger, its Use action is bound to the Interact method
        if (collision.GetComponent<WSB_Ban>())
        {
            inputBan.FindAction("Interact").performed += Interact;
            WSB_Ban.I.ToggleLever(true, active);
        }

        // If lux enter this trigger, its Use action is bound to the Interact method
        if (collision.GetComponent<WSB_Lux>())
        {
            inputLux.FindAction("Interact").performed += Interact;
            WSB_Lux.I.ToggleLever(true, active);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        // If ban exits this trigger, its Interact method is unbound from the Use action
        if (collision.GetComponent<WSB_Ban>())
        {
            inputBan.FindAction("Interact").performed -= Interact;
            WSB_Ban.I.ToggleLever(false);
        }

        // If lux exits this trigger, its Interact method is unbound from the Use action
        if (collision.GetComponent<WSB_Lux>())
        {
            inputLux.FindAction("Interact").performed -= Interact;
            WSB_Lux.I.ToggleLever(false);
        }
    }

    public void Interact(InputAction.CallbackContext _ctx)
    {
        // Call activate event and inverse active bool
        if(active && canPress)
        {
            if(animator)
                animator.SetBool("Open", active);

            transform.position = new Vector3(transform.position.x, transform.position.y, -2);
            WSB_Ban.I.AnimateLever((Vector2)transform.position + characterPosition);
            WSB_Lux.I.AnimateLever((Vector2)transform.position + characterPosition);
            onDeactivate?.Invoke();
            active = canPress = false;
            StartCoroutine(Cooldown());
        }
        // Call deactivate event and inverse active bool
        else if (canPress)
        {
            if (animator)
                animator.SetBool("Open", active);

            transform.position = new Vector3(transform.position.x, transform.position.y, 2);
            WSB_Ban.I.AnimateLever((Vector2)transform.position + characterPosition);
            WSB_Lux.I.AnimateLever((Vector2)transform.position + characterPosition);
            onActivate?.Invoke();
            active = true;
            canPress = false;
            StartCoroutine(Cooldown());
        }
    }

    IEnumerator Cooldown()
    {
        if (animator)
            animator.SetTrigger("Activate");

        yield return new WaitForSeconds(cooldown);
        canPress = true;
    }
}
