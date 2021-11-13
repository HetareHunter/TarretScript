using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using Tarret;

namespace Players
{
    public class HandleInput : MonoBehaviour
    {
        [Inject]
        ITarretStateChangeable tarret;
        [SerializeField] GameObject tarretCartObj;
        TarretCartMover tarretCart;
        
        private void Start()
        {
            if (tarretCartObj != null)
            {
                tarretCart = tarretCartObj.GetComponent<TarretCartMover>();
            }
        }

        public void Attack()
        {
            tarret.ToAttack();
        }
    }
}