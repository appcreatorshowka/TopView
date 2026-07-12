using UnityEngine;

public class ArrowController : MonoBehaviour
{
    public float deleteTime = 2;
    public int attackPower = 1;
    void Start()
    {
        Destroy(gameObject, deleteTime);
    }
    void Update()
    {

    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("Arrow collided with: " + collision.gameObject.tag + " (name: " + collision.gameObject.name + ")");

        transform.SetParent(collision.transform);
        GetComponent<CircleCollider2D>().enabled = false;
        GetComponent<Rigidbody2D>().simulated = false;
    }
}
