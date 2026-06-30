using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;
using System.Collections;
 
public class FightManager : MonoBehaviour
{
    [SerializeField]
    private UnityEvent onFightReady;
    [SerializeField]
    private UnityEvent onCancelFinght;
    [SerializeField]
    private UnityEvent onFinghtStart;
    [SerializeField]
    private int minimumFighters = 2;
    [SerializeField]
    private int maximumFighters = 2;
    [SerializeField]
    private PoolManager poolManager;
    private List<Fighter> fighters = new List<Fighter>();
    public void AddFighter(Fighter fighter)
    {
        if (fighters.Count < maximumFighters && !fighters.Contains(fighter))
        {
            poolManager.GetObject(fighter.FighterData.appearParticles, fighter.transform.position);
            SoundManager.instance.Play(fighter.FighterData.appearSoundName);
            fighters.Add(fighter);
            if (fighters.Count >= minimumFighters)
            {
                onFightReady?.Invoke();
            }
        }
    }
    public void RemoveFighter(Fighter fighter)
    {
        if (fighters.Contains(fighter))
        {
            fighters.Remove(fighter);
            if (fighters.Count< minimumFighters)
            {
                onCancelFinght?.Invoke();
            }
        }
    }
    public void StartFigh()
    {
        onFinghtStart?.Invoke();
        StartCoroutine(FightCoroutine());
    }
    private IEnumerator FightCoroutine()
    {
        foreach (Fighter fighter in fighters)
        {
            fighter.Health.InitializeHealth();
        }
        while (fighters.Count > 1)
        {
        Fighter attacker = fighters[Random.Range(0, fighters.Count)];
        Fighter defender = fighters[Random.Range(0, fighters.Count)];
        while (defender == attacker)
            {
                defender = fighters[Random.Range(0, fighters.Count)];
            }
            attacker.transform.LookAt(defender.transform);
            attacker.transform.LookAt(attacker.transform);
            AttackData attackData = attacker.FighterData.GetRandomAttack();
            attacker.Animator.Play("Change", 0, 0f);
            yield return new WaitForSeconds (attacker.FighterData.chargeTime);
            poolManager.GetObject(attackData.chargeParticles, attacker.transform.position);
            attacker.Animator.Play(attackData.animationName, 0, 0f);
            SoundManager.instance.Play(attackData.attackSoundName);
            yield return null;
            yield return new WaitForSeconds(attacker.Animator.GetCurrentAnimatorStateInfo(0).length);
            poolManager.GetObject(attackData.attackParticles, defender.transform.position);
            Health defenderHealth = defender.GetComponent<Health>();
            SoundManager.instance.Play(defender.FighterData.damageSoundName);
            defenderHealth.TakeDamage(Random.Range(attackData.minDamage, attackData.maxDamage));
            if (defenderHealth.CurrentHealth <= 0)
            {
                SoundManager.instance.Play(defender.FighterData.deadSoundNmae);
                RemoveFighter(defender);
                FighterWin(attacker);
            }
            yield return new WaitForSeconds(1.5f);
        }
    }
    private void FighterWin(Fighter winner)
    {
        Debug.Log(winner.name + " wins the fight!");
    }
   
}