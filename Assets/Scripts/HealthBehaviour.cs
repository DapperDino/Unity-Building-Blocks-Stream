using System;
using UnityEngine;

namespace DapperDino.BuildingBlocks
{
    public class HealthBehaviour : MonoBehaviour
    {
        [SerializeField] private int maxHealth = 1;

        public event Action<HealthBehaviour> OnDeath;
        public event Action<int, int> OnHealthChanged;

        private int health;
        public int Health
        {
            get => health;
            private set
            {
                health = value;
                OnHealthChanged?.Invoke(health, maxHealth);
            }
        }

        public int MaxHealth => maxHealth;

        private void Awake() => Health = maxHealth;

        private void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag("Projectile")) { return; }

            Health--;

            if (Health != 0) { return; }

            OnDeath?.Invoke(this);

            gameObject.SetActive(false);
        }
    }
}
