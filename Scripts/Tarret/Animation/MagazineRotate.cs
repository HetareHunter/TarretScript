using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagazineRotate : MonoBehaviour
{
    [SerializeField] GameObject baseTarret;
    Animator magazineAnim;
    
    // Start is called before the first frame update
    void Start()
    {
        magazineAnim = GetComponent<Animator>();
    }

    public void RotateMagazine()
    {
        magazineAnim.SetTrigger("RotateMagezine");
    }
}
