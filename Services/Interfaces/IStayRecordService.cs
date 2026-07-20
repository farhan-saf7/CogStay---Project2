using System.Collections.Generic;
using System.Threading.Tasks;
using CogStayMVC.Models;

namespace CogStayMVC.Services.Interfaces;

public interface IStayRecordService
{
    Task<StayRecord?> GetByIdAsync(int id);
    Task<IEnumerable<StayRecord>> GetAllStaysAsync();
    Task<IEnumerable<StayRecord>> GetActiveStaysAsync();
    Task<StayRecord> CheckInAsync(int reservationId);
    Task<StayRecord> CheckOutAsync(int stayId);
}
