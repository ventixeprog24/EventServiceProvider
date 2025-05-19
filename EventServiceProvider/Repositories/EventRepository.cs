using EventServiceProvider.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.Internal;
using System.ComponentModel;
using System.Linq.Expressions;

namespace EventServiceProvider.Repositories;

public interface IEventRepository : IBaseRepository<EventEntity>
{
}
public class EventRepository(EventDbContext context) : BaseRepository<EventEntity>(context),IEventRepository 
{


}
