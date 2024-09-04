using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entits;
namespace ServiceContracts.DTO
{
    public class PersonAddRequest
    {
        public string? PersonName { get; set; }
        public string? Email { get; set; }
        public GenderOptions? Gender { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public Guid CountryID { get; set; }
        public string? Address { get; set; }
        public bool ReceiveNewsLetters { get; set; }


        /// <summary>
        /// Converts the current object of PersonAddRequest into a new object of Person type
        /// </summary>
        /// <returns></returns>
        public Person ToPerson()
        {
            return new Person()
            {
                PersonName = this.PersonName,
                Email = this.Email,
                Gender = this.Gender.ToString(),
                DateOfBirth = this.DateOfBirth,
                CountryID = this.CountryID,
                Address = this.Address,
                ReceiveNewsLetters = this.ReceiveNewsLetters
            };
        }
    }
}
