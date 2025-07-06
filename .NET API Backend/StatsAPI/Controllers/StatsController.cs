using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using StatsAPI.Models;

[ApiController]
[Route("[controller]")]
public class PlayerStatsController : ControllerBase
{
    private readonly StatsDbContext _db;  // Database context to access PlayerStats data

    // Constructor injects the database context
    public PlayerStatsController(StatsDbContext db)
    {
        _db = db;
    }

    // POST endpoint to save new player stats sent in the request body
    [HttpPost]
    public async Task<IActionResult> Post([FromBody] PlayerStats stats)
    {
        if (stats == null)
            return BadRequest("PlayerStats is null"); // Validate incoming data

        _db.Add(stats); // Add new stats record to the database context
        await _db.SaveChangesAsync(); // Persist changes asynchronously
        return Ok(new { message = "Stats saved" }); // Return success response
    }

    // GET endpoint to retrieve stats for a specific player by playerId
    [HttpGet("{playerId}")]
    public async Task<IActionResult> Get(int playerId)
    {
        // Query database for first matching player stats entry by playerId
        var stats = await _db.PlayerStats.FirstOrDefaultAsync(s => s.PlayerId == playerId);

        if (stats == null)
            return NotFound(); // Return 404 if player stats not found

        return Ok(stats); // Return found stats as JSON
    }
    
    // GET endpoint to retrieve all player stats records
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var stats = await _db.PlayerStats.ToListAsync(); // Fetch all player stats from DB
        return Ok(stats); // Return full list as JSON
    }
}
