using UnityEngine;
using System.Collections;

namespace ST
{
    /// <summary>
    /// AI component. Should be base component and base class for all AI behavior
    /// </summary>
    public class AIComponent : MonoBehaviour
    {
        class AIBehavior
        {
        }
               
        public enum AIState
        {
            Searching, 
            Seeking,   
            Evading,
            Patrolling,
            Following,
            Idling
        }

        private Entity entity;
        private MoveComponent cachedMoveComponent;
        private StatsComponent cachedStatsComponent;

        // Timers for behavior
        private float searchTimer = 0.0f;
        private float attackTimer = 0.0f;
        private float timerUntilNextAttack = 0.0f; // time since last attacked target
        // Timer Thresholds
        private float timeBeforeChangeDirection = 5.0f;
        private float attackFrequence = 3.0f; // minimum time between being allowed to attack
        // TODO: use events/messages instead 
        public delegate void OnDamaged(float amount); // when damaged by another possibly perform action 

        //direction relative to the character to head.
        private Vector2 heading;
        private GameObject spawnedBy;
        private AIState currentState;

        // TODO: determine what can be target(s)
        private GameObject targetGO;
        private TileCoord targetTile;
        private Entity targetEntity;

        private void Awake () {
            // cache
            this.cachedMoveComponent = this.GetComponent<MoveComponent>();
            this.cachedStatsComponent = this.GetComponent<StatsComponent>();
            this.entity = this.GetComponent<Entity>();
            
            //set an initial heading.
            this.createNewHeading();
            
            //set internal timer for damagefrequency
                this.timerUntilNextAttack = this.attackFrequence;
            
            this.currentState = AIState.Searching;
        }

        private void Start()
        {
//            this.entity.OnDeath( () => {
//                //destroy the game object.
//                Destroy(this.gameObject);
//            });
        }

        private void Update () {
            //increment our damage timer by the time it took to complete the last frame.
            this.timerUntilNextAttack += Time.deltaTime;
        }
        
        /// <summary>
        /// Update that is kept in sync with physics.
        /// </summary>
        private void FixedUpdate()
        {
            //if we are searching, call the search function
            if(this.currentState == AIState.Searching)
            {
                this.Search();
            }else if(this.currentState == AIState.Following)
            {
                //if we are following, call the follow function
                this.Follow();
            }
        }
        
        /// <summary>
        /// The enemy follows the player.
        /// </summary>
        private void Follow()
        {
            //vector math says position to go minus current position = vector we should head in.
            Vector3 dirVector = this.targetGO.transform.position - this.transform.position;
            //make sure its of magnitude 1.
            dirVector.Normalize();
            //move that direction
            //character.Move(dirVector);
        }
        
        /// <summary>
        /// The enemy searches for the player.
        /// </summary>
        private void Search()
        {
            //increase the internal timer by 
            //the amount of time that has passed.
            this.searchTimer += Time.deltaTime;
            
            //determine if we need to create a new heading.
            if (this.searchTimer > this.timeBeforeChangeDirection)
            {
                //create a new heading
                this.createNewHeading();
                //reset the timer.
                this.searchTimer = 0.0f;
            }
            //move the character in the direction of its heading
            //character.Move(heading);
        }
        
        /// <summary>
        /// Generates a new random heading for the character
        /// and applies that heading immediately.
        /// </summary>
        private void createNewHeading()
        {
            //I used -100 to 100 to give an equal chance for any direction
            float xCoord = Random.Range(-100.0f, 100.0f);
            float yCoord = Random.Range(-100.0f, 100.0f);
            //set our heading to the random headings.
            heading = new Vector2(xCoord, yCoord);
            //make sure the magnitude of our heading is 1
            heading.Normalize();
        }
        
        /// <summary>
        /// Called when a collision happens between the character
        /// and something else.
        /// </summary>
        /// <param name="other">What you collided with.</param>
        private void OnCollisionEnter2D(Collision2D other)
        {
            //make sure we don't do too much damage to the player
            if (other.gameObject.tag == "Player"
                && this.attackTimer > this.attackFrequence)
            {
                // TODO: determine cost/benefit of sending event/message instead
                int damageAmount = cachedStatsComponent.GetAttackPower();
                StatsComponent otherStats = other.gameObject.GetComponent<StatsComponent>();
                if(otherStats != null) {
                    otherStats.AdjustHealth(-1 * damageAmount);
                }
                //reset the timer so we don't damage too much again.
                this.attackTimer = 0.0f;
            }
            //create a new heading, you ran into something.
            this.createNewHeading();
        }
        
        public void SetSpawnedBy(GameObject o)
        {
            this.spawnedBy = o;
        }
        
        public GameObject GetSpawnedBy()
        {
            return this.spawnedBy;
        }
        
        /// <summary>
        /// Occurs when something enters its trigger zone.
        /// </summary>
        /// <param name="other"></param>
        private void OnTriggerEnter2D(Collider2D other)
        {
            //check to see what this thing is
            if (other.gameObject.tag == "Player")
            {
                //Its the player!  FOLLOW IT!
                this.currentState = AIState.Following;
                //set the target.
                this.targetGO = other.gameObject;
            }
        }
        
        /// <summary>
        /// Occurs when something enters its trigger zone.
        /// </summary>
        /// <param name="other"></param>
        private void OnTriggerExit2D(Collider2D other)
        {
            //Check to see what it is
            if (other.gameObject.tag == "Player")
            {
                //Its the player, I can't see it anymore, stop following.
                this.currentState = AIState.Searching;
            }
        }
    }
}
