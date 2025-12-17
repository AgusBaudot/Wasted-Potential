using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TackTile : MonoBehaviour
{
    [Header("State")]
    [SerializeField] private int currentTacks = 0;
    private const int MAX_TACKS = 5;

    // We cache enemies currently standing on this tile
    private List<EnemyBase> enemiesOnTile = new List<EnemyBase>();
    private Coroutine activeBurnRoutine;

    // --- Public Access for the Tower ---
    public bool IsFull => currentTacks >= MAX_TACKS;
    public int CurrentTacks => currentTacks;

    public void AddTack(int amount = 1)
    {
        if (currentTacks < MAX_TACKS)
        {
            currentTacks = Mathf.Min(currentTacks + amount, MAX_TACKS);
            UpdateVisuals(); // Implement your spike visuals here
            
            // If enemies are already here, ensure the DoT loop is running
            CheckBurnState();
        }
    }

    // --- Collision Logic ---
    private void OnTriggerEnter(Collider other)
    {
        // Assuming your enemies have the EnemyBase component
        if (other.TryGetComponent(out EnemyBase enemy))
        {
            enemiesOnTile.Add(enemy);

            // 1. ENTRY BURST: Immediate Damage & Consumption
            if (currentTacks > 0)
            {
                // Hardcoded 2 damage per prompt, or pass this in via the Tower later
                enemy.ApplyDamage(2, null); 
                ConsumeTack();
            }

            CheckBurnState();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out EnemyBase enemy))
        {
            if (enemiesOnTile.Contains(enemy))
                enemiesOnTile.Remove(enemy);
        }
    }

    // --- The "1 Tack Per Second" Logic ---
    private void CheckBurnState()
    {
        if (activeBurnRoutine == null && currentTacks > 0 && enemiesOnTile.Count > 0)
        {
            activeBurnRoutine = StartCoroutine(BurnRoutine());
        }
    }

    private IEnumerator BurnRoutine()
    {
        // Wait 1 second before first tick (since Enter handled the immediate hit)
        yield return new WaitForSeconds(1f);

        while (currentTacks > 0 && enemiesOnTile.Count > 0)
        {
            // 1. Deal Damage to ALL enemies
            // Iterate backwards to safely handle enemies dying/nulling mid-loop
            for (int i = enemiesOnTile.Count - 1; i >= 0; i--)
            {
                if (enemiesOnTile[i] == null || !enemiesOnTile[i].gameObject.activeInHierarchy)
                {
                    enemiesOnTile.RemoveAt(i);
                    continue;
                }
                enemiesOnTile[i].ApplyDamage(2, null);
            }

            // 2. Consume ONLY 1 tack (Efficiency rule)
            ConsumeTack();

            yield return new WaitForSeconds(1f);
        }

        activeBurnRoutine = null;
    }

    private void ConsumeTack()
    {
        currentTacks--;
        UpdateVisuals();
    }

    private void UpdateVisuals()
    {
        // Create a visual representation of how many tacks are left
    }
}