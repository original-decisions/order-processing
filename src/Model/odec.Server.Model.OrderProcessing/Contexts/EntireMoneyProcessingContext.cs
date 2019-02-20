using System.Linq;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using odec.Server.Model.Contact;
using odec.Server.Model.OrderProcessing.Money;
using odec.Server.Model.OrderProcessing.Money.Withdrawal;
using odec.Server.Model.User;

namespace odec.Server.Model.OrderProcessing.Contexts
{
    public class EntireMoneyProcessingContext : DbContext
    //IdentityDbContext<User.User, Role, int, UserClaim, UserRole, UserLogin, IdentityRoleClaim<int>, UserToken>

    {
        private string MembershipScheme = "AspNet";
        private string ECashScheme = "ecash";
        private string OrderScheme = "order";
        private string UserScheme = "users";

        public EntireMoneyProcessingContext(DbContextOptions<EntireMoneyProcessingContext> options)
            : base(options)
        {

        }
        //TODO: перевести на англ
        /// <summary>
        /// Учетка пользователя для внутренней валюты. Подразумевается, что будет всего 1 валюта на сайте
        /// </summary>
        public DbSet<UserMAccount> UsersAccount { get; set; }
        /// <summary>
        /// История операций с внутренней валютой. Зачисление, блокировка, снятие, и т. д.
        /// </summary>
        public DbSet<AccountOperation> AccountOperationHistory { get; set; }
        /// <summary>
        /// Типы операций. Перевод, блокировка, зачисление и т д.
        /// </summary>
        public DbSet<OperationType> OperationTypes { get; set; }
        /// <summary>
        /// Заявки на вывод средств.
        /// </summary>
        public DbSet<WithdrawalApplication> WithdrawalApplications { get; set; }
        /// <summary>
        /// Способы вывода средств.
        /// </summary>
        public DbSet<WithdrawalMethod> WithdrawalMethods { get; set; }
        //todo: change migrations
        public DbSet<UserContact> UserContacts { get; set; }
        public DbSet<DeliveryMethod> DeliveryMethods { get; set; }
        public DbSet<DeliveryZone> DeliveryZones { get; set; }
        public DbSet<DeliveryCharge> DeliveryCharges { get; set; }
        public DbSet<PaymentMethod> PaymentMethods { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }
        public DbSet<OrderState> OrderStates { get; set; }
        public DbSet<OrderOrderType> OrderTypes { get; set; }
        public DbSet<OrderType> Types { get; set; }
        public DbSet<Contact.Contact> Contacts { get; set; }

        public DbSet<Sex> Sexes { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //modelBuilder.Conventions.Remove<ManyToManyCascadeDeleteConvention>();
            //modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
            modelBuilder.Entity<User.User>().ToTable("Users", MembershipScheme);
            modelBuilder.Entity<Role>().ToTable("Roles", MembershipScheme);
            // modelBuilder.Entity<UserRole>().ToTable("UserRoles", MembershipScheme);
            //modelBuilder.Entity<UserClaim>().ToTable("UserClaims", MembershipScheme);
            //modelBuilder.Entity<UserLogin>().ToTable("UserLogins", MembershipScheme);
            //modelBuilder.Entity<RoleClaim>().ToTable("RoleClaims", MembershipScheme);
            //modelBuilder.Entity<UserToken>().ToTable("UserTokens", MembershipScheme);

            modelBuilder.Entity<WithdrawalMethod>().ToTable("WithdrawalMethods", ECashScheme);
            modelBuilder.Entity<WithdrawalApplication>().ToTable("WithdrawalApplications", ECashScheme);
            modelBuilder.Entity<UserMAccount>().ToTable("UsersAccount", ECashScheme);
            modelBuilder.Entity<AccountOperation>()
                .ToTable("AccountOperationHistory", ECashScheme)
                .HasKey(it => new { it.OperationDate, it.UserId });
            modelBuilder.Entity<OperationType>()
                .ToTable("OperationTypes", ECashScheme);
            modelBuilder.Entity<Contact.Contact>()
                .ToTable("Contacts", UserScheme);
            modelBuilder.Entity<UserContact>()
                .ToTable("UserContacts", UserScheme)
                .HasKey(it => new { it.UserId, it.ContactId });
            modelBuilder.Entity<Sex>()
                .ToTable("Sexes", UserScheme);

            modelBuilder.Entity<Order>()
                .ToTable("Orders", OrderScheme);
            modelBuilder.Entity<OrderDetail>()
                .ToTable("OrderDetails", OrderScheme);
            modelBuilder.Entity<DeliveryMethod>()
                .ToTable("DeliveryMethods", OrderScheme);
            modelBuilder.Entity<DeliveryCharge>()
                .ToTable("DeliveryCharges", OrderScheme)
                .HasKey(it => new { it.DeliveryMethodId, it.ZoneId });
            modelBuilder.Entity<DeliveryZone>()
                .ToTable("DeliveryZones", OrderScheme);
            modelBuilder.Entity<PaymentMethod>()
                .ToTable("PaymentMethods", OrderScheme);
            modelBuilder.Entity<OrderState>()
                .ToTable("OrderStates", OrderScheme);
            modelBuilder.Entity<OrderType>()
                .ToTable("Types", OrderScheme);
            modelBuilder.Entity<OrderOrderType>()
                .ToTable("OrderTypes", OrderScheme)
                .HasKey(it => new { it.OrderId, it.OrderTypeId });
            foreach (var relationship in modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
                relationship.DeleteBehavior = DeleteBehavior.Restrict;
            base.OnModelCreating(modelBuilder);
        }
    }
}
