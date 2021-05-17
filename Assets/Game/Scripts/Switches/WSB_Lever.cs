using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class WSB_Lever : MonoBehaviour
{
    [SerializeField] bool active = false;
    [SerializeField] UnityEvent onActivate = null;
    [SerializeField] UnityEvent onDeactivate = null;
    [SerializeField] Vector2 characterPosition = Vector2.zero;
    public Vector2 Position { get { return (Vector2)transform.position + characterPosition; } }
    [SerializeField] Animator animator = null;
    private static readonly int open_Hash = Animator.StringToHash("Open");
    private static readonly int activate_Hash = Animator.StringToHash("Activate");


    [SerializeField] float cooldown = .2f;
    public bool CanPress = true;

    private void Start()
    {
        TryGetComponent(out animator);
        animator.SetBool(open_Hash, !active);
        animator.SetTrigger(activate_Hash);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = active ? Color.green : Color.red;
        Gizmos.DrawSphere((Vector2)transform.position + characterPosition, .2f);
    }

    public void Interact()
    {
        // Call activate event and inverse active bool
        if(active && CanPress)
        {
            if(animator)
            {
                animator.SetBool(open_Hash, active);
                animator.SetTrigger(activate_Hash);
            }

            transform.position = new Vector3(transform.position.x, transform.position.y, -2);

            onDeactivate?.Invoke();
            active = CanPress = false;
        }
        // Call deactivate event and inverse active bool
        else if (CanPress)
        {
            if (animator)
            {
                animator.SetBool(open_Hash, active);
                animator.SetTrigger(activate_Hash);
            }

            transform.position = new Vector3(transform.position.x, transform.position.y, 2);


            onActivate?.Invoke();
            active = true;
            CanPress = false;
        }
    }
}
