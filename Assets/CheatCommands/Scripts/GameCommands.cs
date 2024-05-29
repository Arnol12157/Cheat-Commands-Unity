using UnityEngine;

public class GameCommands : MonoBehaviour
{
    [Command("echo", "Prints a message to the console.")]
    public void Echo(string message)
    {
        Debug.Log(message);
    }

    [Command("setscale", "Sets the scale of a GameObject.")]
    public void SetScale(string gameObjectName, Vector3 scale)
    {
        var obj = GameObject.Find(gameObjectName);
        if (obj != null)
        {
            obj.transform.localScale = scale;
            Debug.Log($"Set scale of {gameObjectName} to {scale}");
        }
        else
        {
            Debug.LogError($"GameObject '{gameObjectName}' not found  to {scale}");
        }
    }

    [Command("setenv", "simple test")]
    public void SetEnv()
    {
        Debug.Log("yesssss");
    }
}
