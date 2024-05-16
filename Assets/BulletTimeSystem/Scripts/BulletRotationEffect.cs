using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TheBox
{
    public class BulletRotationEffect : MonoBehaviour
    {
        public long speed;
        [SerializeField] Vector3 axis = new (0, 0, 1);

        private void Start()
        {

        }
        void Update()
        {
            transform.Rotate(speed * Time.deltaTime * axis); 
        }
    }
}
