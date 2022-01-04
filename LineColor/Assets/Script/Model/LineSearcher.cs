using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LineSearcher
{
    // Start is called before the first frame update
    private const int MinLineLength = 5;
    private const int boardSize = 9;
    public class Result{
        public List<Point> Points {get; private set;}
        public int Score {get; private set;}
        public Result(List<Point> points, int score){
            Points = points;
            Score = score;
        }
    }
    public static Result SearchBy(BallBehaviour[,] field){
        var PointForDelete = new List<Point>();

        var ScoreX = SearchByX(field,PointForDelete);
        var ScoreY = SearchByY(field,PointForDelete);
        var ScoreDiagonal = SearchByDiagonals(field,PointForDelete);

        var fullScore = GetScore(ScoreX + ScoreY + ScoreDiagonal);
         
        return new Result(PointForDelete,fullScore);
    }
    // Search By X
    private static int SearchByX(BallBehaviour[,] field,List<Point> PointForDelete){
        var score = 0;
        for (int j = 0; j < boardSize; j++){
            var LengthOfColor = 1;
            var lastColorValue = -1;
            for (int i = 0; i < boardSize;i++){
                if(i == 0){
                    lastColorValue = GetColorValue(field,i,j);
                    continue;
                }
                var currentBallColor = GetColorValue(field,i,j);
                if(currentBallColor == lastColorValue){
                    LengthOfColor++;
                }
                else{
                    score += SetDeleteBallByX(PointForDelete,i,j,LengthOfColor,lastColorValue);
                    LengthOfColor = 1;
                }
                lastColorValue = currentBallColor;
            }
            score += SetDeleteBallByX(PointForDelete,boardSize,j,LengthOfColor,lastColorValue);
        }
        return score;
    }
    private static int SetDeleteBallByX(
        List<Point> PointForDelete, 
        int x, int y, 
        int LengthOfColor, 
        int lastColorValue
    ){
        if(lastColorValue >= 0 && LengthOfColor >= MinLineLength){
            for (int i = x - LengthOfColor; i < x;i++){
                PointForDelete.Add(new Point(i,y));
            }
            return LengthOfColor;
        }
        return 0;
    }

    // Search by Y
    private static int SearchByY(BallBehaviour[,] field,List<Point> PointForDelete){
        var score = 0;
        for (int i = 0; i < boardSize; i++){
            var LengthOfColor = 1;
            var lastColorValue = -1;
            for (int j = 0; j < boardSize;j++){
                if(j == 0){
                    lastColorValue = GetColorValue(field,i,j);
                    continue;
                }
                var currentBallColor = GetColorValue(field,i,j);
                if(currentBallColor == lastColorValue){
                    LengthOfColor++;
                }
                else{
                    score += SetDeleteBallByY(PointForDelete,i,j,LengthOfColor,lastColorValue);
                    LengthOfColor = 1;
                }
                lastColorValue = currentBallColor;
            }
            score += SetDeleteBallByY(PointForDelete,i,boardSize,LengthOfColor,lastColorValue);
        }
        return score;
    }
    private static int SetDeleteBallByY(
        List<Point> PointForDelete, 
        int x, int y, 
        int LengthOfColor, 
        int lastColorValue
    ){
        if(lastColorValue >= 0 && LengthOfColor >= MinLineLength){
            for (int i = y - LengthOfColor; i < y;i++){
                PointForDelete.Add(new Point(x,i));
            }
            return LengthOfColor;
        }
        return 0;
    }
    // Search for diagonal
    private static int SearchByDiagonals(BallBehaviour[,] field,List<Point> PointForDelete){
        var score = 0;
        for ( int i = 0; i < boardSize; i++){
            for (int j = 0; j < boardSize;j++){
                score += SearchByDiagonal(PointForDelete,field,i,j,+1,+1);
                score += SearchByDiagonal(PointForDelete,field,i,j,-1,-1);
                score += SearchByDiagonal(PointForDelete,field,i,j,+1,-1);
                score += SearchByDiagonal(PointForDelete,field,i,j,-1,+1);
            }
        }
        return score;
    }
    private static int SearchByDiagonal(
        List<Point> PointForDelete,
        BallBehaviour[,] field,
        int x, int y,
        int vectorX,
        int vectorY
    ){
        var LengthOfColor = 1;
        var lastColorValue = GetColorValue(field,x,y);
        var score = 0;
        x += vectorX;
        y += vectorY;
        while(x < boardSize && x >= 0 && y < boardSize && y >= 0){
            var currentBallColor = GetColorValue(field,x,y);
            if(currentBallColor == lastColorValue){
                LengthOfColor++;
            }
            else{
                score += SetDeleteByDiagonal(
                    PointForDelete,
                    x,y,
                    LengthOfColor,
                    lastColorValue,
                    vectorX,
                    vectorY
                );
                LengthOfColor = 1;
            }
            lastColorValue = currentBallColor;
            x += vectorX;
            y += vectorY;
        }
        score += SetDeleteByDiagonal(
            PointForDelete,
            x,y,
            LengthOfColor,
            lastColorValue,
            vectorX,
            vectorY
        );
        return score;
    }
    private static int SetDeleteByDiagonal(
        List<Point>PointForDelete,
        int x,int y,
        int LengthOfColor,
        int lastColorValue,
        int vectorX,
        int vectorY
    ){
        if(lastColorValue >= 0 && LengthOfColor >= MinLineLength){
            var realLength = LengthOfColor;
            for ( var counter = 0; counter < LengthOfColor;counter++){
                x -= vectorX;
                y -= vectorY;
                if(PointForDelete.Any(item => item.X == x && item.Y == y)){
                    realLength--;
                }
                else{
                    PointForDelete.Add(new Point(x,y));
                }
            }
            if(realLength > 0){
                return realLength;
            }
        }
        return 0;
    }
    private static int GetColorValue(BallBehaviour[,] field,int i, int j){
        return field[i,j] == null ? -1: field[i,j].BallCode;
    }
    private static int GetScore(int length){
        if (length < MinLineLength){
            return 0;
        }
        var a = length/MinLineLength;
        return 50*a + 20*(length - MinLineLength);
    }
}
