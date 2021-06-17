using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class WSB_Lever : MonoBehaviour
{
    public bool Active { get; private set; } = false;
    [SerializeField] UnityEvent onActivate = null;
    [SerializeField] UnityEvent onDeactivate = null;
    [SerializeField] Vector2 characterPosition = Vector2.zero;
    public Vector2 Position { get { return (Vector2)transform.position + characterPosition; } }
    [SerializeField] Animator animator = null;
    private static readonly int open_Hash = Animator.StringToHash("Open");
    private static readonly int activate_Hash = Animator.StringToHash("Activate");

    public bool CanPress = true;

    private void Start()
    {
        TryGetComponent(out animator);
        animator.SetBool(open_Hash, !Active);
        animator.SetTrigger(activate_Hash);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Active ? Color.green : Color.red;
        Gizmos.DrawSphere((Vector2)transform.position + characterPosition, .2f);
    }

    public void EnablePress() => StartCoroutine(Cooldown());

    IEnumerator Cooldown()
    {
        yield return new WaitForSeconds(1.5f);
        CanPress = true;
    }

    public void DisablePress() => CanPress = false;

    public void Interact()
    {
        // Call activate event and inverse active bool
        if(Active)
        {
            if(animator)
            {
                animator.SetBool(open_Hash, Active);
                animator.SetTrigger(activate_Hash);
            }

            transform.position = new Vector3(transform.position.x, transform.position.y, -7);

            onDeactivate?.Invoke();
            Active = CanPress = false;
        }
        // Call deactivate event and inverse active bool
        else
        {
            if (animator)
            {
                animator.SetBool(open_Hash, Active);
                animator.SetTrigger(activate_Hash);
            }

            transform.position = new Vector3(transform.position.x, transform.position.y, 2);


            onActivate?.Invoke();
            Active = true;
            CanPress = false;
        }
    }
}
