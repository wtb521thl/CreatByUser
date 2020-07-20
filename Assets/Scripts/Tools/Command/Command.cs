
public class Command : ICommand
{
    IReciver[] recivers=new IReciver[1];
    public Command(IReciver _reciver)
    {
        recivers[0] = _reciver;
    }
    public Command(IReciver[] _reciver)
    {
        recivers = _reciver;
    }
    public void ExcuteCommand()
    {
        for (int i = 0; i < recivers.Length; i++)
        {
            recivers[i].Action();
        }
    }

    public void UndoCommand()
    {
        for (int i = 0; i < recivers.Length; i++)
        {
            recivers[i].UndoAction();
        }
    }
}
