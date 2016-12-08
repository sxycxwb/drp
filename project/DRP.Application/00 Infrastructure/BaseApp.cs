using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DRP.Code;

namespace DRP.Application
{
    public class BaseApp
    {
        public Log Logger
        {
            get { return LogFactory.GetLogger(this.GetType().ToString()); }
        }
    }
}
