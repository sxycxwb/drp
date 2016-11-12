﻿using System.Collections.Generic;
using System.Data;

namespace StackExchange.Profiling.SqlFormatters
{
    /// <summary>
    /// Extensions for ISqlFormatter instances
    /// </summary>
    public static class SqlFormatterExtensions
    {
        /// <summary>
        /// Format sql using the FormatSql method available in the current <see cref="MiniProfiler.Settings.SqlFormatter"/>. 
        /// </summary>
        /// <remarks>It is preferable to use this rather than accessing <see cref="ISqlFormatter.FormatSql"/> directly, 
        /// as this method will detect whether an <see cref="IAdvancedSqlFormatter"/> is being used, and will access it properly. 
        /// This may be removed in a future major version when <see cref="IAdvancedSqlFormatter"/> can be consolidated back
        /// into <see cref="ISqlFormatter"/>.
        /// </remarks>
        /// <returns></returns>
        public static string GetFormattedSql(this ISqlFormatter sqlFormatter, string commandText, List<SqlTimingParameter> parameters, IDbCommand command = null)
        {
            var advancedFormatter = sqlFormatter as IAdvancedSqlFormatter;
            return advancedFormatter != null ? advancedFormatter.FormatSql(commandText, parameters, command) : sqlFormatter.FormatSql(commandText, parameters);
        }
    }
}
