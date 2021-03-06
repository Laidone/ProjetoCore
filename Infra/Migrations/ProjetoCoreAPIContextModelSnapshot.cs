using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infra.Migrations
{
    [DbContext(typeof(ProjetoCoreAPIContext))]
    partial class ProjetoCoreAPIContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.12")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("ProjetoCore.API.Models.Genre", b =>
            {
                b.Property<int>("GenreID")
                    .ValueGeneratedOnAdd()
                    .HasColumnType("int")
                    .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                b.Property<string>("Description")
                    .HasColumnType("nvarchar(max)");

                b.Property<string>("Name")
                    .IsRequired()
                    .HasColumnType("nvarchar(max)");

                b.HasKey("GenreID");

                b.ToTable("Genre");
            });

            modelBuilder.Entity("ProjetoCore.API.Models.Movie", b =>
            {
                b.Property<int>("ID")
                    .ValueGeneratedOnAdd()
                    .HasColumnType("int")
                    .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                b.Property<string>("Director")
                    .HasMaxLength(60)
                    .HasColumnType("nvarchar(60)");

                b.Property<int>("GenreID")
                    .HasColumnType("int");

                b.Property<decimal>("Gross")
                    .HasColumnType("decimal(18,2)");

                b.Property<byte[]>("ImageFile")
                    .HasColumnType("varbinary(max)");

                b.Property<string>("ImageMimeType")
                    .HasColumnType("nvarchar(max)");

                b.Property<string>("ImageUrl")
                    .HasColumnType("nvarchar(max)");

                b.Property<double>("Rating")
                    .HasColumnType("float");

                b.Property<DateTime>("ReleaseDate")
                    .HasColumnType("datetime2");

                b.Property<string>("Title")
                    .IsRequired()
                    .HasMaxLength(60)
                    .HasColumnType("nvarchar(60)");

                b.HasKey("ID");

                b.HasIndex("GenreID");

                b.ToTable("Movie");
            });

            modelBuilder.Entity("ProjetoCore.API.Models.Movie", b =>
            {
                b.HasOne("ProjetoCore.API.Models.Genre", "Genre")
                    .WithMany("Movies")
                    .HasForeignKey("GenreID")
                    .OnDelete(DeleteBehavior.Cascade)
                    .IsRequired();

                b.Navigation("Genre");
            });

            modelBuilder.Entity("ProjetoCore.API.Models.Genre", b =>
            {
                b.Navigation("Movies");
            });
#pragma warning restore 612, 618
        }
    }
}
