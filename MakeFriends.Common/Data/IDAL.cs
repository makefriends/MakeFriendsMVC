using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.Common;

namespace MakeFriends.Common.Data
{    
    public interface IDAL
    {
        void Update(object entity);
        bool GetOneItem(Entity entity);
        bool GetCollection(System.Collections.IList entities);
        bool GetCollectionByParentID(System.Collections.IList entities);
        bool GetByParentID(object entities);
        IDataReader GetReader(string sql, CommandType commandType, DbParameter[] sqlParams);
        void ExecuteSql(string sql, CommandType commandType, DbParameter[] sqlParams);
        DataSet GetDataSet(string sql, CommandType commandType, DbParameter[] sqlParams);
        bool Save(object entity);
        Int32 Create(object entity);
        bool Delete(object entity);
        System.Data.IDataReader GetDataReader(object entity);

        T ExecuteScalar<T>(string command);
    }    
}
