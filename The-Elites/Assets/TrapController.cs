using UnityEngine;

public class TrapController : MonoBehaviour
{
    private float speed = 2f;
    private int direction = 1;

    public float range = 5f;

    private Animator animator;
    private void Start()
    {
        animator = GetComponent<Animator>();
    }
    void Update()
    {
        transform.position += new Vector3(speed * direction * Time.deltaTime, 0f, 0f);
        if (transform.position.x > range)
        {
            direction = -1;
            animator.SetBool("flip", true);
        }
        else if (transform.position.x < range * -1f)
        {
            direction = 1;
            animator.SetBool("flip", true);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        
    }
}
