using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class EnemyController : MonoBehaviour
{
    public Animator anim;
   
    // Start is called before the first frame update
    void Start()
    {
        this.anim.SetInteger("State", 0);



    }

    // Update is called once per frame
    void Update()
    {


    }



}