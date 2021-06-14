namespace TagAC.Management.Domain.Commands
{
    public class CommandValidationResult
    {
        public bool IsSuccessful { get; set; } = true;
        public string Error { get; init; }

        public static CommandValidationResult Success => new CommandValidationResult();
        public static CommandValidationResult Fail(string error) => new CommandValidationResult { IsSuccessful = false, Error = error };
    }
}
