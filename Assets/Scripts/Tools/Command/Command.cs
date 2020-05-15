
public class Command : ICommand
{
    IReciver reciver;
    public Command(IReciver _reciver)
    {
        reciver = _reciver;
    }

    public void ExcuteCommand()
    {
        reciver.Action();
    }

    public void UndoCommand()
    {
        reciver.UndoAction();
    }
}
