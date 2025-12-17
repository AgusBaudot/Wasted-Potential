using UnityEngine;
using System.Collections.Generic;
using System.Linq; // For sorting if needed

public class TackFactoryController : MonoBehaviour
{
    [Header("References")]
    // We reference the CardData to stay consistent with your system's stats
    public CardData myData; 
    public LayerMask tileLayer; // Set this to the layer your Ground Tiles are on

    [Header("Runtime")]
    private float fireTimer;
    private List<TackTile> tilesInRange = new List<TackTile>();
    private int currentTileIndex = 0; // For circular order

    void Start()
    {
        if(myData == null)
        {
            Debug.LogError("CardData not assigned on Tack Factory!");
            return;
        }

        InitializeTiles();
    }

    void Update()
    {
        if (myData == null) return;

        fireTimer -= Time.deltaTime;

        if (fireTimer <= 0f)
        {
            TryDispenseTack();
        }
    }

    private void InitializeTiles()
    {
        tilesInRange.Clear();
        // Find all colliders in range based on CardData.range
        Collider[] hits = Physics.OverlapSphere(transform.position, myData.range, tileLayer);

        foreach (var hit in hits)
        {
            if (hit.TryGetComponent(out TackTile tile))
            {
                tilesInRange.Add(tile);
            }
        }

        // Optional: Sort by distance so the "Circular Order" feels logical visually
        tilesInRange.Sort((a, b) => 
            Vector3.Distance(transform.position, a.transform.position)
            .CompareTo(Vector3.Distance(transform.position, b.transform.position))
        );
    }

    private void TryDispenseTack()
    {
        if (tilesInRange.Count == 0) return;

        // Check if we need to resume firing (Logic: stops if all full)
        bool anySpaceAvailable = false;
        foreach(var tile in tilesInRange)
        {
            if (!tile.IsFull)
            {
                anySpaceAvailable = true;
                break;
            }
        }

        if (!anySpaceAvailable) return; // Do nothing, wait for space to clear

        // CIRCULAR DISTRIBUTION LOGIC
        // We loop through the list starting at currentTileIndex
        // We try to find the *next* available tile.
        int attempts = 0;
        while (attempts < tilesInRange.Count)
        {
            TackTile target = tilesInRange[currentTileIndex];
            
            // Prepare index for next time (Circular)
            currentTileIndex = (currentTileIndex + 1) % tilesInRange.Count;
            attempts++;

            if (!target.IsFull)
            {
                // Found a valid tile!
                FireTackAt(target);
                
                // Reset timer based on CardData.fireRate
                // If fireRate is "Shots per second", do 1/fireRate. 
                // If it's "Delay", just use fireRate. Assuming Delay here:
                fireTimer = myData.fireRate; 
                return;
            }
        }
    }

    private void FireTackAt(TackTile tile)
    {
        // 1. Logic
        tile.AddTack();

        // 2. Visuals
        // Even though we aren't using the standard TowerAbility.Fire, 
        // we can still instantiate the visualVFX if you have one on the card.
        if (myData.ability is InstantAttack instantAttack && instantAttack.visualVFX != null)
        {
             Instantiate(instantAttack.visualVFX, tile.transform.position, Quaternion.identity);
        }
    }
    
    // Debug Range
    void OnDrawGizmosSelected()
    {
        if(myData != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, myData.range);
        }
    }
}