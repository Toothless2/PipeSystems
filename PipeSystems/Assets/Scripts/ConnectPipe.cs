using UnityEngine;
using System.Collections;

namespace Pipes
{
    public class ConnectPipe : MonoBehaviour
    {
        public GameObject pipeNub;
        public GameObject[] spawnedNubs = new GameObject[6];

        private float waitTime;
        private RaycastHit hit;
        private Vector3 thisPipesPositon;

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
            GetComponent<PipeConnections>().connectionDirections[arrayIndex].direction = direction;

            //sets the rotation of the nub correctly depending on what direction was passed into the function
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

            //checks if a pipe exists in that direction
            if(Physics.Raycast(thisPipesPositon, direction, out hit, 1))
            {
                //if their already is a nub at that index tht is passed into the function dont spawn another one
                if(spawnedNubs[arrayIndex] == null)
                {
                    if (hit.transform.tag == "Pipe")
                    {
                        //spawns a nub(connection) and sets its parent to the pipe object to keep the inspector tidy
                        spawnedNubs[arrayIndex] = (GameObject)Instantiate(pipeNub, thisPipesPositon + (direction * 0.35f), Quaternion.Euler(rotation));
                        spawnedNubs[arrayIndex].transform.SetParent(transform);
                    }
                }
            }
            else
            {
                //if their is no pipe and their was a connection at that point destry the connection
                if(spawnedNubs[arrayIndex] != null)
                {
                    Destroy(spawnedNubs[arrayIndex]);
                    spawnedNubs[arrayIndex] = null;
                }
            }
        }
    }
}