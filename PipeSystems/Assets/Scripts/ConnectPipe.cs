using UnityEngine;
using System.Collections;

namespace Pipes
{
    public class ConnectPipe : MonoBehaviour
    {
        public GameObject pipeNub;

        private float waitTime;
        private RaycastHit hit;
        private Vector3 thisPipesPositon;
        private GameObject spawnedNub;


        GameObject[] spawnedNubs = new GameObject[6];

        // Use this for initialization
        void Start()
        {
            thisPipesPositon = transform.position;
            CheckForPipes(Vector3.forward, 0);
            CheckForPipes(Vector3.back, 1);
            CheckForPipes(Vector3.up, 2);
            CheckForPipes(Vector3.down, 3);
            CheckForPipes(Vector3.right, 4);
            CheckForPipes(Vector3.left, 5);
        }

        // Update is called once per frame
        void Update()
        {
            if(Time.time > waitTime)
            {
                waitTime = Time.time + Random.Range(0.5f, 3f);
                CheckForPipes(Vector3.forward, 0);
                CheckForPipes(Vector3.back, 1);
                CheckForPipes(Vector3.up, 2);
                CheckForPipes(Vector3.down, 3);
                CheckForPipes(Vector3.right, 4);
                CheckForPipes(Vector3.left, 5);
            }
        }

        void CheckForPipes(Vector3 direction, int arrayIndex)
        {
            Vector3 rotation;

            if(direction == Vector3.down || direction == Vector3.up)
            {
                rotation = new Vector3(90, 0, 0);
            }   
            else if(direction == Vector3.right || direction == Vector3.left)
            {
                rotation = new Vector3(0, 90, 0);
            }
            else
            {
                rotation = Vector3.zero;
            }

            if(Physics.Raycast(thisPipesPositon, direction, out hit, 1))
            {
                if(spawnedNubs[arrayIndex] == null)
                {
                    if (hit.transform.tag == "Pipe")
                    {
                        spawnedNubs[arrayIndex] = (GameObject)Instantiate(pipeNub, thisPipesPositon + (direction * 0.35f), Quaternion.Euler(rotation));
                        spawnedNubs[arrayIndex].transform.SetParent(transform);
                    }
                }
            }
        }
    }
}