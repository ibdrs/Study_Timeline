namespace Study_Timeline.Logic.Exceptions
{
    public class ForbiddenException : Exception
    {
        public ForbiddenException(string message = "You are not allowed to do this.")
            : base(message) { }
    }
}
