using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace B4.EE.RodriguezA.Domain.Services
{
    public  interface IAlarmService
    {
        Task<bool> CreateCalendarForAppAlarmsAsync();

        Task<bool> CheckIfAlarmAlreadyExistAsync(string id);

        Task<string> CreateAlarmAsync(string title, string description, DateTime timeInit, DateTime timeEnd, int alarmMinutes);

        Task<bool> DeleteAlarmAsync(string id);


    }
}
