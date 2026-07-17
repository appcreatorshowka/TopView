using UnityEditor.Build.Content; 
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public float speed = 3.0f;
    Direction direction = Direction.Down;
    Vector2 moveVec;
    float angleZ = -90.0f;
    Rigidbody2D rbody;
    Animator animator;

    public float shootSpeed = 12.0f;
    public float shootDelay = 0.25f;
    public GameObject bowPrefab;
    public GameObject arrowPrefab;
    GameObject bowObj;
    public static int hasArrows = 10;
    InputAction attackAction;

    public static int hasSilverKeys = 0;

    float pressTime;

    public static float life = 1.0f;
    public GameState gameState; 
    bool inDamage = false;

    void OnLongPressStarted (InputAction.CallbackContext context)
    {
         bowObj.SetActive(true);   // 攻撃時に弓を表示 (0713)
         Debug.Log("Started;");
    }
    void OnLongPressPerformed(InputAction.CallbackContext context)
    {
        Debug.Log("Performed;");
    }
    void OnAttackCallback(InputAction.CallbackContext context)
    {
        
       bowObj.SetActive(false);   // 矢を放つと同時に弓をしまう (0713)     
       
        if (hasArrows > 0)
        {
            ShootArrow();
        }
    }

    void ShootArrow()
    {
        bowObj.transform.localScale = new Vector3(1, 1, 1);
        hasArrows -= 1;
        Quaternion r = Quaternion.Euler(0, 0, angleZ);
        // GameObject arrowObj = Instantiate(arrowPrefab, transform.position, r);
        // 矢を飛ばすための修正(0713)
        Vector2 forward = new Vector2(Mathf.Cos(angleZ * Mathf.Deg2Rad),Mathf.Sin(angleZ * Mathf.Deg2Rad));
        GameObject arrowObj = Instantiate(arrowPrefab,(Vector2)transform.position + forward * 0.3f,r);


        if (pressTime >= 1)
        {
            ArrowController arrow = arrowObj.GetComponent<ArrowController>();
            arrow.attackPower *= 2;
        }
        float x = Mathf.Cos(angleZ * Mathf.Deg2Rad);
        float y = Mathf.Sin(angleZ * Mathf.Deg2Rad);
        Vector2 v = new Vector2(x, y) * shootSpeed;
        Rigidbody2D body = arrowObj.GetComponent<Rigidbody2D>();
        if (body != null)
        {
            body.AddForce(v, ForceMode2D.Impulse);
        }

        GameManager gm = GameObject.FindAnyObjectByType<GameManager>();
        gm.UpdateItemCount(ItemType.Arrow, hasArrows);
        gm.UpdatePower(0);
    }

    float GetAngle()
    {
        float angle = angleZ;
        if(moveVec != Vector2.zero)
        {
            float rad = Mathf.Atan2(moveVec.y, moveVec.x);
            angle = rad * Mathf.Rad2Deg;
        }
        return angle;
    }
    Direction AngleToDirection()
    {
        Direction dir;
        if (angleZ >= -45 && angleZ < 45)
        {
            dir = Direction.Right;
        }
        else if (angleZ >= 45 && angleZ < 135)
        {
            dir = Direction.Up;
        }
        else if (angleZ >= -135 && angleZ < -45)
        {
            dir = Direction.Down;
        }
        else
        {
            dir = Direction.Left;
        }
        return dir;
    }

    void OnMove(InputValue value)
    {
        moveVec = value.Get<Vector2>();
    }


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rbody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        Vector3 pos = transform.position;
        bowObj = Instantiate(bowPrefab, pos, Quaternion.identity);
        bowObj.transform.SetParent(transform);
        bowObj.SetActive(false); // 通常時は弓を非表示 (0713)
        PlayerInput input = GetComponent<PlayerInput>();
        attackAction = input.currentActionMap.FindAction("Attack");
        attackAction.started += OnLongPressStarted;
        attackAction.performed += OnLongPressPerformed;
        attackAction.canceled += OnAttackCallback;
        InputActionMap uiMap = input.actions.FindActionMap("UI");
        uiMap.Disable();

        gameState = GameState.InGame;

        GameManager gm = GameObject.FindAnyObjectByType<GameManager>(); // GameManager取得
        gm.UpdateLife(life); // ライフを更新
        gm.UpdateItemCount(ItemType.Arrow, hasArrows); // 矢を更新
        gm.UpdateItemCount(ItemType.SilverKey, hasSilverKeys); // シルバーキーを更新



    }
    void OnDisable()
    {
        if (attackAction != null)
        {
            attackAction.started -= OnLongPressStarted;
            attackAction.performed -= OnLongPressPerformed;
            attackAction.canceled -= OnAttackCallback;
        }
    }

    // Update is called once per frame
    void Update()
    {

        if (gameState != GameState.InGame || inDamage)
        {
            if (inDamage)
            {
                float val = Mathf.Sin(Time.time * 50);
                if (val > 0)
                {
                    gameObject.GetComponent<SpriteRenderer>().enabled = true;
                }
                else
                {
                    gameObject.GetComponent<SpriteRenderer>().enabled = false;
                }
            }
            return;
        }
        
        angleZ = GetAngle();
        Direction dir = AngleToDirection();
        if (dir != direction)
        {
            direction = dir;
            animator.SetInteger("Direction", (int)direction);
        }

        // 弓を表示するための修正 (0713)
        // float bowZ = -1f;
             // if (angleZ > 30f && angleZ < 150f)
       //  {
       //     bowZ = 1f;
       //  }
        bowObj.transform.rotation = Quaternion.Euler(0, 0, angleZ);
        //  bowObj.transform.position = new Vector3(transform.position.x, transform.position.y, bowZ);
        bowObj.transform.position = new Vector3(transform.position.x, transform.position.y, 1f);




        if (hasArrows > 0)
        {
            pressTime = attackAction.GetTimeoutCompletionPercentage();
            if (pressTime > 0)
            {
                bowObj.transform.localScale = new Vector3(1 - (pressTime * 0.2f),
                                                          1 + (pressTime * 0.5f),
                                                          1);

                GameManager gm = GameObject.FindAnyObjectByType<GameManager>();
                gm.UpdatePower(pressTime);
            }

        }
       

    }

    private void FixedUpdate()
    {
        rbody.linearVelocity = moveVec * speed;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            GetDamage(collision.gameObject);
        }
    }

    void GetDamage(GameObject target)
    {
        if (gameState == GameState.InGame)
        {
            life -= 0.25f;
            GameManager gm = GameObject.FindAnyObjectByType<GameManager>();
            gm.UpdateLife(life);
            if (life > 0)
            {
                rbody.linearVelocity = Vector2.zero;
                Vector3 v = (transform.position - target.transform.position).normalized;
                rbody.AddForce(new Vector2(v.x * 4, v.y * 4), ForceMode2D.Impulse);
                inDamage = true;
                Invoke("DamageEnd", 0.25f);
            }
            else
            {
                GameOver();
            }
        }
    }
    void DamageEnd()
    {
        inDamage = false;
        gameObject.GetComponent<SpriteRenderer>().enabled = true;
    }
    void GameOver()
    {
        gameState = GameState.GameOver;
        // GetComponent<CircleCollider2D>().enabled = false;
        rbody.linearVelocity = Vector2.zero;  
        // rbody.gravityScale = 1;
        rbody.AddForce(new Vector2(0, 5), ForceMode2D.Impulse);
         animator.SetTrigger("IsDead");

        PlayerInput input = GetComponent<PlayerInput>();
        input.currentActionMap.Disable();
        input.SwitchCurrentActionMap("UI");
        input.currentActionMap.Enable();

    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Item")
        {
            GameManager gm = GameObject.FindAnyObjectByType<GameManager>();
            Item item = collision.gameObject.GetComponent<Item>();
            if (ItemType.SilverKey == item.itemdata.type)
            {
                hasSilverKeys++;
                gm.UpdateItemCount(ItemType.SilverKey, hasSilverKeys);

            }
            else if (ItemType.Arrow == item.itemdata.type)
            {
                hasArrows += (int)item.itemdata.value;
                gm.UpdateItemCount(ItemType.Arrow, hasArrows);
            }
            else if (ItemType.Life == item.itemdata.type)
            {
                if (life < 1.0f)
                {
                    life = Mathf.Clamp(life + 0.25f, 0, 1);
                }
            }
        }
    }

    void OnSubmit(InputValue value)
    {
        if (gameState != GameState.InGame)
        {
            GameManager gm = GameObject.FindAnyObjectByType<GameManager>();
            gm.Retry();
        }
    }
    
}
