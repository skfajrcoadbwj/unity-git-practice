using UnityEngine;

public class Enemy : MonoBehaviour
{
    float health = 50f;

    private void TakeDamage(int value)
    {
        health -= value;
    }

    public float GetHealth()
    {
        return health;
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        TakeDamage(10);
        Debug.Log("Health : " + health);
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
