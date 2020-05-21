using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommadManager : SingleMono<CommadManager>
{
    /// <summary>
    /// 最大命令
    /// </summary>
    public int maxCommand = 50;

    /// <summary>
    /// 命令执行队列
    /// </summary>
    public Queue<ICommand> commands = new Queue<ICommand>();
    /// <summary>
    /// 命令撤销队列
    /// </summary>
    public Stack<ICommand> unDoCommands = new Stack<ICommand>();
    /// <summary>
    /// 点击下一步的时候执行的命令
    /// </summary>
    public Stack<ICommand> nextCommands = new Stack<ICommand>();

    /// <summary>
    /// 用户新加命令
    /// </summary>
    /// <param name="command"></param>
    public void AddCommand(ICommand command)
    {
        //保证用户每次发送命令都可以覆盖撤销的命令
        if (nextCommands.Count > 0)
        {
            nextCommands.Clear();
        }
        if (commands.Count > maxCommand)
        {
            commands.Dequeue();
        }
        commands.Enqueue(command);

    }
    /// <summary>
    /// 执行下一个命令
    /// </summary>
    public void ExcuteCommand()
    {

        if (nextCommands.Count > 0)
        {
            ICommand tempNextCommand = nextCommands.Pop();

            tempNextCommand.ExcuteCommand();
            if (unDoCommands.Count > maxCommand)
            {
                unDoCommands.Pop();
            }
            unDoCommands.Push(tempNextCommand);//执行完的命令放到撤销栈中
            return;
        }

        if (commands.Count <= 0)
        {
            Debug.Log("命令执行没了");
            return;
        }

        ICommand tempCommand= commands.Dequeue();

        tempCommand.ExcuteCommand();
        if (unDoCommands.Count > maxCommand)
        {
            unDoCommands.Pop();
        }
        unDoCommands.Push(tempCommand);//执行完的命令放到撤销栈中
    }

    public void ExcuteAllCommand()
    {

        int tempI = commands.Count;
        for (int i = 0; i < tempI; i++)
        {

            ICommand tempCommand = commands.Dequeue();

            tempCommand.ExcuteCommand();
            if (unDoCommands.Count > maxCommand)
            {
                unDoCommands.Pop();
            }
            unDoCommands.Push(tempCommand);//执行完的命令放到撤销栈中
        }
    }

    /// <summary>
    /// 撤销命令
    /// </summary>
    public void UnDoCommand()
    {
        if (unDoCommands.Count <= 0)
        {
            Debug.Log("撤销命令没了");
            return;
        }
        ICommand tempCommand = unDoCommands.Pop();
        tempCommand.UndoCommand();
        if (nextCommands.Count > maxCommand)
        {
            nextCommands.Pop();
        }
        nextCommands.Push(tempCommand);//撤销完成的命令放回执行命令栈中，以便点击下一步的时候继续操作
    }
    /// <summary>
    /// 清除所有的命令
    /// </summary>
    public void ClearAllCommand()
    {
        commands.Clear();
        unDoCommands.Clear();
        nextCommands.Clear();
    }

    private void Update()
    {
        //if (Input.GetKeyDown(KeyCode.Q))
        //{
        //    AddCommand(new RectTransformCommand());
        //}

        //if (Input.GetKeyDown(KeyCode.E))
        //{
        //    ExcuteCommand();
        //}

        //if (Input.GetKeyDown(KeyCode.Space))
        //{
        //    UnDoCommand();
        //}
    }
}
