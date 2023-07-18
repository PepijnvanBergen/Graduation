using UnityEngine;


public class Soldier : BaseUnit
{
    public Vector3 safePosition;
    public Soldier friend;
    [SerializeField] private Transform friendShowerPos;
    public BaseUnit attackTarget;
    public BaseChoice choice;
    public Group group;

    public float attackDamage = 10f;
    public GameObject choiceSignal;

    private bool dying = false;

    public override void FollowChoice()
    {
        if (!dying)
        {
            choice.Action(this);
        
            if (health < 0)
            {
                animator.SetBool("isIdle", false);
                animator.SetBool("isRunning", false);
                animator.SetBool("isAttacking", false);
                animator.SetBool("isDead", true);
             
                group.units.Remove(this);
                dying = true;
                Destroy(this.gameObject, 2.3f);
                
            }

            if (choiceSignal != null)
            {
                choiceSignal.transform.position = new Vector3(transform.position.x, 2, 2 + transform.position.z);
            }
        }
    }
    
    public float time = 0;
    private float attackTime = 1f;
    public void Attack()
    {
        if (time > attackTime)
        {
            if (attackTarget == null)
            {
                if (friend.attackTarget != null)
                {
                    attackTarget = friend.attackTarget;
                }
            }
            attackTarget.health -= attackDamage;
            attackTarget.morale -= 15f;

            time = 0;
        }
        else
        {
            time += Time.deltaTime;
        }
    }
}