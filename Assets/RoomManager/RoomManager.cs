using UnityEngine;
using UnityEngine.SceneManagement;

public class RoomManager : MonoBehaviour
{
     public static int doorNumber =  0; //ドア番号


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // プレイヤーキャラクター位置
        // 出入口を配列で得る
        GameObject[] enters = GameObject.FindGameObjectsWithTag("Exit");
        for (int i = 0; i < enters.Length; i++)
        {
            GameObject doorObj = enters[i]; //配列から取り消す
            Exit exit = doorObj.GetComponent<Exit>(); // Exitクラス取得
            if ( doorNumber == exit.doorNumber) //ドア番号が一致するか
            {
                // ドア番号同じ
                // プライヤーキャラクター出入口に移動
                float x = doorObj.transform.position.x;
                float y = doorObj.transform.position.y;
                if (exit.direction == Direction.Up)
                {
                    y += 1;
                }
                else if (exit.direction == Direction.Down)
                {
                    y -= 1;
                }
                else if (exit.direction == Direction.Left)
                {
                    x -= 1;
                }
                else if (exit.direction == Direction.Right)
                {
                    x += 1;
                }
                GameObject player = GameObject.FindGameObjectWithTag("Player");
                player.transform.position = new Vector3(x, y);
                break; // ループ終了


            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //　シーン遷移
    public static void ChangeScene(string sceneName, int doorNum)
    {
        doorNumber = doorNum; //ドア番号をセット
        SceneManager.LoadScene(sceneName); //シーン遷移
    }
}
