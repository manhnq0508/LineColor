using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject[] balls;
    public BallBehaviour[] ballBehaviours;
    readonly int[] NextBalls = new int[3];
    void Start(){
        ReStartGame();
    }
    void ReStartGame(){
        for(int i = 0; i < 0; i++){
            for (int j = 0 ; j < 9;j++){
                if(BoardManage.Instance.field[i,j] != null){
                    BoardManage.Instance.field[i,j].Die();
                }
                BoardManage.Instance.field[i,j] = null;
            }
        }
        StartCoroutine(RestartGameComplete());
    }
    IEnumerator RestartGameComplete(){
        yield return new WaitForSeconds(0.2f);
        for( int i = 0; i < 5; i++){
            CreateBallInRandomPlace();
        }
        InitNextBalls();
    }
    private void InitNextBalls(){
        for (int i = 0; i < 3; i++){
            NextBalls[i] = ballBehaviours[Random.Range(0,ballBehaviours.Length)].BallCode - 1;
        }
    }
    private void CreateBallInRandomPlace(int nextBalls = -1){
        var x = Random.Range(0, 8);
        var y = Random.Range(0, 8);
        int counter = 0;
        while (BoardManage.Instance.field[x, y] != null)
        {
            counter++;
            x = Random.Range(0,8);
            y = Random.Range(0,8);
            if(counter == 5){
                break;
            }
        }
        if (BoardManage.Instance.field[x,y] == null){
            BoardManage.Instance.field[x,y] = CreateBall(x,y, nextBalls);
            return;
        }
        for ( int i = 0 ; i < 9; i++){
            for (int j = 0; j < 9;j++){
                if(BoardManage.Instance.field[i,j] == null){
                    BoardManage.Instance.field[i,j] = CreateBall(i,j,nextBalls);
                    return;
                }
            }
        }
    }
    private BallBehaviour CreateBall(int x, int y, int ballValue = -1){
        var pos = BoardManage.Instance.GetPos(x,y);
        if(ballValue == -1){
            ballValue = Random.Range(0, balls.Length);
        }
        var ball = Instantiate(balls[ballValue], pos, Quaternion.identity);
        var component = ball.GetComponent<BallBehaviour>();
        return component;
    }

}
