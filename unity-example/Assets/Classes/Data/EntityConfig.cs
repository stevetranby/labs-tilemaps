using System.Collections;
using System.Collections.Generic;

namespace ST
{
    /// <summary>
    /// Entity config to parse and spawn a given type of entity.
    /// </summary>
    //[UnityEngine.Serialization]
    public class EntityConfig
    {
        public class WeaponConfig
        {
            public enum WeaponType {
                Magic,
                Melee,
                Projectile
            }

            public string name;
            public WeaponType type;
        }

        public int entityHash;
        public string name;
        public int attack;
        public int defense;
        public WeaponConfig weaponConfig;

    }
}
