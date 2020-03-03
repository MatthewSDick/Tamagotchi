using System;

namespace Tamagotchi.Models
{
  public class Pet
  {
    public int Id { get; set; }
    public string Name { get; set; }
    public DateTime Birthday { get; set; } = DateTime.Now;
    public DateTime DeathDate { get; set; }
    public int HungerLevel { get; set; }
    public int HappinessLevel { get; set; }
    public bool IsDead { get; set; } = false;
    public DateTime LastInteractedWithDate { get; set; } = DateTime.Now;
  }

}