using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CommitteeAdministration.Helper;
using CommitteeAdministration.ViewModels;
using CommitteeManagement.Model;
using CommitteeManagement.Repository;
using Microsoft.Practices.Unity;

namespace CommitteeAdministration.Services
{
    public class RealValueAlarm:IRealValueAlarm
    {
        private readonly IMainContainer _mainContainer = ModelContainer.Instance.Resolve<IMainContainer>();
        private readonly ICommitteeStatus _committeeStatus = ModelContainer.Instance.Resolve<ICommitteeStatus>();
        /// <summary>
        /// Alarms the users to update real values.
        /// </summary>
        /// <param name="logedInUser">The loged in user.</param>
        /// <returns></returns>
        public RealValueAlarmViewModel AlarmUsers(User logedInUser)
        {
            //get committee Id
            var committeId = logedInUser.Committee.Id;

            //get committee from user
            var committee = logedInUser.Committee;// _mainContainer.CommitteeRepository.FirstOrDefault(committeeT => committeeT.Id == committeId);

            //get users in committee
            var users = _mainContainer.UserRepository.Where(userT => userT.Committee.Id == committeId);

            //get all indicator modifications from users in committee
            var indicatorModifications = (from indicatorModification in _mainContainer.IndicatorModificationRepository.All()
                     where (from user in users where user.Id == indicatorModification.User.Id select user.Id).Any()
                select indicatorModification).Distinct();

            //get all indicators that are associated with modifications
            var indicators =
                _mainContainer.IndicatorRepository.All()
                    .Where(indicatorT => indicatorModifications.Select(indicatorModifyT => indicatorModifyT.IndicatorId).Contains(indicatorT.Id)).Distinct();

            //get all real values of indicators. every indicator has some real values.
            var realValues = _mainContainer.IndicatorRealValueRepository.All().Where(realValueT=>indicators.Select(indicatorT=>indicatorT.Id).Contains(realValueT.Indicator.Id)).Distinct();

            var realValueAlramModel=new RealValueAlarmViewModel();
            foreach (var indicator in indicators)
            {
                //select some realValues from all realValues that is related to the indicator 
                var realValuesIndicator = realValues.Where(realValueT => realValueT.Indicator.Id == indicator.Id);

                //select latest realValue... 
                var selectedRealValue = _committeeStatus.FindFitestRealValue(realValuesIndicator, DateTime.Now);

                //get period of last update of real value from now
                var realValueTimeSpan = selectedRealValue.Time.GetValueOrDefault().Subtract(DateTime.Now);

                //get deadLine period from days.
                var deadlineTimeSpan = TimeSpan.FromDays(indicator.DeadlinePeriod.GetValueOrDefault());

                //if deadLine period reaches, the user should be alarmed
                if (realValueTimeSpan>deadlineTimeSpan)
                {
                    realValueAlramModel.IndicatorsExpirationDays.Add(new Tuple<Indicator, int>(indicator, (int)(realValueTimeSpan.TotalDays - deadlineTimeSpan.TotalDays)));                   
                }
            }
            realValueAlramModel.User=logedInUser;
            return realValueAlramModel;
        }
    }

    
}