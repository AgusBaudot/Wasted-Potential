using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour
{
    public Vector2Int GridPosistion { get; private set; }
    public CardData Data { get; private set; }
    
    //Called by factory/command immediately after creation.
    public void Initialize(CardData data, Vector2Int gridPosition)
    {
        Data = data;
        GridPosistion = gridPosition;
        //Set visuals, range, damage, etc using Data.
    }
}
