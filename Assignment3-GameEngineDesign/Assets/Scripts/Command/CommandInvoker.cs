using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandInvoker : MonoBehaviour
{
    static Queue<ICommand> commandBuffer;

    static List<ICommand> commandHistory;
    static int counter;

    private bool dirtyFlag; //dirty flag

    private void Awake()
    {
        commandBuffer = new Queue<ICommand>();
        commandHistory = new List<ICommand>();

        dirtyFlag = false; //initalizing it to false
    }

    public static void AddCommand(ICommand command)
    {
        while (commandHistory.Count > counter)
        {
            commandHistory.RemoveAt(counter);
        }

        commandBuffer.Enqueue(command);
    }

    // Update is called once per frame
    void Update()
    {
        if (commandBuffer.Count > 0)
        {
            ICommand c = commandBuffer.Dequeue();
            c.Execute();

            //commandBuffer.Dequeue().Execute();

            commandHistory.Add(c);
            counter++;
            Debug.Log("Command history length: " + commandHistory.Count);
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.Z))
            {
                if (counter > 0)
                {
                    counter--;
                    commandHistory[counter].Undo();
                }
            }
            else if (Input.GetKeyDown(KeyCode.R))
            {
                if (counter < commandHistory.Count)
                {
                    commandHistory[counter].Execute();
                    counter++;
                }
            }
        }

        if (dirtyFlag) //checks if dirty flag is true
        {
            List<string> lines = new List<string>();

            foreach(ICommand c in commandHistory)
            {
                lines.Add(c.ToString());
            }


            System.IO.File.WriteAllLines(Application.dataPath + "/logfile.txt", lines);

            dirtyFlag = false; //reset dirty flag to false
        }

        if(Input.GetKeyDown(KeyCode.P))  //when you press P you save
        {
            dirtyFlag = true;
        }
    }
}

