using UnityEngine;
using System.Collections;

namespace Pipes
{
    public class SpawnPipeObect : MonoBehaviour
    {
        public GameObject pipeItem;
        private GameObject spawnedItem;
        private float waitTime;
        
        void Start()
        {
            waitTime = Time.time + 5;
            spawnedItem = Instantiate(pipeItem);
            spawnedItem.GetComponent<Transform>().position = transform.position;
        }

        void Update()
        {
            if(Time.time > waitTime)
            {
                waitTime = Time.time + 1f;
                //Instantiate(pipeItem, transform.position, transform.rotation);
            }
        }
    }
}