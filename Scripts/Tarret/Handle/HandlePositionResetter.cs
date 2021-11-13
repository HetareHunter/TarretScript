using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Players
{
    public class HandlePositionResetter : MonoBehaviour
    {
        [SerializeField] Transform returningPosition;
        Vector3 startRotation;

        // Start is called before the first frame update
        void Start()
        {
            startRotation = transform.localEulerAngles;
        }

        public void Released()
        {
            transform.localPosition = returningPosition.position;
            transform.localEulerAngles = startRotation;
        }
    }
}