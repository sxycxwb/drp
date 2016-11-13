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
            new ScheduleTaskApp().ProfitCalculateTask("674e8a26-cae3-4f38-ba37-1df0d978b4cc");
        }

        int Add(int x, int y)
        {
            return x + y;
        }
    }
}
