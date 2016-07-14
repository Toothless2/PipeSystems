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
            if (!hasDestination)
            {
                //checks if the pipe is open
                RaycastForward();

                if (hit.transform != null)
                {
                    //print(hit.transform.tag);
                    if (hit.transform.tag == "ItemDirector")
                    {
                        print("fournd thing");
                        MoveForward(itemSpeed, hit.transform.root);
                        lastTarget.gameObject.SetActive(true);
                        lastTarget = hit.transform;
                        hasDestination = true;
                    }
                    else
                    {
                        ChangeDirection();
                        lastTarget = hit.transform;
                        hasDestination = true;
                    }
                }
            }
            else if(Vector3.Distance(transform.position, hit.transform.root.position) < 0.01)
            {
                GetComponent<Transform>().position = hit.transform.root.position;
                hasDestination = false;

                RaycastForward();
                
                if(hit.transform.tag == "PipeCap")
                {
                    lastTarget.gameObject.SetActive(false);
                    ChangeDirection();
                    lastTarget.gameObject.SetActive(true);
                    lastTarget = hit.transform;
                }
            }
            else
            {
                MoveForward(itemSpeed, hit.transform.root);
            }
        }

        void RaycastForward()
        {
            Physics.Raycast(transform.position, transform.forward, out hit, 0.5f);
            //Debug.DrawRay(transform.position, transform.forward, Color.green, 5);
        }

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
                                    print("No Direction");
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
            Debug.DrawRay(transform.position, direction, Color.red, 5);
            Physics.Raycast(transform.position, direction, out hit, 1, ignoredLayer);
            return hit;
        }
    }
}