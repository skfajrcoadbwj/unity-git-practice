using UnityEngine;

public class Player : MonoBehaviour
{
    float health = 100f;

    public float speed;

    public float Health
    {
        get { return health; }
    }

    private void TakeDamage(int value)
    {
        health -= value;
        if (health <= 0)
        {
            Die();
        }
        Debug.Log("Player Health : " + health);
    }

    void Die()
    {
        Destroy(this.gameObject);
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        TakeDamage(20);
        Destroy(collision.gameObject);
    }

    public virtual void Move()
    {

    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}