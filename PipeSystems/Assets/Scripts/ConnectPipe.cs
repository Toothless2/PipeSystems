using UnityEngine;
using System.Collections;

namespace Pipes
{
    public class ConnectPipe : MonoBehaviour
    {
        public GameObject pipeNub;
        private RaycastHit hit;
        private Vector3 thisPipesPositon;
        private GameObject spawnedNub;

        // Use this for initialization
        void Start()
        {
            thisPipesPositon = transform.position;
            CheckForPipes();
        }

        // Update is called once per frame
        void Update()
        {

        }

        void CheckForPipes()
        {
            if(Physics.Raycast(transform.position, transform.forward, out hit, 1))
            {
                if(hit.transform.tag == "Pipe")
                {
                    spawnedNub = (GameObject)Instantiate(pipeNub, thisPipesPositon + (Vector3.forward * 0.35f), Quaternion.Euler(0, 0, 0));
                    spawnedNub.transform.SetParent(transform);
                }
            }

            if (Physics.Raycast(transform.position, (transform.forward * -1), out hit, 1))
            {
                if (hit.transform.tag == "Pipe")
                {
                    spawnedNub = (GameObject)Instantiate(pipeNub, thisPipesPositon + -(Vector3.forward * 0.35f), Quaternion.Euler(0, 0, 0));
                    spawnedNub.transform.SetParent(transform);
                }
            }
            
            if (Physics.Raycast(transform.position, transform.up, out hit, 1))
            {
                if (hit.transform.tag == "Pipe")
                {
                    spawnedNub = (GameObject)Instantiate(pipeNub, thisPipesPositon + (Vector3.up * 0.35f), Quaternion.Euler(90, 0, 0));
                    spawnedNub.transform.SetParent(transform);
                }
            }

            if (Physics.Raycast(transform.position, (transform.up * -1), out hit, 1))
            {

                if (hit.transform.tag == "Pipe")
                {
                    spawnedNub = (GameObject)Instantiate(pipeNub, thisPipesPositon + -(Vector3.up * 0.35f), Quaternion.Euler(90, 0, 0));
                    spawnedNub.transform.SetParent(transform);
                }
            }

            if (Physics.Raycast(transform.position, transform.right, out hit, 1))
            {
                if (hit.transform.tag == "Pipe")
                {
                    spawnedNub = (GameObject)Instantiate(pipeNub, thisPipesPositon + (Vector3.right * 0.35f), Quaternion.Euler(0, 90, 0));
                    spawnedNub.transform.SetParent(transform);
                }
            }

            if (Physics.Raycast(transform.position, (transform.right * -1), out hit, 1))
            {

                if (hit.transform.tag == "Pipe")
                {
                    spawnedNub = (GameObject)Instantiate(pipeNub, thisPipesPositon + (Vector3.left * 0.35f), Quaternion.Euler(0, 90, 0));
                    spawnedNub.transform.SetParent(transform);
                }
            }
        }
    }
}