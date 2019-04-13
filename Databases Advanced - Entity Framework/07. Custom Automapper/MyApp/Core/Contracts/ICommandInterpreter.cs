namespace MyApp.Core.Contracts
{
    interface ICommandInterpreter
    {
        string Read(string[] inputArgs);
    }
}
