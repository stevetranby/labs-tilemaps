using System.Collections;
using System.Collections.Generic;
using System;

namespace ST
{
    public class EntityManager
    {
        private List<Entity> entities;
//        private List<Entity> playerUnits;
//        private List<Entity> enemyUnits;
//        private List<Entity> npcUnits;   

        static private EntityManager _instance = null;
        static public EntityManager GetInstance()
        {
            if(_instance == null) {
                _instance = new EntityManager();
            }
            return _instance;
        }

        public EntityManager() 
        {
            entities = new List<Entity>();
//            playerUnits = new List<Entity>();
//            enemyUnits = new List<Entity>();
//            npcUnits = new List<Entity>();
        }

        public Entity SpawnEntity(Map map, EntityConfig config)
        {
            // parse config
            Entity e = new Entity(map);
            return e;
        }

        public Entity GetEntity(int guid)
        {
            return null;
        }

        public List<Entity> GetEntitiesByName(string name)
        {
            return null;
        }

        public List<Entity> GetEntitiesByType(string type)
        {
            return null;
        }

        public Entity GetEntity(Predicate<Entity> predicate)
        {
            return entities.Find(predicate);
        }

        public List<Entity> GetEntities(Predicate<Entity> predicate)
        {
            return entities.FindAll(predicate);
        }
    }
}
