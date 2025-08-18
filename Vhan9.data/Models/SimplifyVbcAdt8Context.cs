using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;

namespace Vhan9.data.Models;

public partial class SimplifyVbcAdt8Context : DbContext
{
    public SimplifyVbcAdt8Context(DbContextOptions<SimplifyVbcAdt8Context> options)
        : base(options)
    {
        string projectPath = AppDomain.CurrentDomain.BaseDirectory;
        IConfigurationRoot configuration =
            new ConfigurationBuilder()
                .SetBasePath(projectPath)
        .AddJsonFile(MyConstants.AppSettingsFile)
        .Build();
        Database.SetCommandTimeout(9000);
        MyConnectionString =
            configuration.GetConnectionString(MyConstants.ConnectionString);
    }
    
    public string MyConnectionString { get; set; }

    public virtual DbSet<qy_GetVhanConfigOutputColumns> qy_GetVhanConfigOutputColumnsList { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<qy_GetVhanConfigOutputColumns>(entity =>
        {
            entity.HasNoKey();
        });
        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
