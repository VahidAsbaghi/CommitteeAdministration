using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommitteeAdministration.ViewModels;
using CommitteeManagement.Model;

namespace CommitteeAdministration.Services
{
    public interface IRealValueAlarm
    {
        RealValueAlarmViewModel AlarmUsers(User user);
    }
}
