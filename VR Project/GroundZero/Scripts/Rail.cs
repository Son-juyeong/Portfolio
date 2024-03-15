using System.Collections;
using System.Collections.Generic;
using UnityEngine;


    public class Rail : MonoBehaviour
    {
        public enum eType
        {
            Left = 0, Middle = 1, Right = 2
        }
        private int value;
        public int Value { get { return value; } }

        public eType type;
        [SerializeField] Transform[] arrRoute;

        void Start ()
        {
            switch (type)
            {
                case eType.Left:
                    value = 4;
                    break;
                case eType.Middle:
                    value = 0;
                    break;
                case eType.Right:
                    value = -4;
                    break;
            }
        }

        public Transform[] GetRoute()
        {
            return this.arrRoute;
        }
    }

