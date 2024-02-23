using UnityEngine;
using UnityEngine.AI;

public class Zombie : MonoBehaviour {
    private NavMeshAgent agent;
    private Transform player;
    [SerializeField] private LayerMask playerLayer;
    private float attackRange = 2;
    private bool isWalking, isSprinting, isAttacking;
    private int health;
    void Start() {
        health = 75;
        this.gameObject.SetActive(true);
        player = GameObject.Find("Player").transform;
        agent = GetComponent<NavMeshAgent>();
    }
    void Update() {
        isAttacking = false;
        HandleMovement();
        if(Physics.CheckSphere(transform.position,attackRange,playerLayer)) {
            AttackPlayer();
        }
    }

    private void HandleMovement() {
        isWalking = true;
        agent.SetDestination(player.position);
        
    }
    private void AttackPlayer() {
        transform.LookAt(player.position);
        isAttacking=true;
    }
    private void OnCollisionEnter(Collision collision) {
        Debug.Log("test");
    }
    void OnCollisionStay(Collision collision) {
        Debug.Log(collision.gameObject.layer.ToString());
        if(collision.gameObject.layer == playerLayer) {
            Debug.Log("Hitting player");
            //player.damage(1);
        }
    }

    //anim get functions
    public bool IsWalking { get { return isWalking; } }
    public bool IsSprinting { get { return isSprinting; } }
    public bool IsAttacking { get { return isAttacking; } }
    //
    public void Damage(int dmg) {
        if((health-=dmg)>0) {
            health-=dmg;
        } else {
            health = 0;
            Destroy(this.gameObject);
        }
        Debug.Log("Zombie health now: " + health.ToString());
    }
}
