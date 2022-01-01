using System;
using UnityEngine;

public static class Helper 
{
    // Start is called before the first frame update
    public static Vector2 GetTopLeftPosition(SpriteRenderer field, SpriteRenderer gameObject){
        return new Vector2(field.transform.position.x - field.bounds.extents.x + gameObject.bounds.extents.x,
                            field.transform.position.y + field.bounds.extents.y - gameObject.bounds.extents.y);
    }
    public static void ForceLandscapeOrientation(){
        Screen.autorotateToLandscapeLeft = true;
        Screen.autorotateToLandscapeRight = true;
        Screen.autorotateToPortrait = false;
        Screen.autorotateToPortraitUpsideDown = false;
        Screen.orientation = ScreenOrientation.AutoRotation;
    }

}
