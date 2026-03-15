namespace LMS___Mini_Version.Errors.Exeptions
{
    public class NotFoundException : ApplicationException
    {
        public NotFoundException(string message) : base($"{message} Not Found") 
        {
            
        }

        public NotFoundException(string name , object id)
            : base($"{name} Is Not Found With Id {id} ")
        {

        }
    }
}
