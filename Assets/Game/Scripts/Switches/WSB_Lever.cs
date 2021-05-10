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

    public void Interact()
    {
        // Call activate event and inverse active bool
        if(active && canPress)
        {
            if(animator)
                animator.SetBool(open_Hash, active);

            transform.position = new Vector3(transform.position.x, transform.position.y, -2);

            onDeactivate?.Invoke();
            active = canPress = false;
            StartCoroutine(Cooldown());
        }
        // Call deactivate event and inverse active bool
        else if (canPress)
        {
            if (animator)
                animator.SetBool(open_Hash, active);

            transform.position = new Vector3(transform.position.x, transform.position.y, 2);


            onActivate?.Invoke();
            active = true;
            canPress = false;
            StartCoroutine(Cooldown());
        }
    }

    IEnumerator Cooldown()
    {
        if (animator)
            animator.SetTrigger(activate_Hash);

        yield return new WaitForSeconds(cooldown);
        canPress = true;
    }
}
