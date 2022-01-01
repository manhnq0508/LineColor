using System;
using UnityEngine;

public class BallBehaviour : MonoBehaviour
{
    // Start is called before the first frame update
    public int BallCode;
    Animator animator;
    public void Die(){
        Destroy(gameObject);
    }
}
