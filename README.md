# Cheat Commands

Cheat Commands is a Unity tool designed to provide an in-editor and in-game command console, allowing developers and testers to execute methods and manipulate game variables easily. The tool supports both static and non-static methods, with real-time command suggestions and detailed command information.

## Features

- **Execute Commands Anywhere:** Use the `[Command]` attribute on any method and execute them directly from the editor console or in-game UI.
- **Input Parameters:** Supports methods with various input parameters including primitive types, strings, and Unity-specific types like `Vector3` and `Quaternion`.
- **Non-Static Methods:** Execute both static and instance methods.
- **Real-Time Suggestions:** Show command suggestions and expected parameters as the user types.
- **Command Information:** Display detailed information about commands including descriptions and parameter types.
- **Command History:** Maintain a history of executed commands for quick recall.
- **Logs Console:** Custom logs console to provide an easy way to debug commands.
- **Auto-Complete:** Auto-complete commands based on user input.
- **In-Game UI:** Use a canvas-based UI to execute commands in-game.

## Installation
1. Download the latest package from [latest](https://github.com/Arnol12157/TGUtils.CheatCommands/releases/tag/Latest)

or
1. Clone this repository to your local machine:
    ```bash
    git clone https://github.com/Arnol12157/TGUtils.CheatCommands.git
    ```
2. Open your project in Unity.
3. Copy the `CheatCommands` folder into the `Assets` folder of your Unity project.

## Usage

### Setting Up Commands

1. **Create Commands:**
   Annotate any method with the `[Command]` attribute.
   ```csharp
   public class GameCommands : MonoBehaviour
   {
       [Command]
       public void Add()
       {
           // Your command logic
       }

       [Command]
       public void SetScale(Vector3 scale)
       {
           transform.localScale = scale;
       }
   }
   ```

2. **Syntax**
   There are some rules to use this tool, the most important is the command syntax, we always write first the `[Command]` and then the `[Parameters]`(Optional)
    ```csharp
   [Command] [Parameter]
   ```
   Some examples
   - With Vector3: `setscale (10,5,9)`
   - With Vector3 and Quaternion: `settransform (10,5,9) (5,9,6,1)`
   - With strings: `echo "Hello world"` or `echo Holas`
   - With int: `sum 5 9`
   - With floats: `sum 5.2 9.6`
   - Without parameters: `somecrazylogic`

3. **AutoComplete Commands**
- InGame UI
  ![image](https://github.com/Arnol12157/TGUtils.CheatCommands/assets/13397644/631d6cac-b3d2-4c56-8cfa-ba71bc5adbab)
- Window Editor
  ![image](https://github.com/Arnol12157/TGUtils.CheatCommands/assets/13397644/2df30927-862f-43c6-86c4-8b2babc2735a)

4. **View Command Information**
- InGame UI
  ![image](https://github.com/Arnol12157/TGUtils.CheatCommands/assets/13397644/ca288f47-a48a-42d0-b835-17824348649f)
- Window Editor
  ![image](https://github.com/Arnol12157/TGUtils.CheatCommands/assets/13397644/9332b38a-f226-4328-ae5c-d4fe40216d3e)

5. **Logs Console**
- InGame UI
  ![image](https://github.com/Arnol12157/TGUtils.CheatCommands/assets/13397644/bfebd1a9-dcc0-48d8-bdb0-156f063ce4ac)
- Window Editor
  ![image](https://github.com/Arnol12157/TGUtils.CheatCommands/assets/13397644/76152fa2-34e3-492a-b262-d82b508c7eb9)


