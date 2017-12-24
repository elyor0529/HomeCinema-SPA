using System.Data.Entity.ModelConfiguration;
using HomeCinema.Entities;

namespace HomeCinema.Data.Configurations
{
    public class EntityBaseConfiguration<T> : EntityTypeConfiguration<T> where T : class, IEntityBase
    {
        public EntityBaseConfiguration()
        {
            HasKey(e => e.ID);
        }
    }
}