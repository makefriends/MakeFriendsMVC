using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using MakeFriends.Common;
using MakeFriends.Common.Data;
using Microsoft.Practices.EnterpriseLibrary.Data;

namespace MakeFriends.DAL
{
    public abstract class DALBase
    {
        protected virtual void UpdateEntity(Entity entity)
        {
            Database db = DatabaseFactory.CreateDatabase();

            using (DbCommand cmd = db.GetSqlStringCommand("update"))
            {
                DBActionAttribute actions = Attribute.GetCustomAttributes(entity.GetType(), typeof(DBActionAttribute)).Cast<DBActionAttribute>().First(a => a.Type.Has<DBActionType>(DBActionType.Create));

                StringBuilder fileds = new StringBuilder();
                StringBuilder whereClause = new StringBuilder();

                DBFieldAttribute attribute;

                foreach (PropertyInfo prop in
                    entity.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public))
                {
                    if (Attribute.IsDefined(prop, typeof(DBFieldAttribute)))
                    {
                        attribute = Attribute.GetCustomAttribute(prop, typeof(DBFieldAttribute)) as DBFieldAttribute;

                        if (attribute.isKey)
                        {
                            whereClause.Append("where ");
                            whereClause.Append(attribute.Name);
                            whereClause.Append("= ");
                            whereClause.Append("@");
                            whereClause.Append(attribute.Name);
                        }
                        else
                        {
                            fileds.Append(attribute.Name);
                            fileds.Append("= ");
                            fileds.Append("@");
                            fileds.Append(attribute.Name);
                            fileds.Append(", ");
                        }
                        db.AddInParameter(cmd, "@" + attribute.Name, GetDBType(prop.PropertyType), attribute.GetDataBaseValue(prop, entity));
                    }
                }

                fileds.Remove(fileds.Length - 2, 1);

                string query = string.Format("update {0} set {1} {2}", actions.Name, fileds.ToString(), whereClause.ToString());

                cmd.CommandText = query;
                db.ExecuteNonQuery(cmd);
            }
        }

