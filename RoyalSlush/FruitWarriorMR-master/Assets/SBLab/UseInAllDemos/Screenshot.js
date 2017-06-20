#pragma strict

var screenshotKey : KeyCode = KeyCode.Space;
var filename : String;
var superSize : int = 2;

private var counter : int;

function Update () {
	if(Input.GetKeyDown(screenshotKey)){
		Application.CaptureScreenshot(filename + counter.ToString() + ".png", superSize);
		counter++;
	}
}