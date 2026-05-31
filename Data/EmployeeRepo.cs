namespace WebApplication1.Data
{
    public class EmployeeRepo
    {

        private readonly string _connectionString;
        public EmployeeRepo(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("Default");
        }


       
    }
}
