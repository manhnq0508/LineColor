using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardManage : MonoBehaviour
{
    public static BoardManage Instance;
    
    public GameObject ballSpace;
    public GameObject gameField;
    public GameObject ballExample;
    Vector3 leftTopAngle;

    float xPadding;
    float yPadding;
    public BallBehaviour[,] field = new BallBehaviour[9,9];
    void Awake(){
        Instance = this;
        var gameFieldSpriteRenderer = gameField.GetComponent<SpriteRenderer>();
        var ballSpriteRenderer = ballExample.GetComponent<SpriteRenderer>();
        leftTopAngle = Helper.GetTopLeftPosition(gameFieldSpriteRenderer,ballSpriteRenderer);
        xPadding = gameFieldSpriteRenderer.bounds.size.x / 9;
        yPadding = -gameFieldSpriteRenderer.bounds.size.y / 9;
        PrepareField();
    }
    public void PrepareField(){
        for(int i = 0; i < 9; i++){
            for (int j = 0; j < 9; j++){
                Instantiate(ballSpace,GetPos(i,j),Quaternion.identity);
            }
        }
    }
    public Vector3 GetPos(int x, int y){
        return new Vector3(leftTopAngle.x + x*xPadding, leftTopAngle.y + y*yPadding);
    }
    public Point ScreenCoordToBoardCoords(Vector3 position){
        var xCoord = (int)Mathf.Round((position.x - leftTopAngle.x)/xPadding);
        var yCoord = (int)Mathf.Round((position.y - leftTopAngle.y)/yPadding);
        return new Point(xCoord, yCoord);
    }

}
