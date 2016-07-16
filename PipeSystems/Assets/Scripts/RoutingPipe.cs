using UnityEngine;
using System.Collections;

namespace Pipes
{
    public class RoutingPipe : MonoBehaviour
    {
        public bool isRoutingPipe;

        private GameObject[] connectors;
        private GameObject selectedConnector;

        void Start()
        {
            if (isRoutingPipe)
            {
                ForceUpdate();
            }
        }

        public void ForceUpdate()
        {
            connectors = transform.root.GetComponent<ConnectPipe>().pipeConnectors;

            for (int i = 0; i < 6; i++)
            {
                if (connectors[i] != null)
                {
                    SelectConnector(i);
                    return;
                }
            }
        }

        public void SelectConnector(int index)
        {
            if(selectedConnector != null)
            {
                selectedConnector.GetComponent<MeshRenderer>().enabled = true;
            }
            selectedConnector = connectors[index];
            selectedConnector.GetComponent<MeshRenderer>().enabled = false;
        }

        public Transform WhereDoIGo()
        {
            if(selectedConnector != null)
            {
                return selectedConnector.transform;
            }
            else
            {
                return null;
            }
        }
    }
}