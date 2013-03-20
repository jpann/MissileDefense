using UnityEngine;
using System.Collections;

public class TextInputController : MonoBehaviour 
{
	public int maxCharacters = 15;
	public Vector3 focusScale = new Vector3(1.1f, 1.1f, 1.1f);
	
	private bool clicked = false;
	private bool focused = false;
	private tk2dTextMesh text;
	
	void Start() 
	{
		//Debug.Log("TextInputController:Start()");
		
		text = gameObject.GetComponent<tk2dTextMesh>();
	}
	
	void Update() 
	{
		//Debug.Log("TextInputController:Update() - clicked = " + clicked + "; focused = " + focused);
		
		if (clicked && focused)
		{		
//			if (Input.anyKeyDown)
//			{
				//Debug.Log("TextInputController:Update() - Input.GetKeyDown(KeyCode.Backspace) = " + Input.GetKeyDown(KeyCode.Backspace));
			
				if (Input.GetKeyDown(KeyCode.Backspace))
				{
					if (!string.IsNullOrEmpty(text.text))
					{
						if (text.text.Length == 1)
						{
							text.text = "";
						}
						else
						{
							text.text = text.text.Substring(0, text.text.Length - 1);
						}
					
						//Debug.Log("TextInputController:Update() - text.text = " + text.text);
						text.Commit();
					}
				}
				
				if ((!Input.GetKeyUp(KeyCode.Backspace)) && (!Input.GetKeyDown(KeyCode.Backspace)))
				{
					if (text.text.Length < maxCharacters)
					{
						foreach (char c in Input.inputString)
						{
							//if (text.text.Length > maxCharacters)
							//	break;
						
							if (char.IsControl(c))
								break;
						
							text.text += c;	
							text.Commit();
						}
					}
				}
			//}
		}
		
		if (Input.GetMouseButtonDown(0))
		{
			if (!focused)
			{
				clicked = false;	
			}
		}
	}
	
	void OnMouseDown()
	{
		//Debug.Log("TextInputController:OnMouseDown()");
		
		clicked = true;
	}
	
	void OnMouseUp()
	{
		//Debug.Log("TextInputController:OnMouseUp()");
	}
	
	void OnMouseEnter()
	{
		focused = true;
		
		text.scale = new Vector3(1.1f, 1.1f, 1.1f);
		text.Commit();
	}
	
	void OnMouseExit()
	{
		focused = false;
		text.scale = new Vector3(1.0f, 1.0f, 1.0f);
		text.Commit();
	}
}
