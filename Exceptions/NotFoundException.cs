namespace LMS___Mini_Version.Exceptions
{
    public class NotFoundException : Exception
    {
        public NotFoundException(string name, object key)
            : base($"{name} with key ({key}) was not found.")
        {
        }
    }
}
