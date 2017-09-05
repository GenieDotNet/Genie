﻿using System.Collections.Generic;
using Genie.Base.Configuration.Abstract;
using Genie.Base.Reading.Abstract;
using Genie.Models.Abstract;

namespace Genie.Base.Generating.Absract
{
    /// <summary>
    /// Helps to generate a DAL content from a database schema and settings
    /// </summary>
    internal interface IContentGenerator
    {
        /// <summary>
        /// Generates list of file content using given schema and configurations
        /// </summary>
        /// <param name="schema">schema to use</param>
        /// <param name="configuration">basic configuration to use</param>
        /// <returns>list of files</returns>
        List<IContentFile> Generate(IDatabaseSchema schema, IConfiguration configuration);
    }
}