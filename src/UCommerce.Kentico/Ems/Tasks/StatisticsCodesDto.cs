using System.Collections.Generic;

namespace UCommerce.Kentico.Ems.Tasks
{
    public class StatisticsCodesDto
    {
        public StatisticsCodesDto()
        {
            StatisticsCodes = new List<string>();
        }

        /// <summary>Conversions performed during this test.</summary>
        public IList<string> StatisticsCodes { get; set; }
    }
}
