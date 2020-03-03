using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Tamagotchi.Models;

namespace Tamagotchi.Controllers
{
  [Route("api/[controller]")]
  [ApiController]


  public class PetController : ControllerBase
  {
    public DatabaseContext db { get; set; } = new DatabaseContext();

    public bool isDead()
    {
      var rnd = new Random();
      var isDead = rnd.Next(10);
      if (isDead == 1)
      {
        return true;
      }
      else
      {
        return false;
      }

    }

    public void UpdateInteractive(int id)
    {

      if (id == 0)
      {
        for (var i = 1; i < db.Pets.Count(); i++)
        {
          var currPet = db.Pets.FirstOrDefault(p => p.Id == i);
          DateTime date1 = currPet.LastInteractedWithDate;
          DateTime date2 = DateTime.Today;
          int daysDiff = date2.Subtract(date1).Days;
          currPet.LastInteractedWithDate = DateTime.Now;
        }
      }
      else
      {
        var currPet = db.Pets.FirstOrDefault(p => p.Id == id);
        DateTime date1 = currPet.LastInteractedWithDate;
        DateTime date2 = DateTime.Today;
        int daysDiff = date2.Subtract(date1).Days;

        if (daysDiff > 3)
        {
          currPet.IsDead = true;
        }
        else
        {
          currPet.LastInteractedWithDate = DateTime.Now;
        }

      }
      db.SaveChanges();
    }

    // GET /api/pets, this should return all pets in your database.
    [HttpGet]
    public List<Pet> GetAllPets()
    {
      UpdateInteractive(0);
      var allPets = db.Pets.OrderBy(p => p.Name);
      return allPets.ToList();
    }

    // GET /api/pets/{id}, This should return the pet with the corresponding Id.
    [HttpGet("{id}")]
    public Pet GetSinglePet(int id)
    {
      UpdateInteractive(id);
      var pet = db.Pets.FirstOrDefault(p => p.Id == id);
      return pet;
    }

    [HttpGet("alive")]
    public List<Pet> GetAlivePets()
    {
      var allPets = db.Pets.Where(p => p.IsDead == false);
      return allPets.ToList();
    }

    // POST /api/pets, This should create a new pet. The body of the request should contain the name of the pet.
    [HttpPost]
    public Pet AddPet(Pet Pet)
    {
      db.Pets.Add(Pet);
      db.SaveChanges();
      return Pet;
    }

    // PUT /api/pets/{id}/play, This should find the pet by id, and add 5 to its happiness level and add 3 to its hungry level
    [HttpPatch("play/{id}")]
    public Pet PlayWithPet(int id)
    {
      UpdateInteractive(id);
      var item = db.Pets.FirstOrDefault(p => p.Id == id);
      item.HappinessLevel += 5;
      item.HungerLevel += 3;
      item.IsDead = isDead();
      if (item.IsDead)
      {
        item.DeathDate = DateTime.Now;
      }
      db.SaveChanges();
      return item;
    }
    // PUT /api/pets/{id}/feed, This should find the pet by id, and remove 5 from its hungry level and add 3 to its happiness level.
    [HttpPatch("feed/{id}")]
    public Pet FeedPet(int id)
    {
      UpdateInteractive(id);
      var item = db.Pets.FirstOrDefault(p => p.Id == id);
      item.HappinessLevel += 3;
      item.HungerLevel -= 5;
      item.IsDead = isDead();
      if (item.IsDead)
      {
        item.DeathDate = DateTime.Now;
      }
      db.SaveChanges();
      return item;
    }


    // PUT /api/pets/{id}/scold, This should find the pet by id, and remove 5 from its happiness level
    [HttpPatch("scold/{id}")]
    public Pet ScoldPet(int id)
    {
      UpdateInteractive(id);
      var item = db.Pets.FirstOrDefault(p => p.Id == id);
      item.HappinessLevel -= 5;
      item.IsDead = isDead();
      if (item.IsDead)
      {
        item.DeathDate = DateTime.Now;
      }
      db.SaveChanges();
      return item;
    }

    // DELETE /api/pets/{id}, this should delete a pet from the database by Id
    [HttpDelete("{id}")]
    public ActionResult KillPet(int id)
    {
      var item = db.Pets.FirstOrDefault(p => p.Id == id);
      if (item == null)
      {
        return NotFound();
      }
      db.Pets.Remove(item);
      db.SaveChanges();
      return Ok();
    }

  }
}