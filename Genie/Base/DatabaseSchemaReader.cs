﻿using System;
using System.Collections.Generic;
using System.Linq;
using DatabaseSchemaReader;
using DatabaseSchemaReader.DataSchema;
using Genie.Base.Abstract;
using Genie.Models;
using Genie.Models.Abstract;
using DBReader = DatabaseSchemaReader;
using DBSchema = Genie.Base.DatabaseSchema;
namespace Genie.Base
{
    internal class DatabaseSchemaReader : IDatabaseSchemaReader
    {
        public IDatabaseSchema Read(IBasicConfiguration configuration, IProcessOutput output)
        {
            output.WriteInformation("Reading database meta data.");

            DBReader.DataSchema.DatabaseSchema schemaBase;
            try
            {
                using (var reader = new DatabaseReader(configuration.ConnectionString, SqlType.SqlServer))
                {
                    schemaBase = reader.ReadAll();
                }
            }
            catch (Exception e)
            {
                throw new Exception("Unable to read database meta data", e);
            }

            output.WriteSuccess("Schema reading successful.");


            output.WriteInformation("Parsing Schema.");

            IDatabaseSchema schema = new DBSchema();
            schema.Relations = new List<IRelation>();
            schema.BaseNamespace = configuration.BaseNamespace;
            schema.Procedures = new List<IStoredProcedure>();
            schema.Views = new List<IView>();

            if (schemaBase.Tables != null && schemaBase.Tables.Count > 0)
            {
                output.WriteInformation("Parsing tables.");
                foreach (var databaseTable in schemaBase.Tables)
                    schema.Relations.Add(ParseRelation(databaseTable, output));
                output.WriteSuccess("Parsing tables successful.");
            } else { output.WriteInformation("No Tables found in the database.");}

            if (schemaBase.Views != null && schemaBase.Views.Count > 0)
            {
                output.WriteInformation("Parsing views.");
                foreach (var view in schemaBase.Views)
                    schema.Views.Add(ParseView(view, output));
                output.WriteSuccess("Parsing views successful.");
            } else { output.WriteInformation("No views found in the database.");}

            if (schemaBase.StoredProcedures != null && schemaBase.StoredProcedures.Count > 0)
            {
                foreach (var sp in schemaBase.StoredProcedures)
                {
                    schema.Procedures.Add(ParseProcedure(sp, output));
                }
            } else { output.WriteInformation("No stored procedures found in the database.");}

            return schema;
        }

        private static IRelation ParseRelation(DatabaseTable table, IProcessOutput output)
        {
            output.WriteInformation("Parsing table " + table.Name);

            try
            {

                var realation = new Relation
                {
                    Name = table.Name,
                    Attributes = new List<IAttribute>(),
                    ForeignKeyAttributes = new List<IForeignKeyAttribute>()
                };

                foreach (var column in table.Columns)
                {
                    var attribute = new Models.Attribute
                    {
                        IsKey = column.IsPrimaryKey,
                        Name = column.Name,
                        DataType = column.DataType.NetDataTypeCSharpName,
                        FieldName = "_" + (column.Name.First() + "").ToLower() + column.Name.Substring(1)
                    };

                    if (column.IsForeignKey)
                    {
                        var fkAttribute = new ForeignKeyAttribute
                        {
                            ReferencingNonForeignKeyAttribute = attribute,
                            ReferencingRelationName = column.ForeignKeyTableName,
                        };

                        realation.ForeignKeyAttributes.Add(fkAttribute);
                    }
                    realation.Attributes.Add(attribute);
                }
                return realation;
            }
            catch (Exception e)
            {
                throw new Exception("Unable to parse Table " + table.Name, e);
            }
        }

        private static View ParseView(DatabaseTable databaseView, IProcessOutput output)
        {
            output.WriteInformation("Parsing view " + databaseView.Name);

            try
            {
                var view = new View
                {
                    Name = databaseView.Name,
                    Attributes = new List<ISimpleAttribute>()
                };

                foreach (var attribute in databaseView.Columns.Select(column => new SimpleAttribute
                {
                    Name = column.Name,
                    DataType = column.DataType.NetDataTypeCSharpName,
                }))
                {
                    view.Attributes.Add(attribute);
                }

                return view;
            }
            catch (Exception e)
            {
                throw new Exception("Unable to parse view " + databaseView.Name, e);
            }
            
        }
        private static StoredProcedure ParseProcedure(DatabaseStoredProcedure databaseSp, IProcessOutput output)
        {
            output.WriteInformation("Parsing stored procedure " + databaseSp.Name);
            try
            {
                var parameters = databaseSp.Arguments;
                var parameterString = parameters.Aggregate("", (current, param) => current + string.Format("{0} {1}", param.DataType.NetDataTypeCSharpName, param.Name.Replace("@", "")) + ",");
                var parameterPassString = parameters.Aggregate("", (current, param) => current + string.Format("{1} = '\"+{0}+\"'", param.Name.Replace("@", ""), param.Name) + ",");

                return new StoredProcedure
                {
                    Name = databaseSp.Name,
                    ParamString = parameterString.TrimEnd(','),
                    PassString = parameterPassString.TrimEnd(',')
                };
            }
            catch (Exception e)
            {
                throw new Exception("Unable to parse stored procedure "+ databaseSp.Name, e);
            }
        }
    }
}
