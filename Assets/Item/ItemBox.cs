using UnityEngine;

public class ItemBox : MonoBehaviour
{
    public Sprite openImage;
    public GameObject itemPrefab;
    public bool isClosed = true;

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (!isClosed || collision.gameObject.tag != "Player") return;

        // 宝箱を開く
        GetComponent<SpriteRenderer>().sprite = openImage;
        isClosed = false;

        // アイテム生成（少し上にずらす）
        Vector3 spawnPos = transform.position + new Vector3(0, 0.1f, 0);
        GameObject key = Instantiate(itemPrefab, spawnPos, Quaternion.identity);

        // 手前に表示
        key.GetComponent<SpriteRenderer>().sortingOrder = 10;

        // ★ふわっと中はコライダー無効化（絶対に取れない）
        Collider2D col = key.GetComponent<Collider2D>();
        if (col != null) col.enabled = false;

        // ふわっと演出開始
        StartCoroutine(Fuwa(key, col));
    }

    System.Collections.IEnumerator Fuwa(GameObject key, Collider2D col)
    {
        Vector3 start = key.transform.position;
        Vector3 end = start + new Vector3(0, 0.5f, 0);

        float t = 0;
        while (t < 0.5f)
        {
            t += Time.deltaTime;
            key.transform.position = Vector3.Lerp(start, end, t / 0.5f);
            yield return null;
        }

        // ★ふわっと終わったらコライダー有効化（取れるようになる）
        if (col != null) col.enabled = true;
    }
}
