using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using hospins.Repository.Infrastructure;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Collections.Generic;
using hospins.Repository.DataServices;
using System.Data;

namespace hospins.Repository.Data
{
    /// <summary>
    /// To track audit data
    /// </summary>
    public partial class hospinsContext : DbContext
    {
        public static string ConnectionString { get; set; }

        public static void SetConnectionString(string connectionString)
        {
            if (ConnectionString == null)
            {
                ConnectionString = connectionString;
            }
            else
            {
                throw new Exception();
            }
        }

        public virtual int SaveChanges(int UserId, string mode = "")
        {
            var entities = base.ChangeTracker.Entries().Where(x => x.State == EntityState.Added || x.State == EntityState.Modified);
            var actionBy = UserId;
            foreach (var entity in entities)
            {
                if (entity.State == EntityState.Added)
                {
                    var isCreatedDateExists = entity.Properties.Any(t => t.Metadata.Name == "CreatedDate");
                    if (isCreatedDateExists)
                    {
                        entity.Property("CreatedDate").CurrentValue = DateTime.UtcNow;
                    }
                    var isCreatedByExists = entity.Properties.Any(t => t.Metadata.Name == "CreatedBy");
                    if (isCreatedByExists)
                    {
                        var createdBy = Convert.ToInt32(entity.Property("CreatedBy").CurrentValue);
                        entity.Property("CreatedBy").CurrentValue = (actionBy == 0 || createdBy > 0) ? entity.Property("CreatedBy").CurrentValue : actionBy;
                    }

                    var isModifyDateExists = entity.Properties.Any(t => t.Metadata.Name == "ModifyDate");
                    if (isModifyDateExists)
                    {
                        entity.Property("ModifyDate").CurrentValue = DateTime.UtcNow;
                    }

                    var isModifyByExists = entity.Properties.Any(t => t.Metadata.Name == "ModifyBy");
                    if (isModifyByExists)
                    {
                        entity.Property("ModifyBy").CurrentValue = actionBy == 0 ? entity.Property("ModifyBy").CurrentValue : actionBy;
                    }
                }
                else if (entity.State == EntityState.Modified)
                {
                    var isModifyDateExists = entity.Properties.Any(t => t.Metadata.Name == "ModifyDate");
                    if (isModifyDateExists)
                    {
                        entity.Property("ModifyDate").CurrentValue = DateTime.UtcNow;
                    }

                    var isModifyByExists = entity.Properties.Any(t => t.Metadata.Name == "ModifyBy");
                    if (isModifyByExists)
                    {
                        entity.Property("ModifyBy").CurrentValue = actionBy == 0 ? entity.Property("ModifyBy").CurrentValue : actionBy;
                    }
                }
            }
            return base.SaveChanges();
        }
    }

    public class ConnectionStrings
    {
        public string StrDbConn { get; set; }
    }
}