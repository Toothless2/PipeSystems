using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Pipes
{
    public class PipeConnections : MonoBehaviour
    {
        public List<PipeConnectionDirections> connectionDirections;
        
        void Update()
        {
            /*
            for(int i = 0; i < 6; i++)
            {
                if(GetComponent<ConnectPipe>().pipeConnectors[i] != null)
                {
                    connectionDirections[i].hasConnection = true;
                }
                else
                {
                    connectionDirections[i].hasConnection = false;
                }
            }
            */
        }
    }
}