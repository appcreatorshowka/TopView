using UnityEngine;
using static UnityEditor.Progress;

public class Item : MonoBehaviour
{
    public ItemData itemdata;
    public int arrangeId = 0;   //配列の識別に使う

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GetComponent<SpriteRenderer>().sprite = itemdata.itemSprite;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // PlayerDownしかアイテムが取れないような強制プログラム (0716)
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return;
        if (collision.name != "PlayerDown") return; // ★追加

        // (0730)
        if (itemdata.type == ItemType.Light)
        {
            // ライトは壊さない（Player側でカウントする）
            return;
        }
        else
        {
            // ライト以外は壊す
            Destroy(gameObject);
        }

        //Debug.Log("Item Picked by: " + collision.name);
        //Destroy(gameObject);
    }

}
