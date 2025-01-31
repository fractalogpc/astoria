namespace Console
{
  public abstract class ConsoleCommand : IConsoleCommand
  {
    public abstract string Command { get; }

    public abstract bool Process(string[] args);
    
    public abstract string WrongInputMessage { get; }
    public abstract string SuccessMessage { get; }
  }
}