        protected virtual void FillOneRecord(IDataReader rdr, object entity)
        {
            DBFieldAttribute attribute;

            foreach (PropertyInfo prop in
                entity.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public))
            {
                if (Attribute.IsDefined(prop, typeof(DBFieldAttribute)))
                {
                    attribute = Attribute.GetCustomAttribute(prop, typeof(DBFieldAttribute)) as DBFieldAttribute;
                    prop.SetValue(entity, GetValue(rdr, rdr.GetOrdinal(attribute.Name), prop.PropertyType), null);
                }
            }
        }

        protected virtual void FillEntity(IDataReader rdr, Entity entity)
        {
            while (rdr.Read())
            {
                FillOneRecord(rdr, entity);
            }
        }

        protected virtual void FillCollection(IDataReader rdr, System.Collections.IList collection)
        {
            while (rdr.Read())
            {
                object entity = Activator.CreateInstance(GetGenericType(collection));
                FillOneRecord(rdr, entity);
                collection.Add(entity);
            }
        }

        protected virtual IDataReader GetReaderBySQL(string sql, CommandType commandType, DbParameter[] sqlParams)
        {
            Database db = DatabaseFactory.CreateDatabase();
            IDataReader rdr = null;
            DbCommand cmd = null;

            if (commandType == CommandType.StoredProcedure)
                cmd = db.GetStoredProcCommand("select");
            else
                cmd = db.GetSqlStringCommand("select");

            using (cmd)
            {
                foreach (DbParameter par in sqlParams)
                {
                    db.AddInParameter(cmd, par.ParameterName, par.DbType, par.Value);
                }

                cmd.CommandText = sql;
                rdr = db.ExecuteReader(cmd);
            }

            return rdr;
        }

        protected virtual void ExecuteNoneQuery(string sql, CommandType commandType, DbParameter[] sqlParams)
        {
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand cmd = null;

            if (commandType == CommandType.StoredProcedure)
                cmd = db.GetStoredProcCommand("select");
            else
                cmd = db.GetSqlStringCommand("select");

            using (cmd)
            {
                foreach (DbParameter par in sqlParams)
                {
                    db.AddInParameter(cmd, par.ParameterName, par.DbType, par.Value);
                }

                cmd.CommandText = sql;
                db.ExecuteNonQuery(cmd);
            }

        }

        protected virtual DataSet GetDataSetBySQL(string sql, CommandType commandType, DbParameter[] sqlParams)
        {
            Database db = DatabaseFactory.CreateDatabase();
            DataSet ds = null;            
            DbCommand cmd=null;

            if(commandType == CommandType.StoredProcedure)
                cmd = db.GetStoredProcCommand("select");
            else
                cmd = db.GetSqlStringCommand("select");

            using (cmd)
            {
                foreach (DbParameter par in sqlParams)
                {
                    db.AddInParameter(cmd, par.ParameterName, par.DbType, par.Value);
                }

                cmd.CommandText = sql;
                ds = db.ExecuteDataSet(cmd);
            }

            return ds;
        }

        protected virtual IDataReader GetReader(object entity, bool filterByPrimaryKey = false, bool filterByForeignKey = false)
        {
            Database db = DatabaseFactory.CreateDatabase();
            IDataReader rdr = null;

            using (DbCommand cmd = db.GetSqlStringCommand("select"))
            {

                StringBuilder fields = new StringBuilder();

                DBActionAttribute actions = Attribute.GetCustomAttributes(entity.GetType(), typeof(DBActionAttribute)).Cast<DBActionAttribute>().First();

                if (actions.Type.Has<DBActionType>(DBActionType.Get))
                {
                    string whereClause = string.Empty;

                    fields.Append("select ");

                    DBFieldAttribute attribute;

                    foreach (PropertyInfo prop in
                        entity.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public))
                    {
                        if (Attribute.IsDefined(prop, typeof(DBFieldAttribute)))
                        {
                            attribute = Attribute.GetCustomAttribute(prop, typeof(DBFieldAttribute)) as DBFieldAttribute;

                            if (attribute.isKey && filterByPrimaryKey)
                            {
                                db.AddInParameter(cmd, "@" + attribute.Name, GetDBType(prop.PropertyType), attribute.GetDataBaseValue(prop, entity));
                                whereClause += " where " + attribute.Name + " = @" + attribute.Name;
                            }
                            else if (attribute.IsSearchField && filterByForeignKey)
                            {
                                db.AddInParameter(cmd, "@" + attribute.Name, GetDBType(prop.PropertyType), attribute.GetDataBaseValue(prop, entity));
                                whereClause += " where " + attribute.Name + " = @" + attribute.Name;
                            }

                            fields.Append(attribute.Name);
                            fields.Append(", ");
                        }
                    }

                    fields.Remove(fields.Length - 2, 1);

                    fields.Append(" from ");
                    fields.Append(actions.Name);

                    if (filterByPrimaryKey || filterByForeignKey)
                    {
                        fields.Append(whereClause);
                    }

                    cmd.CommandText = fields.ToString();

                    rdr = db.ExecuteReader(cmd);
                }
            }

            return rdr;
        }

        protected virtual IDataReader GetReader(System.Collections.IList collection)
        {
            object entity = Activator.CreateInstance(GetGenericType(collection));
            return GetReader(entity);
        }

        protected virtual IDataReader GetReaderByForeignKey(System.Collections.IList collection)
        {
            object entity = collection[0];
            collection.Clear();

            //object entity = Activator.CreateInstance(GetGenericType(collection));
            return GetReader(entity, false, true);
        }

        protected int CreateCollection(IList enities)
        {
            int retVal = 0;
            Database db = DatabaseFactory.CreateDatabase();
            using (DbConnection connection = db.CreateConnection())
            {
                connection.Open();
                DbTransaction transaction = connection.BeginTransaction();

                long counter = 0;

                try
                {
                    foreach (object entity in enities)
                    {
                        using (DbCommand cmd = db.GetSqlStringCommand("insert"))
                        {
                            DBActionAttribute actions = Attribute.GetCustomAttributes(entity.GetType(), typeof(DBActionAttribute)).Cast<DBActionAttribute>().First(a => a.Type.Has<DBActionType>(DBActionType.Create));

                            StringBuilder fileds = new StringBuilder();
                            StringBuilder values = new StringBuilder();

                            DBFieldAttribute attribute;

                            foreach (PropertyInfo prop in
                                entity.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public))
                            {
                                if (Attribute.IsDefined(prop, typeof(DBFieldAttribute)))
                                {
                                    attribute = Attribute.GetCustomAttribute(prop, typeof(DBFieldAttribute)) as DBFieldAttribute;
                                    fileds.Append(attribute.Name);
                                    fileds.Append(", ");

                                    values.Append("@");
                                    values.Append(attribute.Name);

                                    values.Append(", ");

                                    db.AddInParameter(cmd, "@" + attribute.Name, GetDBType(prop.PropertyType), attribute.GetDataBaseValue(prop, entity));
                                }
                            }

                            fileds.Remove(fileds.Length - 2, 1);
                            values.Remove(values.Length - 2, 1);

                            string query = string.Format("insert into {0} ({1})values({2})", actions.Name, fileds.ToString(), values.ToString());

                            cmd.CommandText = query;
                            db.ExecuteNonQuery(cmd, transaction);
                            counter++;
                        }
                    }


                    transaction.Commit();
                    retVal = 1;
                }
                catch (Exception ex)
                {
                    Log.Log.Report(string.Format("Failed to insert collection : {0} ", ex.ToString()));
                    transaction.Rollback();
                    throw ex;
                }
                finally
                {
                    connection.Close();
                }
            }

            return retVal;
        }

        protected virtual Int32 CreateEntity(Entity entity)
        {
            Database db = DatabaseFactory.CreateDatabase();
            Int32 identity = -1;

            using (DbCommand cmd = db.GetSqlStringCommand("insert"))
            {
                string getIdentityQuery = string.Empty;
                DBActionAttribute actions = Attribute.GetCustomAttributes(entity.GetType(), typeof(DBActionAttribute)).Cast<DBActionAttribute>().First(a => a.Type.Has<DBActionType>(DBActionType.Create));

                StringBuilder fileds = new StringBuilder();
                StringBuilder values = new StringBuilder();

                DBFieldAttribute attribute;

                foreach (PropertyInfo prop in
                    entity.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public))
                {
                    if (Attribute.IsDefined(prop, typeof(DBFieldAttribute)))
                    {
                        attribute = Attribute.GetCustomAttribute(prop, typeof(DBFieldAttribute)) as DBFieldAttribute;

                        if (attribute.IsIdentity)
                        {
                            getIdentityQuery = "Select LAST_INSERT_ID();";
                        }
                        else
                        {
                            fileds.Append(attribute.Name);
                            fileds.Append(", ");
                            values.Append("@");
                            values.Append(attribute.Name);
                            values.Append(", ");

                            db.AddInParameter(cmd, "@" + attribute.Name, GetDBType(prop.PropertyType), attribute.GetDataBaseValue(prop, entity));
                        }
                    }
                }

                fileds.Remove(fileds.Length - 2, 1);
                values.Remove(values.Length - 2, 1);



                string query = string.Format("insert into {0} ({1})values({2});{3}", actions.Name, fileds.ToString(), values.ToString(), getIdentityQuery);

                cmd.CommandText = query;
                if (getIdentityQuery == string.Empty)
                    db.ExecuteNonQuery(cmd);
                else
                    identity = Convert.ToInt32(db.ExecuteScalar(cmd));
            }

            return identity;
        }

        Type GetGenericType(System.Collections.IList collection)
        {
            Type collectionType = collection.GetType();

            foreach (Type interfaceType in collectionType.GetInterfaces())
            {
                if (interfaceType.IsGenericType &&
                    interfaceType.GetGenericTypeDefinition()
                    == typeof(IList<>))
                {
                    return interfaceType.GetGenericArguments()[0];
                }
            }

            return null;
        }

        object GetValue(IDataReader rdr, int index, Type t)
        {
            if (rdr.IsDBNull(index))
            {
                return null;
            }

            switch (t.Name)
            {
                case "String":
                    return rdr.GetString(index);
                case "Int16":
                    return rdr.GetInt16(index);
                case "Int32":
                    return rdr.GetInt32(index);
                case "Int64":
                    return rdr.GetInt64(index);
                case "Double":
                    return rdr.GetDouble(index);
                case "Decimal":
                    return rdr.GetDecimal(index);
                case "DateTime":
                    return rdr.GetDateTime(index);
                case "Float":
                    return rdr.GetFloat(index);
                case "Boolean":
                    return rdr.GetBoolean(index);
                default:
                    return rdr.GetValue(index);

            }
        }

        public DbType GetDBType(Type t)
        {
            switch (t.Name)
            {
                case "String":
                    return DbType.String;
                case "Int16":
                    return DbType.Int16;
                case "Int32":
                    return DbType.Int32;
                case "Int64":
                    return DbType.Int64;
                case "Double":
                    return DbType.Double;
                case "Decimal":
                    return DbType.Decimal;
                case "DateTime":
                    return DbType.DateTime;
                case "Float":
                    return DbType.Decimal;
                case "Boolean":
                    return DbType.Boolean;
                default:
                    return DbType.Object;

            }
        }
    }
}
