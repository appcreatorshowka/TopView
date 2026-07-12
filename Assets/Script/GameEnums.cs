using UnityEngine;

// 共通の列挙型を別ファイルに移して全スクリプトで参照できるようにします
public enum Direction
{
    Down = 0,
    Left = 1,
    Up = 2,
    Right = 3
}

public enum GameState
{
    InGame,
    GameClear,
    GameOver,
    GameEnd,
}
