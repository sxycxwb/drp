﻿using System.Collections.Generic;
using System.Data;

namespace StackExchange.Profiling.SqlFormatters
{
    /// <summary>
    /// Takes a <c>SqlTiming</c> and returns a formatted SQL string, for parameter replacement, etc.
    /// </summary>
    public interface IAdvancedSqlFormatter : ISqlFormatter
    {
        /// <summary>
        /// Return SQL the way you want it to look on the in the trace. Usually used to format parameters.
        /// </summary>
        string FormatSql(string commandText, List<SqlTimingParameter> parameters, IDbCommand command = null);
    }
}
