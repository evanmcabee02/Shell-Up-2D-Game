using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerScript : MonoBehaviour
{
    private float horizontal;
    private float speed = 8f;
    private float jumpingPower = 8f;
    private bool isFacingRight = true;
    private GameObject Player;
    private bool canDash = true;
    private bool isDashing = false;
    private float shotPower = 20f;
    private float shotTime = 0.3f;
    public AudioSource src;
    public AudioClip shotsound;
    public Vector3 velocity = Vector3.zero;
    private Vector3 RespawnPoint;
    private Vector3 currentspeed;
    private bool canMove = true;
    DeathCount deaths;
    [SerializeField] private Animator animator;
    [SerializeField] private Animator deathAnim;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private ParticleSystem smokeParticles;

    private void Start()
    {
        deaths = GameObject.Find("DC").GetComponent<DeathCount>();
        rb.gravityScale = 1.5f;
        src.clip = shotsound;
        RespawnPoint = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (isDashing)
        {
            return;
        }

        if (IsGrounded() == true)
        {
            animator.SetBool("Grounded", true);
            canDash = true;
        }
        if(canMove == true)
        {
            horizontal = Input.GetAxisRaw("Horizontal");
        }
        else 
        {
            horizontal = 0;
        }

        if (horizontal != 0)
        {
            animator.SetFloat("Speed", Mathf.Abs(horizontal));
        }
        else
        {
            animator.SetFloat("Speed", 0.0f);
        }

        if (Input.GetKeyDown(KeyCode.Space) && IsGrounded() && canMove)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpingPower);
            animator.SetBool("Grounded", false);
        }

        if (Input.GetKeyUp(KeyCode.Space) && rb.velocity.y > 0f)
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.5f);
        }

        if (Input.GetKeyDown(KeyCode.Mouse0) && canDash && canMove)
        {
            var mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            var direction = (mousePos - this.transform.position);
            direction.z = 0;
            src.Play();
            animator.SetBool("Grounded", false);

            // Making sure we have a reasonable vector here
            if (direction.magnitude >= 0.1f)
            {
                // Don't exceed the target, you might not want this
                this.StartCoroutine(this.Dash(direction.normalized));
            }
            //StartCoroutine(Dash());
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }

        Flip();
    }

    private void FixedUpdate()
    {
        if (isDashing)
        {
            return;
        }

        rb.velocity = new Vector2(horizontal * speed, rb.velocity.y);
    }

    private bool IsGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, 0.3f, groundLayer);
    }

    private void Flip()
    {
        if (isFacingRight && horizontal < 0f || !isFacingRight && horizontal > 0f)
        {
            isFacingRight = !isFacingRight;
            Vector3 localScale = transform.localScale;
            localScale.x *= -1f;
            transform.localScale = localScale;
        }
    }

    private void OnTriggerEnter2D(UnityEngine.Collider2D collision)
    {
        if (collision.tag == "Checkpoint")
        {
            RespawnPoint = transform.position;
        }
        else if(collision.tag == "Spikes")
        {
            deaths.addDeaths();
            canMove = false;
            this.StartCoroutine(DeathAnimator(1));
            transform.position = RespawnPoint;
        }
        else if(collision.tag == "DashRefresh")
        {
            canDash = true;
        }
    }

    //Straight Dash
    private IEnumerator Dash(Vector3 direction)
    {
        canDash = false;
        isDashing = true;
        float originalGravity = rb.gravityScale;
        rb.gravityScale = 0f;
        currentspeed = rb.velocity;
        rb.velocity = new Vector3(transform.localScale.x - direction.x * shotPower, transform.localScale.y - direction.y * (shotPower * 0.5f), 0f);
        smokeParticles.enableEmission = true;
        yield return new WaitForSeconds(shotTime * 0.8f);
        rb.gravityScale = originalGravity;
        smokeParticles.enableEmission = false;
        yield return new WaitForSeconds(shotTime * 0.2f);
        isDashing = false;
    }

    private IEnumerator DeathAnimator(int id) 
    {
        deathAnim.SetInteger("State", id);
        yield return new WaitForSeconds(0.5f);
        deathAnim.SetInteger("State", 0);
        canMove = true;
    }
}