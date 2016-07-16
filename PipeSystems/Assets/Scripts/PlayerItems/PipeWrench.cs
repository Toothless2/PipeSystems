using UnityEngine;
using System.Collections;

namespace Pipes
{
    public class PipeWrench : MonoBehaviour
    {
        private RaycastHit hit;
        public GameObject[] removedPipesConnections;

        void Update()
        {
            //get a right click from a player
            if (Input.GetButtonDown("Fire2"))
            {
                Raycast();

                //checks if the player has clicked on something
                if(hit.transform != null)
                {
                    //checks if the player has clicked on a connector
                    if (hit.transform.GetComponent<ConnectorIndex>() != null)
                    {
                        //checks if the pipe is a routing pipe
                        if (hit.transform.root.GetComponentInChildren<RoutingPipe>().isRoutingPipe)
                        {
                            //tells the routing pipe to chenge the selected connector to the one tht has been clicked by the player
                            hit.transform.root.GetComponentInChildren<RoutingPipe>().SelectConnector(hit.transform.GetComponent<ConnectorIndex>().myIndex);
                        }
                    }
                }
            }

            if(Input.GetButton("Fire3") && Input.GetButtonDown("Fire2"))
            {
                Raycast();

                if(hit.transform != null)
                {
                    if(hit.transform.GetComponent<ConnectPipe>() != null)
                    {
                        removedPipesConnections = hit.transform.GetComponent<ConnectPipe>().connectedPipes;
                        hit.transform.GetComponent<ConnectPipe>().DestroyPipe();
                        StartCoroutine(WaitForTime(removedPipesConnections));
                    }
                    else if(hit.transform.root.GetComponent<ConnectPipe>() != null)
                    {
                        removedPipesConnections = hit.transform.root.GetComponent<ConnectPipe>().connectedPipes;
                        hit.transform.root.GetComponent<ConnectPipe>().DestroyPipe();
                        StartCoroutine(WaitForTime(removedPipesConnections));
                    }
                }
            }
        }

        void Raycast()
        {
            //Debug.DrawRay(transform.position, transform.forward, Color.green, 5f);
            Physics.Raycast(transform.position, transform.forward, out hit, 2f);
        }

        IEnumerator WaitForTime(GameObject[] thing)
        {
            yield return new WaitForSeconds(0.01f);
            UpdateSurroundingPipes(thing);
        }

        void UpdateSurroundingPipes(GameObject[] pipes)
        {
            //goes through each element in the array
            foreach(GameObject pipe in pipes)
            {
                //if the element is not null
                if(pipe != null)
                {
                    //update its connections
                    pipe.GetComponent<ConnectPipe>().UpdateConnectios();
                }
            }
        }
    }
}