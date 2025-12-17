using System;
using TMPro;
using UnityEngine;

public class PlayerHealthManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _healthText;
    
    public event Action<int> OnHealthChanged;
    public event Action OnDeath;
    
    [SerializeField] private int maxHealth = 100;
    
    private int _currentHealth;

    private void Awake()
    {
        ServiceLocator.Register(this);
        _currentHealth = maxHealth;
    }

    public void ApplyDamage(int amount)
    {
        _currentHealth -= amount;
        OnHealthChanged?.Invoke(_currentHealth);
        if (_currentHealth <= 0)
            OnDeath?.Invoke();
        else
        {
            _healthText.GetComponent<UITextShake>().Shake();
            GetComponent<AudioSource>().Play();
        }
    }

    private void Die()
    {
        //Do game over or something.
    }
}