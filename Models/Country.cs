namespace WebApplication1.Models
{
    public class Country
    {
        public int Id { get; set; }
        public string Countryname { get; set; }
    }

    public class Hobby
    {
        public int Id { get; set; }
        public string HobbyName { get; set; }
    }

    public class EmployeeDto
    {
        public string Name { get; set; }

        public int CountryId { get; set; }

        public int StateId { get; set; }

        public int CityId { get; set; }

        public string Gender { get; set; }

        public List<int> HobbyIds { get; set; }
    }

    public class EmployeeResponseDto
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string CountryName { get; set; }

        public string StateName { get; set; }

        public string CityName { get; set; }

        public string Gender { get; set; }

        public string Hobbies { get; set; }
    }

    public class EmployeeEditDto
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int CountryId { get; set; }

        public int StateId { get; set; }

        public int CityId { get; set; }

        public string Gender { get; set; }

        public List<int> HobbyIds { get; set; }
    }

    public class EmployeeUpdateDto
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int CountryId { get; set; }

        public int StateId { get; set; }

        public int CityId { get; set; }

        public string Gender { get; set; }

        public List<int> HobbyIds { get; set; }
    }

    public class RegisterDto
    {
        public string FullName { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }
    }

    public class LoginDto
    {
        public string Email { get; set; }

        public string Password { get; set; }
    }
}
