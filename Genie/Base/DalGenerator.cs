﻿using System;
using System.Collections.Generic;
using System.Linq;
using Genie.Base.Abstract;
using Genie.Models.Abstract;
using Genie.Templates.Dapper;
using Genie.Templates.Extensions;
using Genie.Templates.Infrastructure;
using Genie.Templates.Infrastructure.Enum;
using Genie.Templates.Infrastructure.Filters;
using Genie.Templates.Infrastructure.Interfaces;
using Genie.Templates.Infrastructure.Models;
using Genie.Templates.Infrastructure.Repositories;
using Genie.Templates.SqlMaker;
using Genie.Templates.SqlMaker.Interfaces;

namespace Genie.Base
{
    internal class DalGenerator : IDalGenerator
    {
        public List<IContentFile> Generate(IDatabaseSchema schema, IBasicConfiguration configuration, IProcessOutput output)
        {

            output.WriteInformation("Generating files.");
            List<ITemplateFile> files;
            try
            {
                files = new List<ITemplateFile>
                {
                    new SqlMapper(@"Dapper\SqlMapper"),
                    new CustomPropertyTypeMap(@"Dapper\CustomPropertyTypeMap"),
                    new DbString(@"Dapper\DbString"),
                    new DefaultTypeMap(@"Dapper\DefaultTypeMap"),
                    new DynamicParameters(@"Dapper\DynamicParameters"),
                    new FeatureSupport(@"Dapper\FeatureSupport"),
                    new ISqlAdapter(@"Dapper\ISqlAdapter"),
                    new KeyAttribute(@"Dapper\KeyAttribute"),
                    new PostgresAdapter(@"Dapper\PostgresAdapter"),
                    new SimpleMemberMap(@"Dapper\SimpleMemberMap"),
                    new SqlMapperExtensions(@"Dapper\SqlMapperExtensions"),
                    new SqlServerAdapter(@"Dapper\SqlServerAdapter"),
                    new TableAttribute(@"Dapper\TableAttribute"),
                    new WriteAttribute(@"Dapper\WriteAttribute"),

                    new ConditionExtension(@"Infrastructure\Enum\ConditionExtension"),
                    new Filters(@"Infrastructure\Filters\Filter"),
                    new RepositoryImplementation(@"Infrastructure\Repositories\Repositories", schema.Relations, schema.Views),
                    new IDapperContext(@"Infrastructure\Interfaces\IDapperContext"),
                    new IRepositoryFactory(@"Infrastructure\Interfaces\IRepositoryFactory"),
                    new IRepository(@"Infrastructure\Interfaces\IRepository"),
                    new IUnitOfWork(schema, @"Infrastructure\Interfaces\IUnitOfWork"),
                    new IReadOnlyRepository(@"Infrastructure\Interfaces\IReadOnlyRepository"),
                    new IProcedureContainer(@"Infrastructure\Interfaces\IProcedureContainer", schema),
                    new IOperation(@"Infrastructure\Interfaces\IOperation"),

                    new DapperContext(@"Infrastructure\DapperContext"),
                    new RepositoryFactory(@"Infrastructure\RepositoryFactory"),
                    new Repository(@"Infrastructure\Repository"),
                    new UnitOfWork(schema, @"Infrastructure\UnitOfWork"),
                    new ReadOnlyRepository(@"Infrastructure\ReadOnlyRepository"),
                    new ProcedureContainer(@"Infrastructure\ProcedureContainer", schema),
                    new Operation(@"Infrastructure\Operation"),

                    new BaseModel(@"Infrastructure\Models\BaseModel"),


                    new QueryMaker(@"SqlMaker\QueryMaker"),
                    new ISqlFirst(@"SqlMaker\Interfaces\ISqlFirst"),
                    new ISqlMaker(@"SqlMaker\Interfaces\ISqlMaker"),
                    new ISqlMakerDelete(@"SqlMaker\Interfaces\ISqlMakerDelete"),
                    new ISqlMakerInsert(@"SqlMaker\Interfaces\ISqlMakerInsert"),
                    new ISqlMakerSelect(@"SqlMaker\Interfaces\ISqlMakerSelect"),
                    new ISqlMakerUpdate(@"SqlMaker\Interfaces\ISqlMakerUpdate"),
                    new ISqlMkrBase(@"SqlMaker\Interfaces\ISqlMkrBase")
                };

                files.AddRange(schema.Relations
                    .Select(relation => new Relation(relation, @"Infrastructure\Models\" + relation.Name)));

                files.AddRange(schema.Views
                    .Select(view => new View(view, @"Infrastructure\Models\" + view.Name)));

                output.WriteInformation(string.Format("{0} file found.", files.Count));
            }
            catch (Exception e)
            {
                throw new Exception("Unable to create list of template files.", e);
            }

            try
            {
                GenerationContext.BaseNamespace = configuration.BaseNamespace;
                output.WriteInformation("Generating File content.");
                var contentFiles =
                    files.Select(templateFile => templateFile.Generate()).ToList();

                output.WriteSuccess(string.Format("Successfully generated {0} files.", contentFiles.Count));

                var comment = string.Format("/*This file is generated by Genie https://www.github.com/rusith/genie\n" +
                              "Modifications made to this file will overwrite by generator*/\n\n");

                foreach (var contentFile in contentFiles)
                {
                    contentFile.Path = contentFile.Path + "." + "cs";
                    contentFile.Content = comment + contentFile.Content;
                }

                return contentFiles;

            }
            catch (Exception e)
            {
                throw new Exception("Unable to generate file content", e);
            }
           
        }
    }
}
