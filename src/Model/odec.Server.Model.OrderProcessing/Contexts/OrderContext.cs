using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using odec.Server.Model.Contact;
using odec.Server.Model.OrderProcessing.Abstractions.Interfaces;
using odec.Server.Model.User;

namespace odec.Server.Model.OrderProcessing.Contexts
{
    public class OrderContext: DbContext,
        //IdentityDbContext<User.User, Role, int, UserClaim, UserRole, UserLogin, IdentityRoleClaim<int>, UserToken>, 
        IOrderContext<Order, OrderDetail, DeliveryMethod,PaymentMethod,OrderState>
    {
        private string MembershipScheme = "AspNet";
        private string OrderScheme = "order";
        private string UserScheme = "users";
        public OrderContext(DbContextOptions<OrderContext> options)
            : base(options)
        {

        }

        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }
        public DbSet<DeliveryMethod> DeliveryMethods { get; set; }
        public DbSet<DeliveryCharge> DeliveryCharges { get; set; }
        public DbSet<DeliveryZone> DeliveryZones { get; set; }

        public DbSet<PaymentMethod> PaymentMethods { get; set; }
        public DbSet<OrderState> OrderStates { get; set; }
        public DbSet<OrderOrderType> OrderTypes { get; set; }
        public DbSet<Contact.Contact> Contacts { get; set; }
        public DbSet<UserContact> UserContacts { get; set; }

        public DbSet<Sex> Sexes { get; set; }
        public DbSet<OrderType> Types { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //modelBuilder.Conventions.Remove<ManyToManyCascadeDeleteConvention>();
            //modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
            modelBuilder.Entity<User.User>()
                .ToTable("Users", MembershipScheme);
            modelBuilder.Entity<Role>()
                .ToTable("Roles", MembershipScheme);
           // modelBuilder.Entity<UserRole>().ToTable("UserRoles", MembershipScheme);
            //modelBuilder.Entity<UserClaim>().ToTable("UserClaims", MembershipScheme);
            //modelBuilder.Entity<UserLogin>().ToTable("UserLogins", MembershipScheme);
            //modelBuilder.Entity<RoleClaim>().ToTable("RoleClaims", MembershipScheme);
            //modelBuilder.Entity<UserToken>().ToTable("UserTokens", MembershipScheme);
            modelBuilder.Entity<Contact.Contact>()
                .ToTable("Contacts", UserScheme);
            modelBuilder.Entity<UserContact>()
                .ToTable("UserContacts", UserScheme)
                .HasKey(it => new { it.UserId, it.ContactId });
            modelBuilder.Entity<Sex>()
                .ToTable("Sexes", UserScheme);
            modelBuilder.Entity<Order>()
                .ToTable("Orders", OrderScheme);
            modelBuilder.Entity<OrderType>()
                .ToTable("Types", OrderScheme);
            modelBuilder.Entity<DeliveryCharge>()
                .ToTable("DeliveryCharges", OrderScheme)
                .HasKey(it => new { it.DeliveryMethodId, it.ZoneId });
            modelBuilder.Entity<DeliveryZone>()
                .ToTable("DeliveryZones", OrderScheme);
            modelBuilder.Entity<OrderOrderType>()
                .ToTable("OrderTypes", OrderScheme)
                .HasKey(it => new { it.OrderId, it.OrderTypeId });
            modelBuilder.Entity<OrderDetail>()
                .ToTable("OrderDetails", OrderScheme);
            modelBuilder.Entity<DeliveryMethod>()
                .ToTable("DeliveryMethods", OrderScheme);
            modelBuilder.Entity<PaymentMethod>()
                .ToTable("PaymentMethods", OrderScheme);
            modelBuilder.Entity<OrderState>()
                .ToTable("OrderStates", OrderScheme);
            foreach (var relationship in modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
                relationship.DeleteBehavior = DeleteBehavior.Restrict;
            base.OnModelCreating(modelBuilder);
        }
    }
}
