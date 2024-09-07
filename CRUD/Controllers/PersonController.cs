using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using ServiceContracts;
using ServiceContracts.DTO;
using Services;
namespace CRUD.Controllers
{
    public class PersonController : Controller
    {
        private readonly IPersonsService _personsService;
        public PersonController(IPersonsService personsService)
        {
            _personsService = personsService;
        }

        [Route("/")]
        [Route("/persons/index")]
        public async Task<IActionResult> Index([FromQuery] string? searchBy, [FromQuery] string? searchString)
        {
            ViewBag.SearchFields = new Dictionary<string,string>()
            {
                {nameof(PersonResponse.PersonName),"PersonName" },
                {nameof(PersonResponse.Email),"Email" },
                {nameof(PersonResponse.Gender),"Gender" },
                {nameof(PersonResponse.Address),"Address" },
                {nameof(PersonResponse.CountryID),"Country" },
                {nameof(PersonResponse.DateOfBirth),"DateOfBirth" }
            };
           List<PersonResponse>? personResponses =await  _personsService.GetFiltredPerson(searchBy,searchString);
           ViewBag.CurrentSearchString=searchString;
            ViewBag.CurrentSearchBy = searchBy;
            return View(personResponses);
        }
    }
}
