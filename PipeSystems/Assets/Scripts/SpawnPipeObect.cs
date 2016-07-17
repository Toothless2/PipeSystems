using UnityEngine;
using System.Collections;

namespace Pipes
{
    public class SpawnPipeObect : MonoBehaviour
    {
        public bool shouldSpawn;
        public GameObject pipeItem;
        private GameObject spawnedItem;
        private float waitTime;
        
        void Start()
        {
            if (shouldSpawn)
            {
                waitTime = Time.time + 5;
                spawnedItem = Instantiate(pipeItem);
                spawnedItem.GetComponent<Transform>().position = transform.position;
            }
        }
    }
}