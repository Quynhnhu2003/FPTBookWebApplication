using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace FPTBookWeb.Models;

public partial class DbFptbookContext : DbContext
{
    public DbFptbookContext()
    {
    }

    public DbFptbookContext(DbContextOptions<DbFptbookContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Author> Authors { get; set; }

    public virtual DbSet<Book> Books { get; set; }

    public virtual DbSet<Cart> Carts { get; set; }

    public virtual DbSet<CartItem> CartItems { get; set; }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<OrderItem> OrderItems { get; set; }

    public virtual DbSet<Publisher> Publishers { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=LAPTOP-M2M74TDG\\SQLEXPRESS; Database=DbFPTBook; Trusted_Connection=True; TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Author>(entity =>
        {
            entity.HasKey(e => e.AuthorId).HasName("PK__Author__8E2731B95227E36E");

            entity.ToTable("Author");

            entity.Property(e => e.AuthorId).HasColumnName("authorId");
            entity.Property(e => e.AuthorDescription)
                .HasMaxLength(200)
                .HasColumnName("authorDescription");
            entity.Property(e => e.AuthorName)
                .HasMaxLength(150)
                .HasColumnName("authorName");
        });

        modelBuilder.Entity<Book>(entity =>
        {
            entity.HasKey(e => e.BookId).HasName("PK__Book__8BE5A10D18DEB8B8");

            entity.ToTable("Book");

            entity.HasIndex(e => e.BookTitle, "UQ__Book__79179C7496E70C21").IsUnique();

            entity.Property(e => e.BookId).HasColumnName("bookId");
            entity.Property(e => e.AuthorId).HasColumnName("authorId");
            entity.Property(e => e.BookDescription)
                .HasMaxLength(200)
                .HasColumnName("bookDescription");
            entity.Property(e => e.BookImage1)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("bookImage1");
            entity.Property(e => e.BookImage2)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("bookImage2");
            entity.Property(e => e.BookImage3)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("bookImage3");
            entity.Property(e => e.BookPages).HasColumnName("bookPages");
            entity.Property(e => e.BookPrice).HasColumnName("bookPrice");
            entity.Property(e => e.BookTitle)
                .HasMaxLength(150)
                .HasColumnName("bookTitle");
            entity.Property(e => e.CategoryId).HasColumnName("categoryId");
            entity.Property(e => e.PublishedYear).HasColumnName("publishedYear");
            entity.Property(e => e.PublisherId).HasColumnName("publisherId");
            entity.Property(e => e.UserId).HasColumnName("userId");

            entity.HasOne(d => d.Author).WithMany(p => p.Books)
                .HasForeignKey(d => d.AuthorId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_authorId");

            entity.HasOne(d => d.Category).WithMany(p => p.Books)
                .HasForeignKey(d => d.CategoryId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_categoryId");

            entity.HasOne(d => d.Publisher).WithMany(p => p.Books)
                .HasForeignKey(d => d.PublisherId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_publisherId");

            entity.HasOne(d => d.User).WithMany(p => p.Books)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_userId");
        });

        modelBuilder.Entity<Cart>(entity =>
        {
            entity.HasKey(e => e.CartId).HasName("PK__Cart__415B03B88F84BCEC");

            entity.ToTable("Cart");

            entity.Property(e => e.CartId)
                .HasMaxLength(10)
                .IsFixedLength()
                .HasColumnName("cartId");
        });

        modelBuilder.Entity<CartItem>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__CartItem__3214EC07B99AA8F9");

            entity.ToTable("CartItem");

            entity.Property(e => e.BookId).HasColumnName("bookId");
            entity.Property(e => e.CartId)
                .HasMaxLength(10)
                .IsFixedLength()
                .HasColumnName("cartId");

            entity.HasOne(d => d.Book).WithMany(p => p.CartItems)
                .HasForeignKey(d => d.BookId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_bookId");

            entity.HasOne(d => d.Cart).WithMany(p => p.CartItems)
                .HasForeignKey(d => d.CartId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_cartId");
        });

        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.CategoryId).HasName("PK__Category__23CAF1D89F0EA6BD");

            entity.ToTable("Category");

            entity.HasIndex(e => e.CategoryName, "UQ__Category__37077ABD007AE374").IsUnique();

            entity.Property(e => e.CategoryId).HasColumnName("categoryId");
            entity.Property(e => e.CategoryName)
                .HasMaxLength(50)
                .HasColumnName("categoryName");
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(e => e.OrderId).HasName("PK__Orders__0809335D2413CF33");

            entity.Property(e => e.OrderId).HasColumnName("orderId");
            entity.Property(e => e.OrderDate)
                .HasColumnType("datetime")
                .HasColumnName("orderDate");
            entity.Property(e => e.OrderTotal).HasColumnName("orderTotal");
        });

        modelBuilder.Entity<OrderItem>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__OrderIte__3214EC07C3B643EE");

            entity.ToTable("OrderItem");

            entity.Property(e => e.BookId).HasColumnName("bookId");
            entity.Property(e => e.OrderId).HasColumnName("orderId");

            entity.HasOne(d => d.Book).WithMany(p => p.OrderItems)
                .HasForeignKey(d => d.BookId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_bookId2");

            entity.HasOne(d => d.Order).WithMany(p => p.OrderItems)
                .HasForeignKey(d => d.OrderId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_orderId");
        });

        modelBuilder.Entity<Publisher>(entity =>
        {
            entity.HasKey(e => e.PublisherId).HasName("PK__Publishe__7E8A0D9662EF9301");

            entity.ToTable("Publisher");

            entity.Property(e => e.PublisherId).HasColumnName("publisherId");
            entity.Property(e => e.PublisherAddress)
                .HasMaxLength(100)
                .HasColumnName("publisherAddress");
            entity.Property(e => e.PublisherEmail)
                .HasMaxLength(100)
                .HasColumnName("publisherEmail");
            entity.Property(e => e.PublisherName)
                .HasMaxLength(150)
                .HasColumnName("publisherName");
            entity.Property(e => e.PublisherPhone)
                .HasMaxLength(15)
                .IsFixedLength()
                .HasColumnName("publisherPhone");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.RoleId).HasName("PK__Roles__CD98462ADE13ABE6");

            entity.HasIndex(e => e.RoleName, "UQ__Roles__B1947861D7F72AD5").IsUnique();

            entity.Property(e => e.RoleId).HasColumnName("roleId");
            entity.Property(e => e.RoleName)
                .HasMaxLength(20)
                .HasColumnName("roleName");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__Users__CB9A1CFFD0FC02AC");

            entity.HasIndex(e => e.UserName, "UQ__Users__66DCF95C32F12FD6").IsUnique();

            entity.HasIndex(e => e.UserAddress, "UQ__Users__B205C844303C065F").IsUnique();

            entity.HasIndex(e => e.UserEmail, "UQ__Users__D54ADF5546E98420").IsUnique();

            entity.HasIndex(e => e.UserPhone, "UQ__Users__E19C9691B398E599").IsUnique();

            entity.Property(e => e.UserId).HasColumnName("userId");
            entity.Property(e => e.RoleId).HasColumnName("roleId");
            entity.Property(e => e.UserAddress)
                .HasMaxLength(150)
                .HasColumnName("userAddress");
            entity.Property(e => e.UserEmail)
                .HasMaxLength(100)
                .HasColumnName("userEmail");
            entity.Property(e => e.UserName)
                .HasMaxLength(150)
                .HasColumnName("userName");
            entity.Property(e => e.UserPass)
                .HasMaxLength(20)
                .HasColumnName("userPass");
            entity.Property(e => e.UserPhone)
                .HasMaxLength(15)
                .IsFixedLength()
                .HasColumnName("userPhone");

            entity.HasOne(d => d.Role).WithMany(p => p.Users)
                .HasForeignKey(d => d.RoleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_roleId");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
