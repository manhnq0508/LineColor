using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LeePathSearchAlgorithm;
using System.Linq;
using UnityEngine.UI;
public class GameManager : MonoBehaviour
{
    // Start is called before the first frame update
    public int counterBig = 0;
    public GameObject[] balls;
    private List<GameObject> predictBall = new List<GameObject>();
    public GameObject[] smallballs;
    public BallBehaviour[] ballBehaviours;
    BallBehaviour currentBall;
    Point currentBallPoint;
    public GameObject[] BallPrediction;
    readonly SpriteRenderer[] BallNextSpriteRenderer = new SpriteRenderer[3];
    public Sprite[] PredictionBallSprite;

    PathSearchLee pathSearch; 

    readonly int[] NextBalls = new int[3];
    Point[] ballPLace = new Point[3];
    int [,] MapFinding = new int[9,9];
    int [,] MapPredict = new int[9,9];
    public Text HighScoreText;
    public Text ScoreText;
    public GameObject playButton;
    public GameObject gameOver;
    private int HighScore;

    int score{ get; set;}
    int Score{
        get{
            return score;
        }
        set{
            score = value;
            ScoreText.text = score.ToString();
            UpdatehighScore();
        }
    }
    public void UpdatehighScore(){
        if(Score > HighScore){
            HighScore = Score;
        }
        HighScoreText.text = HighScore.ToString();
    }
    void Start(){
        gameOver.SetActive(false);
        ReStartGame();
    }
    public void GameOver(){
        gameOver.SetActive(true);
    }
    void Awake(){
        pathSearch = new PathSearchLee(PathSearchLee.SearchMethod.Path4);
        for(int i = 0; i < 3; i++){
            BallNextSpriteRenderer[i] = BallPrediction[i].GetComponent<SpriteRenderer>();
        }
        ballBehaviours = new BallBehaviour[balls.Length];
        for (int i = 0; i < balls.Length; i++){
            ballBehaviours[i] = balls[i].GetComponent<BallBehaviour>();
        }
        

    }
    public void PlayButtonOnClick(){
        ReStartGame();
    }
    void ReStartGame(){
        Score = 0;
        for(int i = 0; i < 9; i++){
            for (int j = 0 ; j < 9;j++){
                if(BoardManage.Instance.field[i,j] != null){
                    BoardManage.Instance.field[i,j].Die();
                }
                BoardManage.Instance.field[i,j] = null;
            }
        }
        foreach (var ball in predictBall){
            Destroy(ball);
        }      
        RestartGameComplete();
    }
    void RestartGameComplete(){
        for( int i = 0; i < 6; i++){
            Create6BallInRandomPlace();
        }
        UpdateNextBalls();
    }
    private void Create6BallInRandomPlace(int nextBalls = -1){
        var x = Random.Range(0, 9);
        var y = Random.Range(0, 9);
        int counter = 0;
        while (BoardManage.Instance.field[x, y] != null)
        {
            counter++;
            x = Random.Range(0,9);
            y = Random.Range(0,9);
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
    private void InitNextBalls(){
        for (int i = 0; i < 3; i++){
            NextBalls[i] = ballBehaviours[Random.Range(0,ballBehaviours.Length)].BallCode ;
        } 
    }

    private void UpdateNextBalls(){

        InitNextBalls();
        for (int i = 0; i < 3; i++){
            BallNextSpriteRenderer[i].sprite = PredictionBallSprite[NextBalls[i]];
        }
        Create3BallPlace();
    }

    private void Mapstate(){
        for (int i = 0 ; i < 9; i++){
            for (int j = 0; j < 9;j++){
                if(BoardManage.Instance.field[i,j] != null){
                    MapPredict[i,j] = 1;
                }
                else{
                    MapPredict[i,j] = 0;
                }
            }
        }
    }
    private void Create3BallPlace(){

        Create3PredictRandomPlace();
        for (int i = 0; i < 3; i++){
            CreateBallPredict(ballPLace[i].X,ballPLace[i].Y,NextBalls[i]);
        }
    }
    private void Create3PredictRandomPlace(){
        Mapstate();
        for (int i = 0; i < 3; i++){
            CreateRandomPredictPlace(i);
        }
    }
    private void CreateRandomPredictPlace(int ballCount){
        var x = Random.Range(0, 9);
        var y = Random.Range(0, 9);
        int counter = 0;
        while (MapPredict[x, y] != 0)
        {
            counter++;
            x = Random.Range(0,9);
            y = Random.Range(0,9);
            if(counter == 5){
                break;
            }
        }
        if (MapPredict[x,y] == 0){
            ballPLace[ballCount] = new Point(x,y);
            MapPredict[x,y] = 1;
            return;
        }
        for ( int i = 0 ; i < 9; i++){
            for (int j = 0; j < 9;j++){
                if(MapPredict[i,j] == 0){
                    ballPLace[ballCount] = new Point(i,j);
                    MapPredict[i,j] = 1;
                    return;
                }
            }
        }

    }


    private void CreateBallPredict(int x, int y, int ballValue = -1){
        var pos = BoardManage.Instance.GetPos(x,y);
        if(ballValue == -1){
            ballValue = Random.Range(0, balls.Length);
        }
        var ball = Instantiate(smallballs[ballValue], pos, Quaternion.identity);
        predictBall.Add(ball);
    }


    // update 3 ball


    void OnMouseDown(){
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 mousePos2D = new Vector2(mousePos.x,mousePos.y);

        var oldBall = currentBall;
        var oldBallPoint = currentBallPoint;
        RaycastHit2D cast = Physics2D.Raycast(mousePos2D, Vector2.zero);
        if(cast.collider != null){
            var position = cast.collider.gameObject.transform.position;
            currentBallPoint = BoardManage.Instance.ScreenCoordToBoardCoords(position);

            currentBall = BoardManage.Instance.field[currentBallPoint.X,currentBallPoint.Y];
            if(currentBall != null){
                // appear 
                currentBall.setScaleSmall();

                Debug.Log(currentBall.BallCode);
            }
            if(currentBall != null && oldBall != null && oldBall != currentBall){

                oldBall.ScaleBig();
                currentBall.setScaleSmall();
                Debug.Log("Click 2");
            }
            if (currentBall == null && oldBall != null){
                oldBall.ScaleBig();
                var successMoveToNewPosition = MoveToNewPosition(oldBall,oldBallPoint,currentBallPoint);
                if(successMoveToNewPosition){
                    //UpdateNextBalls();
                    Debug.Log("Click 3");
                }
                else{
                    currentBall = oldBall;
                    currentBallPoint = oldBallPoint;
                }
                           
                
                
            }
        }
        else{
            Debug.Log("ASNJADN");
        }
    }
    private bool MoveToNewPosition(BallBehaviour oldBall, Point Start, Point End){
        for(int i = 0; i < 9; i++){
            for (int j = 0; j < 9;j++){
                MapFinding[i,j] = BoardManage.Instance.field[i,j] == null ? 0 : 1;
            }
        }
        MapFinding[Start.X,Start.Y] = 0;
        var resultPath = pathSearch.Search(MapFinding, Start, End);
        if(resultPath.Any()){
            oldBall.MoveByPath(resultPath.Select(pathStep => BoardManage.Instance.GetPos(pathStep.X, pathStep.Y)).ToArray());
            StartCoroutine(AfterMovedToNewPosition(oldBall, Start, End));
            return true;
        }
        else {
            return false;
        }
    }
    private IEnumerator AfterMovedToNewPosition(BallBehaviour oldBall, Point start, Point end)
    {

        BoardManage.Instance.field[end.X, end.Y] = BoardManage.Instance.field[start.X, start.Y];
        BoardManage.Instance.field[start.X, start.Y] = null;
        var deletedCount = AddScoreAndRemoveBalls();
        if(deletedCount == 0){
            BallUpdate();
            UpdateNextBalls();
        }
        else{
            yield return new WaitForSeconds(0.05f);
        }
        // BallUpdate();
        // UpdateNextBalls();
        CheckGameOver();
        // yield return new WaitForSeconds(0.05f);
    }
    private void BallUpdate(){
        int i = 0;
        foreach (var ball in predictBall){
            AddNewBall(ballPLace[i],NextBalls[i]);
            i++;
            Destroy(ball);
        }
        predictBall = new List<GameObject>();
    }
    private void AddNewBall(Point smallPoint , int smallBallId){
        if(BoardManage.Instance.field[smallPoint.X,smallPoint.Y] == null){     
            BoardManage.Instance.field[smallPoint.X,smallPoint.Y] = CreateBall(smallPoint.X, smallPoint.Y, smallBallId);
        }
        else{
            for(int i = 0; i < 9; i++){
                for(int j = 0; j < 9;j++){
                    if(BoardManage.Instance.field[i,j] == null){
                        BoardManage.Instance.field[i,j] = CreateBall(i, j, smallBallId);
                        return;
                    }
                }
            }
        }
        
    }
    private void CheckGameOver(){
        bool haveEmptySpace = false;
        for (int i = 0; i < 9; i++){
            for (int j = 0; j < 9; j++){
                if(BoardManage.Instance.field[i,j] == null){
                    haveEmptySpace = true;
                }
            }
        }
        if(!haveEmptySpace){
            GameOver();
        }
    }
    private int AddScoreAndRemoveBalls(){
        var result = LineSearcher.SearchBy(BoardManage.Instance.field);
        foreach (var point in result.Points){
            var ball = BoardManage.Instance.field[point.X,point.Y];
            if(ball != null){
                ball.Die();
            }
            BoardManage.Instance.field[point.X,point.Y] = null;
        }
        Score += result.Score;
        return result.Points.Count;
    }
}
