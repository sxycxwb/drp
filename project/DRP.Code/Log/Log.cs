/*******************************************************************************
 * Copyright © 2016 DRP.Framework 版权所有
 * Author: XuWangbin
 * Description: 分销系统
 * Website：
*********************************************************************************/
using log4net;

namespace DRP.Code
{
    public class Log
    {
        private ILog logger;
        public Log(ILog log)
        {
            this.logger = log;
        }
        public void Debug(object message)
        {
            this.logger.Debug(message);
        }
        public void Error(object message)
        {
            this.logger.Error(message);
        }
        public void Info(object message)
        {
            this.logger.Info(message);
        }
        public void Warn(object message)
        {
            this.logger.Warn(message);
        }
    }
}
