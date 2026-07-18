using UnityEngine;

public class ArrowController : MonoBehaviour
{
    public float deleteTime = 2;
    public int attackPower = 1;

    bool damaged = false; // (0715)

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

        // 19-22(0715)
   
        if (damaged) return;  // ← 2回目以降は無視
        damaged = true;

        //// 25-38 (0715)
        //// ★敵に当たった場合
        if (collision.gameObject.CompareTag("Enemy"))
        {
            //    EnemyController enemy = collision.gameObject.GetComponent<EnemyController>();
            //    enemy.hp -= attackPower;

            // ダメージを入れたら矢を消す
            Destroy(gameObject);
        return;
        }

        // ★地形に当たった場合だけ刺さる演出
        //if (collision.gameObject.CompareTag("Wall") ||
        //    collision.gameObject.CompareTag("Ground"))
        //{
        transform.SetParent(collision.transform);
        GetComponent<CircleCollider2D>().enabled = false;
        GetComponent<Rigidbody2D>().simulated = false;
        //}
    }
}
