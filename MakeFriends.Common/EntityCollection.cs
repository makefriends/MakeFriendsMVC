using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MakeFriends.Common
{
    public abstract class EntityCollection<T> : List<T>, IEntityCollection
        where T : Entity
    {
        public virtual bool GetAll()
        {
            Data.IDAL layer = Data.DALFactory.Create();
            return layer.GetCollection(this);
        }

        public virtual bool GetAllByParentId()
        {
            Data.IDAL layer = Data.DALFactory.Create();
            return layer.GetCollectionByParentID( this);
        }

        public virtual void Create()
        {
            foreach (ICreateIdentity entity in this)
            {
                entity.Create();
            }
        }

        public virtual void CreateAsChunk()
        {
            Data.IDAL layer = Data.DALFactory.Create();
            layer.Create(this);
        }

         
    }
}
