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
    public void setScaleSmall(){
        gameObject.transform.localScale = new Vector3(1.0f,0.5f,1.0f);
    }
    public void ScaleBig(){
        gameObject.transform.localScale = new Vector3(1.0f,1.0f,1.0f);
    }
    


}
