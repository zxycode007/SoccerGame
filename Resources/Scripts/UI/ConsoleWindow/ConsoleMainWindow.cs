using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Diagnostics;  
public class ConsoleMainWindow : MonoBehaviour
{
    public ConsoleContentWidget consoleContent;
    public InputField consoleInput;
    

    public void Log(string str)
    {
        if (consoleContent != null)
        {
            consoleContent.AddDefaultConsoleItem(str);
        }
    }

    public void Warning(string str)
    {
        if (consoleContent != null)
        {
            consoleContent.AddWarningMessage(str);
        }
    }

    public void Error(string str)
    {
        if (consoleContent != null)
        {
            consoleContent.AddErrorMessage(str);
        }
    }

    public void OnEndEdit(string str)
    {

        ConsoleExecutor consoleExecutor = (ConsoleExecutor)gameObject.GetComponent("ConsoleExecutor");
        consoleExecutor.execute(str);
         
    }

    public void OnClose()
    {
        gameObject.SetActive(false);
    }

    // Use this for initialization
    void Start()
    {
        consoleInput.onEndEdit.AddListener(OnEndEdit);

    }

    // Update is called once per frame
    void Update()
    {

    }
}
