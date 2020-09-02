using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetCursor : MonoBehaviour
{
    public Texture2D mouse;
    public Texture2D hand;
	public CursorMode cursorMode = CursorMode.Auto;
	public Vector2 hotSpot = Vector2.zero;
    
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

	public void setMouse(){
		//SetCuror (mouse, hotSpot, cursorMode);
		Cursor.SetCursor (mouse, hotSpot, cursorMode);
	}
	public void setHand(){
		Cursor.SetCursor (hand, hotSpot, cursorMode);
	}
}
