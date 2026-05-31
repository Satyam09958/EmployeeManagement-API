using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Data;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MasterController : ControllerBase
    {
        private readonly MasterRepo _masterRepo;
        public MasterController(MasterRepo repo)
        {
            _masterRepo = repo;
        }

        [HttpGet("countries")]
        public IActionResult GetCountries()
        {
            var countries = _masterRepo.GetCountries();
            return Ok(countries);
        }

        [HttpGet("states/{countryId}")]
        public IActionResult GetStates(int countryId)
        {
            return Ok(_masterRepo.GetStates(countryId));
        }

        [HttpGet("cities/{stateId}")]
        public IActionResult GetCities(int stateId)
        {
            return Ok(_masterRepo.GetCities(stateId));
        }

        [HttpGet("hobbies")]
        public IActionResult GetHobbies()
        {
            return Ok(_masterRepo.GetHobbies());
        }

        [HttpPost("save-employee")]
        public IActionResult SaveEmployee(EmployeeDto model)
        {
            var id = _masterRepo.SaveEmployee(model);

            return Ok(new
            {
                Message = "Saved Successfully",
                EmployeeId = id
            });
        }

        [HttpGet("employees")]
        public IActionResult GetEmployees()
        {
            var data = _masterRepo.GetEmployees();

            return Ok(data);
        }

        [HttpGet("employee/{id}")]
        public IActionResult GetEmployeeById(int id)
        {
            return Ok(_masterRepo.GetEmployeeById(id));
        }

        [HttpPut("update-employee")]
        public IActionResult UpdateEmployee(EmployeeUpdateDto model)
        {
            _masterRepo.UpdateEmployee(model);

            return Ok(new
            {
                Message = "Updated Successfully"
            });
        }

        [HttpDelete("delete-employee/{id}")]
        public IActionResult DeleteEmployee(int id)
        {
            _masterRepo.DeleteEmployee(id);

            return Ok(new
            {
                Message = "Deleted Successfully"
            });
        }
    }
}
