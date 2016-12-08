using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DRP.Application.DrpServManage;
using Xunit;

namespace Drp.UnitTest
{
    public class ScheduleTaskUnitTest
    {
        [Fact]
        public void PassingTest()
        {
            Assert.Equal(4, Add(2, 2));
        }

        [Fact]
        public void ProfitCalculateTaskTest()
        {
            new ScheduleTaskApp().ProfitCalculateTask("");
        }

        int Add(int x, int y)
        {
            return x + y;
        }
    }
}
