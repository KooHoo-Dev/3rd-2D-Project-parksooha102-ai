using UnityEngine;

namespace _2DProject
{
    public enum CombatEventType
    {
        DamageEvent,
        HealEvent,
    }

    public struct CombatEvent
    {
        public CombatEventType EventType;
        public int Amount;
        public Vector3 Position;
    }

    public interface ICombatObserver
    {
        void OnDamageTaken(CombatEntity sender, CombatEntity receiver, CombatEvent @event);
        void OnHealTaken(CombatEntity sender, CombatEntity receiver, CombatEvent @event);
    }
}