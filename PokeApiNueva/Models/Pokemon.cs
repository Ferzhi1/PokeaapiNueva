namespace PokeApiNueva.Models
{
    public class Pokemon
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public int Height { get; set; }

        public int Weight { get; set; }

        public List<AbilitySlot>Abilities { get; set; }
        public Sprites Sprites { get; set; }

        public List<StatSlot>Stats { get; set; }
        public List<TypeSlot> Types { get; set; }
    }
    public class AbilitySlot 
    {
        public Ability Ability { get; set; }    
       
    }
    public class Ability
    {
        public string Name { get; set; }

        public string Url { get; set; }

    }
    public class Sprites 
    {
        public string Front_Default {  get; set; }
    }
    public class StatSlot 
    {
        public int Base_Stat {get;set; }
        public int Effort { get; set; }

        public Stat Stat { get; set; }
    
    
    }
    public class Stat 
    {
        public string Name { get; set; }
        public string Url { get; set; }
    }
    public class TypeSlot
    {
        public int Slot { get; set; }
        public PokemonType Type { get; set; }
    }

    public class PokemonType
    {
        public string Name { get; set; }
        public string Url { get; set; }
    }
}
