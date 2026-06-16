using UnityEngine;

public class Fighter : MonoBehaviour
{
   [SerializeField]
   private Animator animator;
   [SerializeField]
   private Health health;
   public Health Health => health;
   [SerializeField]
   private FighterData fighterData;
   private FighterData FighterData => fighterData;
   private void Awake()
   {
    health.MaxHealth = fighterData.maxHealth;
   }
   public void TakeDamage()
   {
    animator.Play("Damage", 0, 0f);
   }
   public void Die()
   {
    animator.Play("Die", 0, 0f);
   }
}
