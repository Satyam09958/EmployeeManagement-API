using Dapper;
using Microsoft.Data.SqlClient;
using WebApplication1.Models;

namespace WebApplication1.Data
{
    public class MasterRepo
    {
        private readonly string _context;
        public MasterRepo(IConfiguration configuration)
        {
            _context = configuration.GetConnectionString("Default");
        }

        public IEnumerable<Country> GetCountries()
        {
            using (var connection = new SqlConnection(_context))
            {
                connection.Open();
                var query = "SELECT * FROM Country";
                return connection.Query<Country>(query).ToList();
            }
        }

        public IEnumerable<State> GetStates(int countryId)
        {
            using var connection = new SqlConnection(_context);

            string query = @"SELECT * 
                             FROM State 
                             WHERE CountryId = @CountryId";

            return connection.Query<State>(query, new { CountryId = countryId });
        }

        public IEnumerable<City> GetCities(int stateId)
        {
            using var connection = new SqlConnection(_context);

            string query = @"SELECT * 
                             FROM City 
                             WHERE StateId = @StateId";

            return connection.Query<City>(query, new { StateId = stateId });
        }

        public IEnumerable<Hobby> GetHobbies()
        {
            using var connection = new SqlConnection(_context);

            string query = "SELECT * FROM Hobby";

            return connection.Query<Hobby>(query);
        }

        public int SaveEmployee(EmployeeDto model)
        {
            using var connection = new SqlConnection(_context);

            connection.Open();

            var employeeId = connection.ExecuteScalar<int>(
            @"INSERT INTO Employee
      (Name,CountryId,StateId,CityId,Gender)

      VALUES
      (@Name,@CountryId,@StateId,@CityId,@Gender);

      SELECT CAST(SCOPE_IDENTITY() AS INT)",

              model);

            foreach (var hobbyId in model.HobbyIds)
            {
                connection.Execute(
                @"INSERT INTO EmployeeHobby
          (EmployeeId,HobbyId)

          VALUES
          (@EmployeeId,@HobbyId)",

                  new
                  {
                      EmployeeId = employeeId,
                      HobbyId = hobbyId
                  });
            }

            return employeeId;
        }
        public IEnumerable<EmployeeResponseDto> GetEmployees()
        {
            using var connection = new SqlConnection(_context);

            string query = @"
    SELECT
        E.Id,
        E.Name,
        C.CountryName,
        S.StateName,
        CI.CityName,
        E.Gender,

        STRING_AGG(H.HobbyName, ', ') AS Hobbies

    FROM Employee E

    INNER JOIN Country C
        ON E.CountryId = C.Id

    INNER JOIN State S
        ON E.StateId = S.Id

    INNER JOIN City CI
        ON E.CityId = CI.Id

    LEFT JOIN EmployeeHobby EH
        ON E.Id = EH.EmployeeId

    LEFT JOIN Hobby H
        ON EH.HobbyId = H.Id

    GROUP BY
        E.Id,
        E.Name,
        C.CountryName,
        S.StateName,
        CI.CityName,
        E.Gender";

            return connection.Query<EmployeeResponseDto>(query);
        }

        public EmployeeEditDto GetEmployeeById(int id)
        {
            using var connection = new SqlConnection(_context);

            var employee = connection.QueryFirstOrDefault<EmployeeEditDto>(
            @"SELECT
        Id,
        Name,
        CountryId,
        StateId,
        CityId,
        Gender
      FROM Employee
      WHERE Id=@Id",
              new { Id = id });

            if (employee != null)
            {
                employee.HobbyIds = connection.Query<int>(
                @"SELECT HobbyId
          FROM EmployeeHobby
          WHERE EmployeeId=@EmployeeId",
                  new { EmployeeId = id }).ToList();
            }

            return employee;
        }

        public bool UpdateEmployee(EmployeeUpdateDto model)
        {
            using var connection = new SqlConnection(_context);

            connection.Open();

            connection.Execute(
            @"UPDATE Employee
      SET
        Name=@Name,
        CountryId=@CountryId,
        StateId=@StateId,
        CityId=@CityId,
        Gender=@Gender
      WHERE Id=@Id",
              model);

            connection.Execute(
            @"DELETE FROM EmployeeHobby
      WHERE EmployeeId=@EmployeeId",
              new { EmployeeId = model.Id });

            foreach (var hobbyId in model.HobbyIds)
            {
                connection.Execute(
                @"INSERT INTO EmployeeHobby
          (EmployeeId,HobbyId)
          VALUES
          (@EmployeeId,@HobbyId)",
                  new
                  {
                      EmployeeId = model.Id,
                      HobbyId = hobbyId
                  });
            }

            return true;
        }

        public bool DeleteEmployee(int id)
        {
            using var connection = new SqlConnection(_context);

            connection.Open();

            connection.Execute(
            @"DELETE FROM EmployeeHobby
      WHERE EmployeeId=@EmployeeId",
              new { EmployeeId = id });

            connection.Execute(
            @"DELETE FROM Employee
      WHERE Id=@Id",
              new { Id = id });

            return true;
        }

        public string Register(RegisterDto model)
        {
            using var connection = new SqlConnection(_context);

            var exists = connection.QueryFirstOrDefault<int>(
            @"SELECT COUNT(*)
      FROM Users
      WHERE Email=@Email",
              new { model.Email });

            if (exists > 0)
            {
                return "Email Already Exists";
            }

            connection.Execute(
            @"INSERT INTO Users
      (
        FullName,
        Email,
        Password
      )
      VALUES
      (
        @FullName,
        @Email,
        @Password
      )",
              model);

            return "Registration Successful";
        }

        public object Login(LoginDto model)
        {
            using var connection = new SqlConnection(_context);

            var user = connection.QueryFirstOrDefault(
            @"SELECT *
      FROM Users
      WHERE Email=@Email
      AND Password=@Password",
              model);

            return user;
        }
    }


}
