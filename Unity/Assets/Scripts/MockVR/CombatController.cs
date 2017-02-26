using UnityEngine;
using System.Collections;

public class CombatController : MonoBehaviour {
    private Animator animator;
    private Weapon weapon;

    void Start ()
    {
        animator = GetComponent<Animator>();
        weapon = GetComponentInChildren<Weapon>();
        //TODO: remove this until capability issue with AI and VR is solved, refer to Weapon class or FYP-97
        weapon.setWeaponIsActive(false);
    }

	void Update ()
    {
        if (UserPressLeftMouseButton())
        {
            Attack();
        }
    }

    private bool UserPressLeftMouseButton()
    {
        return Input.GetButtonDown("Fire1");
    }

    private void Attack()
    {
        animator.Play("Attack");
        StartCoroutine(WaitForAnimationToFinish());
    }

    private IEnumerator WaitForAnimationToFinish()
    {
        weapon.setWeaponIsActive(true);
        yield return new WaitForSeconds(animator.GetCurrentAnimatorClipInfo(0).Length);
        weapon.setWeaponIsActive(false);
    }
}
