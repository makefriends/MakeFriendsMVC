using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MakeFriends.Common;
using MakeFriends.Common.Data;

namespace MakeFriends.DAL
{
    public class DAL : DALBase, IDAL
    {
        public void Update(object entity)
        {
            if (entity is IList)
            {
            }
            else
            {
                UpdateEntity((Entity)entity);
            }
        }

        public DataSet GetDataSet(string sql, CommandType commandType, DbParameter[] sqlParams)
        {
            DataSet ds = null;
            try
            {
                ds = GetDataSetBySQL(sql, commandType, sqlParams);
            }
            catch (Exception ex)
            {
                Log.Log.Report(ex.ToString());
            }
            return ds;
        }

        public IDataReader GetReader(string sql, CommandType commandType, DbParameter[] sqlParams)
        {
            IDataReader rdr = null;
            try
            {
                rdr = GetReaderBySQL(sql, commandType, sqlParams);
            }
            catch (Exception ex)
            {
                Log.Log.Report(ex.ToString());
            }
            return rdr;
        }

        public void ExecuteSql(string sql, CommandType commandType, DbParameter[] sqlParams)
        {
            try
            {
                ExecuteNoneQuery(sql, commandType, sqlParams);
            }
            catch (Exception ex)
            {
                Log.Log.Report(ex.ToString());
            }
        }

        public bool GetOneItem(Entity entity)
        {
            try
            {
                using (IDataReader rdr = GetReader(entity, true))
                {

                    FillEntity(rdr, entity);
                }
            }
            catch (Exception ex)
            {
                Log.Log.Report(ex.ToString());
            }

            return true;
        }

        public bool GetCollection(System.Collections.IList entities)
        {
            try
            {
                using (IDataReader rdr = GetReader(entities))
                {
                    FillCollection(rdr, entities);
                }
            }
            catch (Exception ex)
            {
                Log.Log.Report(ex.ToString());
            }

            return true;
        }

        public bool GetByParentID(object entity)
        {
            try
            {
                if (entity is IList)
                {
                    using (IDataReader rdr = GetReaderByForeignKey((IList)entity))
                    {
                        FillCollection(rdr, (IList)entity);
                    }
                }
                else
                {
                    using (IDataReader rdr = GetReader(entity, false, true))
                    {

                        FillEntity(rdr, (Entity)entity);
                    }
                }

            }
            catch (Exception ex)
            {
                Log.Log.Report(ex.ToString());
            }

            return true;
        }

        public bool GetCollectionByParentID(System.Collections.IList entities)
        {
            try
            {
                using (IDataReader rdr = GetReaderByForeignKey(entities))
                {
                    FillCollection(rdr, entities);
                }
            }
            catch (Exception ex)
            {
                Log.Log.Report(ex.ToString());
            }

            return true;
        }

        public bool Save(object entity)
        {
            throw new NotImplementedException();
        }

        public Int32 Create(object entity)
        {
            if (entity is IList)
            {
                return CreateCollection(entity as IList);
            }
            else
            {
                return CreateEntity((Entity)entity);
            }
        }

        public bool Delete(object entity)
        {
            throw new NotImplementedException();
        }

        public System.Data.IDataReader GetDataReader(object entity)
        {
            throw new NotImplementedException();
        }

        public T ExecuteScalar<T>(string command)
        {
            throw new NotImplementedException();
        }
    }
}
