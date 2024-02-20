using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace TestCameraMove
{
    public class MainCamera : MonoBehaviour
    {
        [SerializeField]
        private Transform player;
        // Start is called before the first frame update
        void Start()
        {
        }

        // Update is called once per frame
        void Update()
        {
            if (this.player.position.x >= this.transform.position.x)
            {
                this.transform.position = new Vector3(this.player.position.x, this.transform.position.y, this.transform.position.z);
            }
            this.transform.position = new Vector3(Mathf.Clamp(this.transform.position.x, 0, 1000), this.transform.position.y, this.transform.position.z);
        }
    }
}
