namespace MyApp.Core.Commands.Contracts
{
    interface ICommand
    {
        string Execute(string[] commandParams);
    }
}
