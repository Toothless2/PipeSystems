using UnityEngine;
using System.Collections;

namespace Pipes
{
    public class PipeItem : MonoBehaviour
    {
        public LayerMask checkedLayers;

        private RaycastHit hit;
        private bool hasTarget;
        private bool isInPipe = true;
        private float itemSpeed;

        private GameObject target;

        void FixedUpdate()
        {
            //cheks if the item is in a pipe
            if (isInPipe)
            {
                //if it is move the item in the pipe
                MoveItemItemInPipe();
            }
        }

        void MoveItemItemInPipe()
        {
            //if the item does not hvae a target give it one
            if (!hasTarget)
            {
                //gets a target
                ChangeDirection();
                //looks at th target
                transform.LookAt(target.transform.position);
            }
            //if the object had a target and it has been destryoed it is not in a pipe and it should fall to the ground
            else if(target == null)
            {
                isInPipe = false;
                gameObject.AddComponent<Rigidbody>();
                GetComponent<Rigidbody>().useGravity = true;
                GetComponent<Rigidbody>().isKinematic = false;
            }
            //checks if the itemm is close to the target
            else if (Vector3.Distance(transform.position, target.transform.position) < 0.1f)
            {
                //if it is close set the position to the same as the targets
                GetComponent<Transform>().position = target.transform.position;

                //checks if the target as a RoutingPipe component
                if(target.GetComponent<RoutingPipe>() != null)
                {
                    //checks it the target is a routing pipe
                    if (target.GetComponent<RoutingPipe>().isRoutingPipe)
                    {
                        //looks at the selected connector
                        transform.LookAt(target.GetComponent<RoutingPipe>().selectedConnector.transform.position);
                        //moves in that direction
                        ChangeDirection();
                    }
                    else
                    {
                        //if the pipe is not a routing pipe is shoud checks for a new target
                        ChangeDirection();
                    }
                }
                //everything that it detects should have one of those scripts but if it does not it will check for a new direction
                else
                {
                    ChangeDirection();
                }
            }
            else
            {
                //move toward the target
                MoveForward();
            }
        }

        void ChangeDirection()
        {
            //tells the item it does not currently have a target
            hasTarget = false;

            /*
             *checks the forward, right, left, down, up, and back directions relative to the object for a pipe and if one is not found the object is destroyed 
             */

            CheckDirection(Vector3.forward);

            if (!hasTarget)
            {
                CheckDirection(Vector3.right);

                if (!hasTarget)
                {
                    CheckDirection(Vector3.left);

                    if (!hasTarget)
                    {
                        CheckDirection(Vector3.down);

                        if(!hasTarget)
                        {
                            CheckDirection(Vector3.up);

                            if (!hasTarget)
                            {
                                //checks the reverse direction last
                                CheckDirection(Vector3.back);

                                if (!hasTarget)
                                {
                                    //if no directions yeild a result destroy the object
                                    Destroy(gameObject);
                                }
                            }
                        }
                    }
                }
            }
        }

        void CheckDirection(Vector3 direction)
        {
            //Debug.DrawRay(transform.position, direction, Color.green, 5);
            Physics.Raycast(transform.position, transform.TransformDirection(direction), out hit, 1, checkedLayers);

            //chesk that it hit something
            if(hit.transform != null)
            {
                //checks that it did hit a pipe
                if (hit.transform.tag == "ItemDirector")
                {
                    //sets the target to the detected object
                    target = hit.transform.gameObject;
                    //looks at the target
                    transform.LookAt(target.transform.position);
                    //gets the speed that the item should be traveling at
                    itemSpeed = target.transform.root.GetComponent<ConnectPipe>().pipeSpeed;
                    //tells the item it has a target
                    hasTarget = true;
                }
            }
        }

        void MoveForward()
        {
            transform.Translate(target.transform.forward * itemSpeed * Time.deltaTime);
        }
    }
}