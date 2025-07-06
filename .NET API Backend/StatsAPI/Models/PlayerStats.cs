namespace StatsAPI.Models
{
    public class PlayerStats
    {
        public int Id { get; set; }  // Primary key for the database table, unique identifier
        
        public int PlayerId { get; set; }  // Identifier for the player this stat belongs to
        
        public int EnemiesDefeated { get; set; }  // Number of enemies defeated by the player
        
        public int DamageDealt { get; set; }  // Total damage dealt by the player
        
        // Timestamp of when the stats were last updated/played, defaults to current UTC time
        public DateTime LastPlayed { get; set; } = DateTime.UtcNow;
    }
}
