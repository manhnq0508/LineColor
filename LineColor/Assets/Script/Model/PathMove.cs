using System;
using System.Collections.Generic;
using UnityEngine;

public class PathMove 
{
    // Start is called before the first frame update
    Queue<Vector3> path = new Queue<Vector3>();
    public float Speed;
    private GameObject GameObject;
    public bool Moving;
    public PathMove(GameObject gameObject, float speed){
        GameObject = gameObject;
        Speed = speed;
    }
    public void MoveByPath(Vector3[] newPath)
    {
        foreach (var step in newPath){
            path.Enqueue(step);
        }
    }
    public void Update(){
        if(path.Count == 0){
            Moving = false;
            return;
        }
        Moving = true;
        var newPosition = path.Peek();
        GameObject.transform.position = Vector3.MoveTowards(GameObject.transform.position,
                    newPosition,
                    Speed * Time.deltaTime);
        if(GameObject.transform.position == newPosition){
            path.Dequeue();
        }
        if(path.Count == 0){
            Moving = false;
        }
    }
    // Update is called once per frame
    public bool IsMoving(){
        return Moving;
    }
}
