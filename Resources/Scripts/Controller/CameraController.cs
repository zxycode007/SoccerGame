using UnityEngine;
using System.Collections;

namespace Game
{
    public class CameraController: MonoBehaviour
    {
        public Transform targeTransform;
        public  Transform  mainCamTransform;
        public float z, y;
        // Use this for initialization
        void Start()
        {
            mainCamTransform = this.transform;
            mainCamTransform.LookAt(targeTransform);
        }

        // Update is called once per frame
        void LateUpdate()
        {
            GetCameraPos();
            mainCamTransform.LookAt(targeTransform);
        }

        private void GetCameraPos()
        {
            if(targeTransform != null)
            {
                Vector3 newTagetVector3 = new Vector3(targeTransform.position.x, targeTransform.position.y + y,
                   targeTransform.position.z + z);
                mainCamTransform.position = Vector3.Lerp(mainCamTransform.position, newTagetVector3, Time.deltaTime * 5);
            }
           
        }
    }
}

