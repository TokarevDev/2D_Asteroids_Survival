using UnityEngine;

namespace Game.Core
{
    public interface IInputReader
    {
        Vector2 MoveDirection { get; }
    }
}
