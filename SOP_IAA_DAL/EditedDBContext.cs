namespace SOP_IAA_DAL
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;

    public partial class Proyecto_IAAEntities : DbContext
    {

        // Este metodo sobrescribe al método del Model, por lo tanto se debe comentar el del context del model
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<itemReajuste>().Property(x => x.reajuste).HasPrecision(7, 6);
        }

        
    }
}