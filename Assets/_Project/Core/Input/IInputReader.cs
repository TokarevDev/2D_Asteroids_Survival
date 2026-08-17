using UnityEngine;

namespace Game.Core.Input
{
    public interface IInputReader
    {
        Vector2 MoveDirection { get; }
    }
}
