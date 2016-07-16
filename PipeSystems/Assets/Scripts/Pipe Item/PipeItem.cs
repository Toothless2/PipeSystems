using UnityEngine;
using System.Collections;

namespace Pipes
{
    public class PipeItem : MonoBehaviour
    {
        public float itemSpeed;
        public LayerMask ignoredLayer;

        private CharacterController myController;
        private RaycastHit hit;
        private Transform lastTarget;
        private Collider[] parent;
        public bool hasDestination;

        // Use this for initialization
        void Start()
        {
            myController = GetComponent<CharacterController>();
        }
        
        void FixedUpdate()
        {
            CheckForPipeCap();
            //print(lastTarget.tag + " / " + lastTarget.position);
        }

        void CheckForPipeCap()
        {
            //mosit onky used the first time the item is spawned
            if (!hasDestination)
            {
                //checks if the pipe is open
                RaycastForward();

                if (hit.transform != null)
                {
                    ChangeDirection();
                    lastTarget = hit.transform;
                    hasDestination = true;
                }
            }
            else if(lastTarget == null)
            {
                hasDestination = false;
            }
            //if the item is near the destination
            else if(Vector3.Distance(transform.position, lastTarget.root.position) < 0.01)
            {
                //set the items transfrom to the destinations trnsform
                GetComponent<Transform>().position = lastTarget.transform.root.position;
                //tels the item it does not have a destination
                hasDestination = false;
                
                //pipe connectos dont have this script so will gave an error when item goes thorugh a routing pipe
                if(lastTarget.GetComponent<RoutingPipe>() != null)
                {
                    //if the pipe that the item is current in is a routing ppipe go in the dorection thatt it needs to
                    if (lastTarget.GetComponent<RoutingPipe>().isRoutingPipe)
                    {
                        if(lastTarget.GetComponent<RoutingPipe>().WhereDoIGo() != null)
                        {
                            //get the transfom of the connector that the item needs to go through
                            Transform temp = lastTarget.GetComponent<RoutingPipe>().WhereDoIGo();
                            //set that to the target
                            lastTarget = temp;
                            //point at the connector
                            transform.LookAt(lastTarget.position);
                            //tell the object that it has a destinatin (the pipe connector)
                            hasDestination = true;
                        }
                    }
                    else
                    {
                        //if it is not a routing pipe check if the item cam go forward
                        RaycastForward();

                        //if their is a pipe cap infront of the item it cannot go forward
                        //so it changes direction
                        if (hit.transform.tag == "PipeCap")
                        {
                            //sets to false so that it cannot detect the same destination twice in a row
                            lastTarget.gameObject.SetActive(false);
                            ChangeDirection();
                            //sstes to true so other items can go through the pipe
                            lastTarget.gameObject.SetActive(true);
                            //sets the current destintion to the fount pipe
                            lastTarget = hit.transform;
                            //tells the item it has a destination
                            hasDestination = true;
                        }
                        else
                        {
                            //if the pipe was connected to another pipe carry on going forward
                            lastTarget = hit.transform;
                        }
                    }
                }
                else
                {
                    //if the current destination does not have a RoutingPipe script it is a pipe connector so the tiem can go forward
                    RaycastForward();
                    lastTarget = hit.transform;
                }
            }
            else
            {
                MoveForward(itemSpeed, lastTarget.root);
            }
        }

        void RaycastForward()
        {
            Physics.Raycast(transform.position, transform.forward, out hit, 0.5f);
            //Debug.DrawRay(transform.position, transform.forward, Color.green, 5);
        }

        //move the item forward at the given speed
        void MoveForward(float speed, Transform hit)
        {
            speed *= Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, hit.position, speed);
        }

        void ChangeDirection()
        {
            /*
             * changes the items direction what a pipeCap is detected
             * first it rechecks the front of the item then the right, left, up, down, and fianly it will
             * reverse the items direction if their is no other option
             */ 

            Raycast(transform.forward);

            if(hit.transform.tag != "PipeCap")
            {
                //print("forward");
                transform.LookAt(hit.transform.position);
                itemSpeed = hit.transform.root.GetComponent<ConnectPipe>().pipeSpeed;
            }
            else
            {
                Raycast(transform.right);

                if(hit.transform.tag != "PipeCap")
                {
                    //print("right");
                    transform.LookAt(hit.transform.position);
                    itemSpeed = hit.transform.root.GetComponent<ConnectPipe>().pipeSpeed;
                }
                else
                {
                    //(equivelant to Vector3.left)
                    Raycast(-transform.right);

                    if (hit.transform.tag != "PipeCap")
                    {
                        //print("left");
                        transform.LookAt(hit.transform.position);
                        itemSpeed = hit.transform.root.GetComponent<ConnectPipe>().pipeSpeed;
                    }
                    else
                    {
                        Raycast(transform.up);

                        if (hit.transform.tag != "PipeCap")
                        {
                            //print("up");
                            transform.LookAt(hit.transform.position);
                            itemSpeed = hit.transform.root.GetComponent<ConnectPipe>().pipeSpeed;
                        }
                        else
                        {
                            //(equivelan to Vector3.down)
                            Raycast(-transform.up);

                            if (hit.transform.tag != "PipeCap")
                            {
                                //print("down");
                                transform.LookAt(hit.transform.position);
                                itemSpeed = hit.transform.root.GetComponent<ConnectPipe>().pipeSpeed;
                            }
                            else
                            {
                                //(equivelant to Vector3.back)
                                Raycast(-transform.forward);

                                if (hit.transform.tag != "PipeCap")
                                {
                                    //print("back");
                                    transform.LookAt(hit.transform.position);
                                    itemSpeed = hit.transform.root.GetComponent<ConnectPipe>().pipeSpeed;
                                }
                                else
                                {
                                    ///if the item is not in a pipe it should be destroyed
                                    Destroy(gameObject);
                                }
                            }
                        }
                    }
                }
            }
        }

        void ForceMove(float itemSpeed)
        {
            transform.Translate(transform.forward * itemSpeed * Time.deltaTime);
        }

        RaycastHit Raycast(Vector3 direction)
        {
            //Debug.DrawRay(transform.position, direction, Color.red, 5);
            Physics.Raycast(transform.position, direction, out hit, 1, ignoredLayer);
            return hit;
        }
    }
}