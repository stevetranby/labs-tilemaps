using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace ST
{
    /// <summary>
    /// Stats component.
    /// 
    /// PREFER INTs over FLOATS for better determinism, game saves, etc
    /// 
    /// TODO:
    /// - allow for general RPG stats with magic
    /// - allow for game with technology instead of magic (technology/weapons/items used instead of magic stats)
    /// </summary>
    public class StatsComponent : MonoBehaviour
    {
        public Entity entity;

        // TODO: do we want to encapsulate value + max into struct???
        // Health
        private GameObject healthBar;
        private int health = 100;
        private int healthMax = 100;

        // Experience
        private int xp = 0;
        private int level = 1; // cache, could be calculated
        private List<int> nextLevelXp; // xp required to level up (absolute for now: 10,20,50,100,200,...9999)

        // Stats
        private int intellect = 0;
        private int agility = 0; // rate of succesful dodge, escape battle,  
        private int strength = 0; // how much damage dealt
        private int dexterity = 0; // rate of successful attacks
        private int vitality = 0; // 
        private int resiliance = 0;
        private int luck = 0; // chance of inflicting critical hit

        // Combat
        private int defense = 10; // direct for tower defense, calculated or used as base value with armor + resiliance
        private int attack = 10; // direct for tower defense, calculated or used as base value with weapon + strength
        private int attackRadius = 0; // defines area for attack
        private bool attackFalloff = false; // determines if (exponential) falloff occurs for area attack

        // Items
        private int weaponAttack = 1;
        private int armorDefense = 3;
        // Misc
        private int magic;
        private int magicMax;
        private int energy;
        private int energyMax;

        // ----------------------------------------------------------------

        public int GetHealth ()
        {
            return health;
        }

        public void AdjustHealth (int healthAmount)
        {
            health += healthAmount;
            if (health > healthMax)
                health = healthMax;
            if (health <= 0) {
                // entity is dead
                entity.onDeath ();
            }
        }

        public float GetHealthPercent ()
        {
            return (float)health / (float)healthMax;
        }

        public int GetAttackPower ()
        {
            return attack + weaponAttack + strength/2;
        }

        public int GetDamagePower ()
        {
            return defense + armorDefense/2 + resiliance/4;
        }

        // ----------------------------------------------------------------


        // Use this for initialization
        void Start ()
        {
    
        }
    
        // Update is called once per frame
        void Update ()
        {
    
        }
    }
}