using CommitteeAdministration.ViewModels;
using CommitteeManagement.Model;

namespace CommitteeAdministration.Services.Contract
{
    public interface IRealValueAlarm
    {
        RealValueAlarmViewModel AlarmUsers(User user);
    }
}
