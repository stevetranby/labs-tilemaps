using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace ST
{     
    public class JobsComponent : MonoBehaviour
    {
        // FFT job tree
        public class Job
        {
            public enum JobType
            {
                Noob,
                Paladain,                
                Warrior,
                Magician,
                Wizard,
                Blacksmith,
                Healer,
                Ninja,
                NinjaWarrior, // or allow job bitmask for multiples
                Clown,
                Lawyer,
                Chief,
                Thief,
                Cracker,
                Grenadier,
                ExplosiveExpert,
                etc
            }
            string name;
            int jobLevel;
            int timesActive;

            public enum WeaponType {
                Magic,
                Melee,
                Projectile
            }

            // what can this job use?
            bool canUseMagic;
            WeaponType weaponType; 
            int usableWeaponsMask; 

            //TODO: refactor RPG elements from stats into data struct
            public struct RpgStats
            {
                // TODO: consider properties to protect devs
                public int attack;
                public int defense;
                public int intellect;
                public int agility;

                public int getAttackDamage ()
                {
                    return attack + intellect;
                }

                public int getDamageDefense ()
                {
                    return defense + agility;
                }
            }
        }

        private int jobPoints; // JP in FFT
        private Job activeJob; // could allow multiples
        private List<Job> jobs;

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