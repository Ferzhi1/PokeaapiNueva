namespace PokeApiNueva.Models
{
    public class CollectionUserPkm
    {
        public int PokemonId { get; set; }
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Name { get; set; }
        public int Count { get; set; } = 1;
        public DateTime CaughtAt { get; set; }

        public Userpkm User { get; set; }

    
    }
}
