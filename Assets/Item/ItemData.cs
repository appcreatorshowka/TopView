using UnityEngine;

// アイテムタイプ
public enum ItemType
{
    SilverKey,
    Arrow,
    Life,
    Light,
}
[CreateAssetMenu(menuName = "Item/Game Item", fileName = "GameItem")]
public class ItemData : ScriptableObject
{
    public float value = 0;
    public ItemType type;
    public string itemName = "";
    public Sprite itemSprite;
}

