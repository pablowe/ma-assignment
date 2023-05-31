using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AiMoveSystem
{
    private HintSystem hintSystem;

    public AiMoveSystem(HintSystem hintSystem)
    {
        this.hintSystem = hintSystem;
    }

    public Vector2Int? GetMoveCoordinates()
    {
        return hintSystem.GetHintCoordinates();
    }
}
