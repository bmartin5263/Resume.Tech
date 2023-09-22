using Microsoft.EntityFrameworkCore;

namespace ResumeTech.Persistence.EntityFramework; 

public static class DatabaseBuilderUtils {
    
    public static void DefineTable<E>(this ModelBuilder builder) where E : class {
        builder.DefineTable<E>(typeof(E).Name);
    }
    
    public static void DefineTable<E>(this ModelBuilder builder, string tableName) where E : class {
        builder.Entity<E>().ToTable(tableName);
        builder.Entity<E>()
            .HasKey("Id");
        builder.Entity<E>()
            .Property("Id")
            .HasColumnName(tableName + "Id");
    }
    
}