using UnityEngine;

public class Door : MonoBehaviour
{
    public int arrangeID = 0; //　配置の識別に使う
    public bool IsDoor = false; // ドア

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnCollisionEnter2D(Collision2D collision)
    {

        if (collision.gameObject.tag == "Player")
        {
            // カギを持っている
            if (IsDoor)
            {
                if (PlayerController.hasSilverKeys > 0)
                {
                    PlayerController.hasSilverKeys--; //　カギを消費する
                    Destroy(this.gameObject); //　ドアを開ける
                }
            }


        }

    }
}