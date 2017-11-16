

function Start () {
	
}

function Update () {
	
}

var consoleMainWnd = null;

function Log(str)
{
    
    if(consoleMainWnd == null)
    {
        consoleMainWnd = GameObject.Find("ConsoleMainWnd").GetComponent("ConsoleMainWindow");
    }
    if (consoleMainWnd != null && consoleMainWnd.gameObject.activeSelf != false) {

        consoleMainWnd.Log(str);
    }
}

function Warning(str) {

    if (consoleMainWnd == null)
    {
        consoleMainWnd = GameObject.Find("ConsoleMainWnd").GetComponent("ConsoleMainWindow");
         
    }
    if (consoleMainWnd != null && consoleMainWnd.gameObject.activeSelf != false)
    {
        consoleMainWnd.Warning(str);
    }
}

function Error(str) {


    if (consoleMainWnd == null) {

        consoleMainWnd = GameObject.Find("ConsoleMainWnd").GetComponent("ConsoleMainWindow");  
    }
    if (consoleMainWnd != null && consoleMainWnd.gameObject.activeSelf != false) {

        consoleMainWnd.Error(str);
    }
}


function execute(str)
{
    eval(str);
}