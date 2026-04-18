using System;
using System.Collections.Generic;
using UnityEngine;

namespace Project.Scripts.GamePlay.Player.Stealing
{
    [Serializable]
    public class StealParameters
    {
        [field:SerializeField] public string Description { get; private set; }
        [field: SerializeField] public bool CanUnlockInstantly { get; private set; }//YES
        
        [field: SerializeField] public bool CanUnlockAtTerminal { get; private set; }
        
        [field: SerializeField] public bool CanUnlockAtKill { get; private set; } //YES 
        [field: SerializeField] public bool CanUnlockAtKillWithSameSpell { get; private set; }//YES

        [field:Space]

        [field:SerializeField] public bool CanUnlockStealingSeveralTimes { get; private set; }//yes
        [field:SerializeField] public int AmountOfSteals { get;private set; }
        [field:Space]
        [field:SerializeField] public bool CanUnlockDealingCertainAmountOfDamage { get; private set; } //YES
        [field:SerializeField] public float AmountOfDamageToDeal { get;private set; }
        // [field:Space]
        //
        // [field:SerializeField] public bool CanUnlockBlockingCertainAmountOfDamage { get; private set; }
        // [field:SerializeField] public float AmountOfDamageToBlock { get;private set; }
        [field:Space]

        [field:SerializeField] public bool CanUnlockByKillingCertainAmountOfEnemies { get; private set; }//YES
        [field:SerializeField] public int AmountOfEnemiesToKill { get;private set; }
        [field:Space]

        [field:SerializeField] public bool CanUnlockByHavingCertainSpeed { get; private set; }//YES
        [field:SerializeField] public float ValueOfSpeed { get;private set; }
        [field:Space]

        [field:SerializeField] public bool CanUnlockByNotHavingOtherSpells { get; private set; }//YES
        //[field: SerializeField] public bool CanUnlockWithGloryKill { get; private set; }


        //survive with this spell
        public string GetDescription()
        {
            if (!string.IsNullOrWhiteSpace(Description))
                return Description;

            List<string> parts = new List<string>();

            if (CanUnlockAtTerminal)
                parts.Add("Can Unlock At Terminal");

            if (CanUnlockStealingSeveralTimes)
                parts.Add($"Can Unlock Stealing {AmountOfSteals} Times");

            if (CanUnlockInstantly)
                parts.Add("Can Unlock Instantly");

            if (CanUnlockAtKill)
                parts.Add("Can Unlock At Kill");

            if (CanUnlockAtKillWithSameSpell)
                parts.Add("Can Unlock At Kill With Same Spell");

            if (CanUnlockDealingCertainAmountOfDamage)
                parts.Add($"Can Unlock By Dealing {AmountOfDamageToDeal} Damage");

            if (CanUnlockByKillingCertainAmountOfEnemies)
                parts.Add($"Can Unlock By Killing {AmountOfEnemiesToKill} Enemies");

            if (CanUnlockByHavingCertainSpeed)
                parts.Add($"Can Unlock By Having Speed {ValueOfSpeed}");

            if (CanUnlockByNotHavingOtherSpells)
                parts.Add("Can Unlock By Not Having Other Spells");

            return parts.Count > 0 ? string.Join(", ", parts) : "?????";
        }
    }
}