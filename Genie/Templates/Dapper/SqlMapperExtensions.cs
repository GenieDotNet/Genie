#region Usings



#endregion

using Genie.Core.Base.Generating;

namespace Genie.Core.Templates.Dapper
{
    internal class SqlMapperExtensionsTemplate : GenieTemplate
    {
        public SqlMapperExtensionsTemplate(string path) : base(path)
        {
        }

        public override string Generate()
        {
            var dapperUsing = GenerationContext.NoDapper ? "using Dapper;" : "";
            var write = GenerationContext.Core ? ".First()" : "[0]";
            var getTypeInfo = GenerationContext.Core ? "GetTypeInfo()." : "";
            L($@"

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;
using {GenerationContext.BaseNamespace}.Infrastructure.Models.Concrete;
using {GenerationContext.BaseNamespace}.Infrastructure.Filters.Abstract;
{dapperUsing}

namespace {GenerationContext.BaseNamespace}.Dapper
{{
	public static class SqlMapperExtensions
    {{
        public interface IProxy
        {{
            bool IsDirty {{ get; set; }}
        }}


        public class SqlWhereOrderCache
        {{
            public string Sql {{ get; set; }}
            public IEnumerable<string> Where {{ get; set; }}
            public IEnumerable<string> Order {{ get; set; }}
        }}

        private static readonly ConcurrentDictionary<RuntimeTypeHandle, IEnumerable<PropertyInfo>> KeyProperties = new ConcurrentDictionary<RuntimeTypeHandle, IEnumerable<PropertyInfo>>();
        private static readonly ConcurrentDictionary<RuntimeTypeHandle, IEnumerable<PropertyInfo>> IdentityProperties = new ConcurrentDictionary<RuntimeTypeHandle, IEnumerable<PropertyInfo>>();
        private static readonly ConcurrentDictionary<RuntimeTypeHandle, IEnumerable<PropertyInfo>> TypeProperties = new ConcurrentDictionary<RuntimeTypeHandle, IEnumerable<PropertyInfo>>();
        private static readonly ConcurrentDictionary<RuntimeTypeHandle, string> TypeTableName = new ConcurrentDictionary<RuntimeTypeHandle, string>();

        private static readonly Dictionary<string, ISqlAdapter> AdapterDictionary = new Dictionary<string, ISqlAdapter>() {{
                                                                                         {{""sqlconnection"", new SqlServerAdapter()}},
                                                                                        {{""npgsqlconnection"", new PostgresAdapter()}}
                                                                                    }};

        private static IEnumerable<PropertyInfo> KeyPropertiesCache(Type type)
        {{

            IEnumerable<PropertyInfo> pi;
            if (KeyProperties.TryGetValue(type.TypeHandle, out pi))
            {{
                return pi;
            }}

            var allProperties = TypePropertiesCache(type).ToList();
            var keyProperties = allProperties.Where(p => p.GetCustomAttributes(true).Any(a => a is KeyAttribute)).ToList();

            KeyProperties[type.TypeHandle] = keyProperties;
            return keyProperties;
        }}

        private static IEnumerable<PropertyInfo> IdentityPropertiesCache(Type type)
        {{

            IEnumerable<PropertyInfo> pi;
            if (IdentityProperties.TryGetValue(type.TypeHandle, out pi))
            {{
                return pi;
            }}

            var allProperties = TypePropertiesCache(type).ToList();
            var identityProperties = allProperties.Where(p => p.GetCustomAttributes(true).Any(a => a is IdentityAttribute)).ToList();

            IdentityProperties[type.TypeHandle] = identityProperties;
            return identityProperties;
        }}

        private static IEnumerable<PropertyInfo> TypePropertiesCache(Type type)
        {{
            IEnumerable<PropertyInfo> pis;
            if (TypeProperties.TryGetValue(type.TypeHandle, out pis))
            {{
                return pis;
            }}

            var properties = type.GetProperties().Where(IsWriteable).ToList();
            TypeProperties[type.TypeHandle] = properties;
            return properties;
        }}

        public static bool IsWriteable(PropertyInfo pi)
        {{
            var attributes = pi.GetCustomAttributes(typeof(WriteAttribute), false).ToList();
            if (attributes.Count != 1)
                return true;
            var write = (WriteAttribute)attributes{write};
            return write.Write;
        }}

	    /// <summary>
	    /// Return all  
	    /// </summary>
	    /// <typeparam name=""T"">Interface type to create and populate</typeparam>
	    /// <param name=""connection"">Open SqlConnection</param>
	    /// <param name=""query""></param>
	    /// <returns>Entity of T</returns>
	    public static IEnumerable<T> Get<T>(this IDbConnection connection, IRepoQuery query)
        {{
			using(connection = new SqlConnection(connection.ConnectionString))
			{{
				connection.Open();
				return connection.Query<T>(GetRetriveQuery(query), transaction: query.Transaction);
			}}
        }}

	    /// <summary>
	    /// Returns count of rows
	    /// </summary>
	    /// <param name=""connection"">Open SqlConnection</param>
	    /// <param name=""query""></param>
	    /// <returns>Entity of T</returns>
	    public static int Count(this IDbConnection connection, IRepoQuery query)
        {{
			using(connection = new SqlConnection(connection.ConnectionString))
			{{
				return connection.ExecuteScalar<int>(GetRetriveQuery(query, true), transaction: query.Transaction);
			}}
        }}

		/// <summary>
	    /// Returns the where clause of the resulting query
	    /// </summary>
	    /// <param name=""connection"">Open SqlConnection</param>
	    /// <param name=""query""></param>
	    /// <returns>Entity of T</returns>
	    public static string GetWhereClause(this IDbConnection connection, IRepoQuery query)
	    {{
	        return GetRetriveQuery(query, false, true);
	    }}

	    private static string GetRetriveQuery(IRepoQuery query, bool isCount = false, bool whereOnly = false)
	    {{
            var queryBuilder = new StringBuilder(whereOnly ? """" : string.Format(""select {{0}} {{1}} from "" + query.Target, query.Limit != null ? "" top "" + query.Limit : """", isCount ? ""count(*)"" : ""*""));
            
            var where = query.Where == null ? new Queue<string>() : new Queue<string>(query.Where);
            var order = query.Order == null ? new Queue<string>() : new Queue<string>(query.Order);

	        if (where.Count > 0)
	        {{
	            queryBuilder.Append("" where "");

	            var first = true;
	            var previous = """";

	            while (where.Count > 0)
	            {{
	                var current = where.Dequeue();

	                if (AndOrOr(current))
	                {{
	                    if (first)
	                    {{
	                        first = false;
	                        previous = current;
	                        continue;
	                    }}

	                    if (AndOrOr(previous))
	                    {{
	                        previous = current;
	                        continue;
	                    }}

	                    previous = current;
	                    queryBuilder.Append($"" {{current}} "");
	                }}
                    else if (current == "")"" || current == ""("")
	                {{
	                    queryBuilder.Append($"" {{current}} "");
                    }}
	                else
	                {{
	                    if (!first && (previous != ""("" && previous != "")"") && !AndOrOr(previous))
                        {{
	                        queryBuilder.Append("" and "");
	                    }}

	                    previous = current;
	                    queryBuilder.Append($"" {{current}} "");
	                }}

	                first = false;
	            }}
	        }}

	        if (whereOnly)
	            return queryBuilder.ToString();

	        if (order.Count > 0)
	        {{
	            queryBuilder.Append("" order by "");
	            while (order.Count > 0)
	            {{
	                var item = order.Dequeue();
	                queryBuilder.Append($"" {{item}} "");
	            }}
	        }}

	        if (query.Page != null && query.PageSize != null)
	        {{
	            queryBuilder.Append($"" OFFSET ({{query.Page * query.PageSize}}) ROWS "" + $"" FETCH NEXT {{query.PageSize}} ROWS ONLY "");
	        }}
	        else
	        {{
	            if (query.Skip != null)
	                queryBuilder.Append($"" OFFSET ({{query.Skip}}) ROWS "");

	            if (query.Take != null)
	                queryBuilder.Append($"" FETCH NEXT {{query.Take}} ROWS ONLY "");
	        }}

	        return queryBuilder.ToString();
        }}

        private static bool AndOrOr(string str)
	    {{
	        return str == ""and"" || str == ""or"";
	    }}


        private static string GetTableName(Type type)
        {{
            string name;
            if (TypeTableName.TryGetValue(type.TypeHandle, out name)) return name;
            name = type.Name + ""s"";
            if (type.{getTypeInfo}IsInterface && name.StartsWith(""I""))
                name = name.Substring(1);

            var tableattr = type.{
                    getTypeInfo
                }GetCustomAttributes(false).SingleOrDefault(attr => attr.GetType().Name == ""TableAttribute"") as
                dynamic;
            if (tableattr != null)
                name = tableattr.Name;
            TypeTableName[type.TypeHandle] = name;
            return name;
        }}

	    /// <summary>
	    /// Inserts an entity into table ""Ts"" and returns identity id.
	    /// </summary>
	    /// <param name=""connection"">Open SqlConnection</param>
	    /// <param name=""entityToInsert"">Entity to insert</param>
	    /// <param name=""transaction""></param>
	    /// <param name=""commandTimeout""></param>
	    /// <returns>Identity of inserted entity</returns>
	    public static long? Insert(this IDbConnection connection, BaseModel entityToInsert, IDbTransaction transaction = null, int? commandTimeout = null)
      {{

          var type = entityToInsert.GetType();

          var name = GetTableName(type);

          var sbColumnList = new StringBuilder(null);

          var allProperties = TypePropertiesCache(type).ToList();
          var keyProperties = KeyPropertiesCache(type).ToList();
          var identityProperties = IdentityPropertiesCache(type).ToList();
          var allPropertiesExceptIndentity = allProperties.Except(identityProperties).ToList();

	        var index = 0;
	        var lst = allProperties.Count == keyProperties.Count ? keyProperties : allPropertiesExceptIndentity;
            foreach (var property in lst)
	        {{
                sbColumnList.AppendFormat(""[{{0}}]"", property.Name);
                if (index < lst.Count - 1)
                    sbColumnList.Append("", "");
	            index ++;
	        }}

	        index = 0;
            var sbParameterList = new StringBuilder(null);

            foreach (var property in lst)
            {{
                sbParameterList.AppendFormat(""@{{0}}"", property.Name);
                if (index < lst.Count - 1)
                    sbParameterList.Append("", "");
                index++;
            }}
            
            var adapter = GetFormatter(connection);
			using(connection = new SqlConnection(connection.ConnectionString))
			{{
				connection.Open();
				var id = adapter.Insert(connection, transaction, commandTimeout, name, sbColumnList.ToString(), sbParameterList.ToString(), keyProperties, entityToInsert);
				return id;
			}}
        }}

	    /// <summary>
	    /// Updates entity in table ""Ts"", checks if the entity is modified if the entity is tracked by the Get() extension.
	    /// </summary>
	    /// <param name=""connection"">Open SqlConnection</param>
	    /// <param name=""entityToUpdate"">Entity to be updated</param>
	    /// <param name=""transaction""></param>
	    /// <param name=""commandTimeout""></param>
	    /// <returns>true if updated, false if not found or not modified (tracked entities)</returns>
	    public static bool Update(this IDbConnection connection, BaseModel entityToUpdate, IDbTransaction transaction = null, int? commandTimeout = null)
        {{
            if (entityToUpdate.DatabaseModelStatus != ModelStatus.Retrieved)
                return false;

            if (entityToUpdate.UpdatedProperties == null || entityToUpdate.UpdatedProperties.Count < 1)
                return false;

            var type = entityToUpdate.GetType();

            var keyProperties = KeyPropertiesCache(type).ToList();
            if (!keyProperties.Any())
                throw new ArgumentException(""Entity must have at least one [Key] property"");

            var name = GetTableName(type);

            var sb = new StringBuilder();
            sb.AppendFormat(""update {{0}} set "", name);

            var allProperties = TypePropertiesCache(type);
            var nonIdProps = allProperties.Where(a => !keyProperties.Contains(a) && entityToUpdate.UpdatedProperties.Contains(a.Name)).ToList(); // Only updated properties


            for (var i = 0; i < nonIdProps.Count; i++)
            {{
                var property = nonIdProps.ElementAt(i);
                sb.AppendFormat(""[{{0}}] = @{{1}}"", property.Name, property.Name);
                if (i < nonIdProps.Count - 1)
                    sb.AppendFormat("", "");
            }}

            sb.Append("" where "");
            for (var i = 0; i < keyProperties.Count; i++)
            {{
                var property = keyProperties.ElementAt(i);
                sb.AppendFormat(""[{{0}}] = @{{1}}"", property.Name, property.Name);
                if (i < keyProperties.Count - 1)
                    sb.AppendFormat("" and "");
            }}

			using(connection = new SqlConnection(connection.ConnectionString))
			{{
				connection.Open();
				var updated = connection.Execute(sb.ToString(), entityToUpdate, commandTimeout: commandTimeout, transaction: transaction);
				return updated > 0;
			}}
        }}

	    /// <summary>
	    /// Delete entity in table ""Ts"".
	    /// </summary>
	    /// <param name=""connection"">Open SqlConnection</param>
	    /// <param name=""entity""></param>
	    /// <param name=""transaction""></param>
	    /// <param name=""commandTimeout""></param>
	    /// <returns>true if deleted, false if not found</returns>
	    public static bool Delete(this IDbConnection connection, BaseModel entity, IDbTransaction transaction = null, int? commandTimeout = null)
        {{
	        if (entity == null)
	        {{
                throw new ArgumentException(""The entity is null, cannot delete a null entity"", nameof(entity));
            }}

            var type = entity.GetType();
            var keyProperties = KeyPropertiesCache(type).ToList();

            if (!keyProperties.Any())
                throw new ArgumentException(""Entity must have at least one [Key] property"");

            var name = GetTableName(type);

            var sb = new StringBuilder();
            sb.AppendFormat(""delete from {{0}} where "", name);

            for (var i = 0; i < keyProperties.Count; i++)
            {{
                var property = keyProperties.ElementAt(i);
                sb.AppendFormat(""[{{0}}] = @{{1}}"", property.Name, property.Name);
                if (i < keyProperties.Count - 1)
                    sb.AppendFormat("" and "");
            }}

			using(connection = new SqlConnection(connection.ConnectionString))
			{{
				connection.Open();
				var deleted = connection.Execute(sb.ToString(), entity, transaction: transaction, commandTimeout: commandTimeout) > 0;
				return deleted;
			}}
        }}

        public static ISqlAdapter GetFormatter(IDbConnection connection)
        {{
            var name = connection.GetType().Name.ToLower();
            return !AdapterDictionary.ContainsKey(name) ? new SqlServerAdapter() : AdapterDictionary[name];
        }}
    }}
}}");

            return E();
        }
    }
}