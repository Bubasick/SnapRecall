namespace SnapRecall.Domain;

public abstract class Command
{
    public long Id { get; set; }
    public string Name { get; set; }
    public List<Command> NextCommand { get; set; }
    public Command PreviousCommand { get; set; }

    public abstract void Execute();

    public virtual void Cancel()
    {
    }
}