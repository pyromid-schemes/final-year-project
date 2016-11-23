using UnityEngine;

public class KnightScript : MonoBehaviour {

    public float direction;
    public Animator animator;

    private enum States {Idle,Walking,Attack,Defend}
    private States state;

	void Start () {       
        direction = 1f;
        animator = GetComponent<Animator>();
        state = States.Idle;      
	}
	
	void Update () {
            RandomStateChange();
    }

    void FixedUpdate()
    {
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Walking"))
        {
            MoveCharacterForward();
           // Rotate180UponHittingAWall();
        }
    }

    void MoveCharacterForward() {
        transform.Translate(Vector3.forward * Time.deltaTime);
    }

  //  void Rotate180UponHittingAWall() {
  //      RaycastHit hit;
  //      if (Physics.Raycast(transform.position, transform.forward, out hit) && hit.collider.CompareTag("Wall"))
  //      {
  //         if (hit.distance < 0.3f)
  //          {
  //              transform.Rotate(new Vector3(0, 180, 0));
  //          }
  //      }
  //  }

     //TODO: Change these magic numbers to time based
    void RandomStateChange()
    {
        int random = Random.Range(1, 15);
        if (random <= 3)
        {
            switch (state)
            {
                case States.Idle:
                    ToWalkingState();
                    SetState(States.Walking);
                    break;
                case States.Walking:
                    ToIdleState();
                    SetState(States.Idle);
                    break;
            }
        }
        Debug.Log("Random dice: " + random + " current state " + state);
    }

    void ToIdleState() {
        SetWalking(false);
        SetIdle(true);
    }

    void ToWalkingState() {
        SetWalking(true);
        SetIdle(false);
    }

    void IdleToAttackState() {
        SetIdle(false);
        SetAttack(true);    
    }

    void AlternateCombat() {
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
        {
            SetAttack(false);
            SetDefend(true);
        } else {
            SetAttack(true);
            SetDefend(false);
        }
    }

    void SetWalking(bool isWalking) {
        animator.SetBool("isWalking", isWalking);
    }

    void SetIdle(bool isIdle) {
        animator.SetBool("isIdle", isIdle);
    }

    void SetAttack(bool isAttack)
    {
        animator.SetBool("isAttacking", isAttack);
    }

    void SetDefend(bool isDefend)
    {
        animator.SetBool("isDefending", isDefend);
    }

    void SetState(States state) {
        this.state = state;
    }
}
