using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MakeFriends.Common
{
    public abstract class Entity
    {
        public virtual Int32 Create()
        {
            Data.IDAL layer = Data.DALFactory.Create();
            return layer.Create(this);
        }

        public virtual bool PopulateEntity()
        {
            Data.IDAL layer = Data.DALFactory.Create();
            return layer.GetOneItem(this);
        }

        public virtual bool GetByParentID()
        {
            Data.IDAL layer = Data.DALFactory.Create();
            return layer.GetByParentID(this);
        }

        public virtual void UpdateEntity()
        {
            Data.IDAL layer = Data.DALFactory.Create();
            layer.Update(this);
        }
    }
}
