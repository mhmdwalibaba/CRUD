using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using ServiceContracts;
using ServiceContracts.DTO;
using ServiceContracts.Enums;
using Services;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CRUD.Controllers
{
    [Route("[controller]")]
    public class PersonsController : Controller
    {
        private readonly IPersonsService _personsService;
        private readonly ICountriesService _countriesService;
        public PersonsController(IPersonsService personsService,ICountriesService countriesService)
        {
            _personsService = personsService;
            _countriesService = countriesService;
        }

        [Route("/")]
        
        [Route("/persons/index")]
        public async Task<IActionResult> Index([FromQuery] string? searchBy, [FromQuery] string? searchString,string sortBy,SortOrderOptions sortOrder=SortOrderOptions.ASC)
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
           List<PersonResponse>? persons =await  _personsService.GetFiltredPerson(searchBy,searchString);
           ViewBag.CurrentSearchString=searchString;
            ViewBag.CurrentSearchBy = searchBy;

            //sort
            List<PersonResponse>? sortedPerson = await _personsService.GetSortedPersons(persons,sortBy, sortOrder);
            ViewBag.CurrentSortBy=sortBy;
            ViewBag.CurrentSortOrder = sortOrder.ToString();

            return View(sortedPerson);
        }
        [HttpGet]
        [Route("/persons/create")]
        public async Task<IActionResult> Create()
        {
            List<CountryResponse?> countries = await _countriesService.GetAllCountries();
            ViewBag.Countries = countries.Select(temp =>
               new SelectListItem() { Text = temp.CountryName, Value = temp.CountryID.ToString() }
             );
            return View();
        }
        [HttpPost]
        [Route("/persons/create")]
        
        public async Task<IActionResult> Create(PersonAddRequest personAddRequest)
        {
            if (!ModelState.IsValid)
            {
                List<CountryResponse?> countries = await _countriesService.GetAllCountries();
                ViewBag.Countries = countries.Select(temp =>
                new SelectListItem() { Text = temp.CountryName, Value = temp.CountryID.ToString() });

                ViewBag.Errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                return View();
            }

            //call the service method
            PersonResponse? personResponse = await _personsService.AddPerson(personAddRequest);

            //navigate to Index() action method (it makes another get request to "persons/index"
            return RedirectToAction("Index", "Persons");

            
        }
    }
}
