using UnityEngine;
using System.Collections;

namespace Pipes
{
    public class ConnectPipe : MonoBehaviour
    {
        public GameObject pipeConnector;
        public GameObject pipeCap;
        public LayerMask checkedLayers;
        public LayerMask ignoredLayers;
        [Tooltip("The Pipes Z - 0.5")]
        public float pipeSize;
        [Range(0.01f, 4.2f)]
        public float pipeSpeed;

        [System.NonSerialized]
        public GameObject[] pipeConnectors = new GameObject[6];
        public GameObject[] connectedPipes = new GameObject[6];
        private GameObject[] pipeCaps = new GameObject[6];

        private float waitTime;
        private RaycastHit hit;
        public Vector3 thisPipesPositon;

        // Use this for initialization
        void OnEnable()
        {
            GetComponent<Transform>().localScale = new Vector3(pipeSize, pipeSize, pipeSize);

            thisPipesPositon = transform.position;

            //checks every face of the pipe for another pipe
            CheckForPipes(Vector3.forward, 0);
            CheckForPipes(Vector3.back, 1);
            CheckForPipes(Vector3.up, 2);
            CheckForPipes(Vector3.down, 3);
            CheckForPipes(Vector3.left, 5);
            CheckForPipes(Vector3.right, 4);
        }

        public void DestroyPipe()
        {
            Destroy(gameObject);
        }

        public void UpdateConnectios()
        {
            thisPipesPositon = transform.position;

            //checks every face of the pipe for another pipe
            UpdateConnectionsCheck(Vector3.forward, 0);
            UpdateConnectionsCheck(Vector3.back, 1);
            UpdateConnectionsCheck(Vector3.up, 2);
            UpdateConnectionsCheck(Vector3.down, 3);
            UpdateConnectionsCheck(Vector3.left, 5);
            UpdateConnectionsCheck(Vector3.right, 4);
        }

        void UpdateConnectionsCheck(Vector3 direction, int arrayIndex)
        {
            //if their is not a pipe in the direction add a cap to that side
            if(!Physics.Raycast(thisPipesPositon, direction, out hit, 1, checkedLayers))
            {
                if(pipeConnectors[arrayIndex] != null)
                {
                    Destroy(pipeConnectors[arrayIndex]);

                    SpawnPipeCap(direction, arrayIndex);
                    connectedPipes[arrayIndex] = null;

                    if(GetComponent<RoutingPipe>() != null)
                    {
                        if (GetComponent<RoutingPipe>().isRoutingPipe)
                        {
                            GetComponent<RoutingPipe>().ForceUpdate();
                        }
                    }
                }
            }
        }

        void CheckForPipes(Vector3 direction, int arrayIndex)
        {
            //checks if a pipe exists in that direction
            if(Physics.Raycast(thisPipesPositon, direction, out hit, 1, checkedLayers))
            {
                if(hit.transform != transform)
                {
                    //if their already is a nub at that index tht is passed into the function dont spawn another one
                    if (pipeConnectors[arrayIndex] == null)
                    {
                        if (hit.transform.tag == "Pipe")
                        {
                            //removes the pipe cap from the pipe so items cam travel through them
                            RemovePipeCap(arrayIndex);

                            //addas the connecting pipe to the array
                            connectedPipes[arrayIndex] = hit.transform.gameObject;

                            //spawns a nub(connection) and sets its parent to the pipe object to keep the inspector tidy
                            pipeConnectors[arrayIndex] = (GameObject)Instantiate(pipeConnector, thisPipesPositon + (direction * pipeSize), Quaternion.Euler(0, 0, 0));
                            pipeConnectors[arrayIndex].transform.LookAt(transform.position);
                            pipeConnectors[arrayIndex].transform.SetParent(transform);
                            pipeConnectors[arrayIndex].GetComponent<ConnectorIndex>().myIndex = arrayIndex;
                        }
                    }
                }
            }
            else if(pipeConnectors[arrayIndex] != null)
            //if their is no pipe and their was a connection at that point destry the connection
            {
                //destroys the pipe connector
                Destroy(pipeConnectors[arrayIndex]);
                pipeConnectors[arrayIndex] = null;

                //spawns a pipe cap so that items dont travel out of the pipe
                SpawnPipeCap(direction, arrayIndex);

                //removes the connecting pipe from the array
                connectedPipes[arrayIndex] = null;
            }
            else if(pipeCaps[arrayIndex] == null)
            {
                SpawnPipeCap(direction, arrayIndex);
                connectedPipes[arrayIndex] = null;
            }
        }
        

        void SpawnPipeCap(Vector3 direction, int arrayIndex)
        {
            //spawns a pipe cap at the crrect localtion and adds it to the array
            pipeCaps[arrayIndex] =  (GameObject)Instantiate(pipeCap, thisPipesPositon + (direction * 0.2f), Quaternion.Euler(0, 0, 0));
            pipeCaps[arrayIndex].transform.LookAt(transform.position);
            pipeCaps[arrayIndex].transform.SetParent(transform);
        }

        void RemovePipeCap(int arrayIndex)
        {
            //destroys the pipe cap at the given index
            Destroy(pipeCaps[arrayIndex]);
            pipeCaps[arrayIndex] = null;
        }
    }
}