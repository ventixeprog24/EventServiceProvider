using EventServiceProvider.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.Internal;
using System.ComponentModel;
using System.Linq.Expressions;

namespace EventServiceProvider.Repository;

public class EventRepository(EventDbContext context)
{
    private readonly EventDbContext _context = context;

    public async Task<IEnumerable<EventEntity>> GetAllEventsAsync()
    {        
        var events = await _context.Events.ToListAsync();
        if (events != null)
        {
            return events;

        }
        return null!;
    }
    public async Task<EventEntity> GetEventByIdAsync(string id)
    {
        var eventEntity = await _context.Events.FirstOrDefaultAsync(e => e.EventId == id);

        if (eventEntity != null)
        {
            return eventEntity;
        }
        return null!;
    }

    public async Task<bool> CreateEventAsync(EventEntity eventEntity)
    {
        if (eventEntity == null)
        {
            return false;
        }
        _context.Events.Add(eventEntity);
        await _context.SaveChangesAsync();
        return true;
    }


}
