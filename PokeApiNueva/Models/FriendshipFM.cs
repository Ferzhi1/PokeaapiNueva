namespace PokeApiNueva.Models
{
    public class FriendshipFM
    {
        public int Id { get; set; }

        public int UserAId { get; set; }

        public int UserBId { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public FriendshipStatus Status { get; set; }
    }
    public enum FriendshipStatus
    {
        Pending = 0,
        Accepted = 1,
        Rejected = 2,
        None=3
    }


}
