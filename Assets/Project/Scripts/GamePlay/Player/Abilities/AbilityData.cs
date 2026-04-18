using System;
using System.Collections.Generic;
using Project.Scripts.Common.IdProvider;
using Project.Scripts.GamePlay.Player.Abilities.Configs;
using Project.Scripts.GamePlay.Player.Abilities.General;
using UnityEngine;

namespace Project.Scripts.GamePlay.Player.Abilities
{
    public class AbilityData
    {
        public BaseAbilityConfig AbilityConfig;
        public int OriginCasterId;
        public int UsesLeft = -1;
        public bool IsLimited => UsesLeft != -1;
        public int Id { get;  }
        public event Action AbilityUnlocked;

        public AbilityData(BaseAbilityConfig abilityConfig, int originCasterId)
        {
            AbilityConfig = abilityConfig;
            OriginCasterId = originCasterId;
            Id=IdProvider.GetNext<AbilityData>();
        }



        public void SetAsUnlocked()
        {
            UsesLeft = -1;
            AbilityUnlocked?.Invoke();
        }
    }
}