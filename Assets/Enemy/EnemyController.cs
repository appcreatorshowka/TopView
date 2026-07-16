using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public float hp = 5f;
    public float speed = 1.0f;
    public float reactionDistance = 4.0f;

    float axisH;
    float axisV;

    Rigidbody2D rbody;
    Animator animator;

    bool isActive = false;

    void Start()
    {
        rbody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        
        // プレイヤー取得
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        // プレイヤーが存在しない（死亡など）なら敵は停止
        if (player == null)
        {
            isActive = false;
            animator.SetBool("IsActive", false);
            axisH = 0;
            axisV = 0;
            return;
        }

        // プレイヤーが存在するので反応状態
        isActive = true;
        animator.SetBool("IsActive", true);

        // プレイヤー方向の角度計算
        float dx = player.transform.position.x - transform.position.x;
        float dy = player.transform.position.y - transform.position.y;
        float rad = Mathf.Atan2(dy, dx);
        float angle = rad * Mathf.Rad2Deg;

        // 方向判定
        Direction direction;
        if (angle > -45.0f && angle <= 45.0f)
            direction = Direction.Right;
        else if (angle > 45.0f && angle <= 135.0f)
            direction = Direction.Up;
        else if (angle > -135.0f && angle <= -45.0f)
            direction = Direction.Down;
        else
            direction = Direction.Left;

        animator.SetInteger("Direction", (int)direction);

        // 移動ベクトル
        axisH = Mathf.Cos(rad);
        axisV = Mathf.Sin(rad);
    }

    void FixedUpdate()
    {
        // 移動
        rbody.linearVelocity = new Vector2(axisH, axisV) * speed;
    }

    // ★ Trigger に統一（矢が弾かれない）
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == ("Arrow"))
        {
            ArrowController arrow = collision.gameObject.GetComponent<ArrowController>();
            hp -= arrow.attackPower;

            if (hp <= 0)
            {
                Die();
            }
        }
    }

    // ★ 死亡処理を関数化（安全に停止）
    void Die()
    {
        isActive = false;
        animator.SetBool("IsActive", false);

        // コライダー無効化
        var col = GetComponent<Collider2D>();
        if (col != null) col.enabled = false;

        // 物理停止（暴走防止）
        if (rbody != null)
        {
            rbody.simulated = false;
        }

        // ★ Animator のパラメータ名は "isDead" に統一
        animator.SetTrigger("IsDead");

        // 1秒後に削除
        Destroy(gameObject, 1.0f);
    }
}