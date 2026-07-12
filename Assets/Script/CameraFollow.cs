using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;   // プレイヤー
    public float smooth = 5f;  // 追従の滑らかさ

    void LateUpdate()
    {
        if (target == null) return;

        Vector3 pos = transform.position;
        pos.x = target.position.x;
        pos.y = target.position.y;

        transform.position = Vector3.Lerp(transform.position, pos, smooth * Time.deltaTime);
    }
}

