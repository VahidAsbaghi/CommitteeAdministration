using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommitteeManagement.Model;

namespace CommitteeAdministration.Services
{
    /// <summary>
    /// 
    /// </summary>
    public interface IMockDataGenerator
    {
        /// <summary>
        /// Criteria the mock data generate.
        /// </summary>
        /// <param name="numberOfCriterion">The number of criterion.</param>
        /// <param name="committee">The committee.</param>
        /// <returns></returns>
        List<Criterion> CriterionMockDataGenerate(int numberOfCriterion, Committee committee);
    }
}
