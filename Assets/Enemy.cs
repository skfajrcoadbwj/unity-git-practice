using UnityEngine;

public class Enemy : MonoBehaviour
{
    float health = 50f;

    public float speed;

    public float Health
    {
        get { return health; }
    }

    void TakeDamage(int value)
    {
        health -= value;
        if (health <= 0)
        {
            Die();
        }
        Debug.Log("Enemy Health : " + health);
    }

    public void TakeDamage(float ratio)
    {
        health -= (int)(health * ratio);
        if (health <= 0)
        {
            Die();
        }
        Debug.Log("Enemy Health : " + health);
    }

    public float GetHealth()
    {
        return health;
    }

    void Die()
    {
        Destroy(this.gameObject);
    }

    public void OnCollisionEnter2D(Collision2D coll)
    {
        if (coll.gameObject.CompareTag("Bullet"))
        {
            Rigidbody2D rb = GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.linearVelocity = Vector2.zero;
                rb.angularVelocity = 0f;
            }

            TakeDamage(10);
            coll.gameObject.SetActive(false);
        }
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