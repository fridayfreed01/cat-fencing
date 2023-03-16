using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//for setting cursor

public class setCursor : MonoBehaviour
{
  public Texture2D cursorImage; 

  void Start(){
    
    //set the cursor origin to its centre. (default is upper left corner)
     Vector2 cursorOffset = new Vector2(cursorImage.width/2, cursorImage.height/2);
     
      //Sets the cursor to the sprite with given offset 
      Cursor.SetCursor(cursorImage, cursorOffset, CursorMode.Auto);
  }
}