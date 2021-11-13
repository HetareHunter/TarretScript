using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Players
{
    public class HandFixer : MonoBehaviour
    {
        [SerializeField] GameObject grabHandMesh;
        public void FixHand()
        {
            grabHandMesh.SetActive(true);
        }

        public void ReleseHand()
        {
            grabHandMesh.SetActive(false);
        }
    }
}