using UnityEngine;
using System.Collections;

public class CombatController : MonoBehaviour {
    private Animator animator;
    
    void Start ()
    {
        animator = GetComponent<Animator>();
    }

	void Update ()
    {
        if (UserPressLeftMouseButton())
        {
            Attack();
        }
    }

    bool UserPressLeftMouseButton()
    {
        return Input.GetButtonDown("Fire1");
    }

    void Attack()
    {
        animator.Play("Attack");
    }
}
