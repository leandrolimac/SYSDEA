using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.ComponentModel.DataAnnotations.Schema;
using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;

namespace SySDEAProject.Models
{
    // You can add profile data for the user by adding more properties to your User class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    /*[Table("EntidadeLogin")]    
    public partial class EntidadeLogin : Usuario
    {
        public EntidadeLogin()
        {
            EmailConfirmed = false;
            PhoneNumberConfirmed = false;
            AccessFailedCount = 0;
            //Entidade = new Entidade();
        }
        
        public virtual Entidade Entidade { get; set; }
    }*/


    [Table("USUARIOS")]

    public partial class Usuario : IdentityUser<int, CustomUserLogin, CustomUserRole, CustomUserClaim>
    {
        [Column(name: "ID_USUARIO")]
        public override int Id { get; set; }
        [Column(name: "TX_EMAIL_USUARIO")]
        public override string Email { get; set; }
        [Column(name: "SN_EMAIL_CONFIRMADO_USUARIO")]
        public override bool EmailConfirmed { get; set; }
        [Column(name: "TX_SENHA_USUARIO")]
        public override string PasswordHash { get; set; }
        [Column(name: "TX_STAMP_USUARIO")]
        public override string SecurityStamp { get; set; }
        [Column(name: "NR_TELEFONE_USUARIO")]
        public override string PhoneNumber { get; set; }
        [Column(name: "SN_TELEFONE_CONFIRMADO_USUARIO")]
        public override bool PhoneNumberConfirmed { get; set; }
        [Column(name: "SN_DOIS_FATORES_ATIVO_USUARIO")]
        public override bool TwoFactorEnabled { get; set; }
        [Column(name: "DT_FIM_LOCKOUT_USUARIO")]
        public override DateTime? LockoutEndDateUtc { get; set; }
        [Column(name: "SN_LOCKOUT_ATIVO_USUARIO")]
        public override bool LockoutEnabled { get; set; }
        [Column(name: "QT_FALHAS_ACESSO_USUARIO")]
        public override int AccessFailedCount { get; set; }
        [Column(name: "NM_USUARIO")]
        public override string UserName { get; set; }
    

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Usuario()
        {

        }
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<Usuario, int> manager)
        
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            
            // Add custom user claims here
            return userIdentity;
        }
               
        
       /* [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Discriminator> Discriminator
        {   get;
            set;
        }*/
        //public ICollection<CustomRole> Roles2 { get; set; }
    }


    public partial class CustomUserRole : IdentityUserRole<int>
    {


        
        
    }
    public partial class CustomUserClaim : IdentityUserClaim<int> { }
    public partial class CustomUserLogin : IdentityUserLogin<int> { }
    public class CustomUserStore : UserStore<Usuario, CustomRole, int,
        CustomUserLogin, CustomUserRole, CustomUserClaim>
    {
       
        public CustomUserStore(SySDEAContext context)
            : base(context)
        {
        }
    }

    public class CustomRoleStore : RoleStore<CustomRole, int, CustomUserRole>
    {
        public CustomRoleStore(SySDEAContext context)
            : base(context)
        {
        }
    }
    public class CustomRoleManager : RoleManager<CustomRole, int>
    {
        public CustomRoleManager(IRoleStore<CustomRole, int> roleStore)
            : base(roleStore)
        {
        }

        public static CustomRoleManager Create(IdentityFactoryOptions<CustomRoleManager> options, IOwinContext context)
        {
            return new CustomRoleManager(new RoleStore<CustomRole, int, CustomUserRole>(context.Get<SySDEAContext>()));
        }
    }

}