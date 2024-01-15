// LocationsController.cs
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;

[Route("api/[controller]")]
[ApiController]
public class LocationsController : ControllerBase
{
    private static List<Location> _locations = new List<Location>
    {
        new Location { Name = "Location 1", Availability = new TimeSpan(10, 0, 0)},
        new Location { Name = "Location 2", Availability = new TimeSpan(11, 30, 0) },
    };

    [HttpGet]
    public IActionResult GetLocationsWithAvailability()
    {
        // Filter locations with availability between 10am and 1pm
        var filteredLocations = _locations.FindAll(location =>
            location.Availability >= new TimeSpan(10, 0, 0) && location.Availability <= new TimeSpan(13, 0, 0));

        return Ok(filteredLocations);
    }

    [HttpPost]
    public IActionResult AddLocation([FromBody] LocationDTO newLocationDTO)
    {
        if (newLocationDTO == null)
        {
            return BadRequest("Invalid location data");
        }

        if (TimeSpan.TryParse(newLocationDTO.Availability, out TimeSpan availabilityTimeSpan))
        {
            var newLocation = new Location
            {
                Name = newLocationDTO.Name,
                Availability = availabilityTimeSpan
            };

            _locations.Add(newLocation);

            return CreatedAtAction(nameof(GetLocationsWithAvailability), new { id = newLocation.Id }, newLocation);
        }
        else
        {
            return BadRequest("Invalid availability format");
        }
    }
}

public class Location
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Name { get; set; }
    public TimeSpan Availability { get; set; }
}

public class LocationDTO
{
    public string Name { get; set; }
    [DefaultValue("00:00:00")]
    public string Availability { get; set; }
}

