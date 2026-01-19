namespace Study_Timeline.Logic.Exceptions
{
    public class ValidationException : Exception
    {
        public string? Field { get; }

        public ValidationException(string message, string? field = null)
            : base(message)
        {
            Field = field;
        }
    }
}
