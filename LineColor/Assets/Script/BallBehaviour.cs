using System;
using UnityEngine;

public class BallBehaviour : MonoBehaviour
{
    // Start is called before the first frame update
    public int BallCode;
    PathMove pathMover;
    void Start(){
        pathMover = new PathMove(gameObject,8f);
    }
    public void MoveByPath(Vector3[] newPositions){
        pathMover.MoveByPath(newPositions);
    }
    public void Die(){
        Destroy(gameObject);
    }
    void Update()
    {
        pathMover.Update();
    }
}
