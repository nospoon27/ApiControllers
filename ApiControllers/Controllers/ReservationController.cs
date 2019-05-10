using ApiControllers.Models;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiControllers.Controllers
{
    [Route("api/[controller]")]
    public class ReservationController : Controller
    {
        private IRepository repository;
        public ReservationController(IRepository repo) => 
            repository = repo;

        // Invoke-RestMethod http://localhost:7000/api/reservation -Method GET
        [HttpGet]
        public IEnumerable<Reservation> Get() => 
            repository.Reservations;

        // Invoke-RestMethod http://localhost:7000/api/reservation/1 -Method GET
        [HttpGet("{id}")]
        public Reservation Get(int id) => 
            repository[id];

        // Invoke-RestMethod http://localhost:7000/api/reservation -Method POST -Body (@{clientName="Anne"; location="Meeting Room 4"} | ConvertTo-Json) -ContentType "application/json"
        [HttpPost]
        public Reservation Post([FromBody] Reservation res) =>
            repository.AddReservation(new Reservation
            {
                ClientName = res.ClientName,
                Location = res.Location
            });


        // Invoke-RestMethod http://localhost:7000/api/reservation -Method PUT -Body (@{reservationId="3"; clientName="Dim"; location="105 cabinet"} | ConvertTo-Json) -ContentType "application/json"
        [HttpPut]
        public Reservation Put([FromBody] Reservation res) =>
            repository.UpdateReservation(res);

        // Invoke-RestMethod http://localhost:7000/api/reservation/1 -Method PATCH -Body (@{ op = "replace"; path = "clientName"; value = "Bob" }, 
        // @{ op = "replace"; path = "location"; value = "Lecture Hall" } | ConvertTo-Json) -ContentType "application/json"
        [HttpPatch("{id}")]
        public StatusCodeResult Patch(int id, [FromBody]JsonPatchDocument<Reservation> patch)
        {
            Reservation res = Get(id);
            if(res != null)
            {
                patch.ApplyTo(res);
                return Ok();
            }
            return NotFound();
        }

        // Invoke-RestMethod http://localhost:7000/api/reservation -Method GET
        [HttpDelete("{id}")]
        public void Delete(int id) =>
            repository.DeleteReservation(id);
    }
}
