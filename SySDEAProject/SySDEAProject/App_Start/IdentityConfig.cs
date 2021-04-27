using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using SySDEAProject.Models;
using System.Net.Mail;
using System.Net;

namespace SySDEAProject
{

    public class EmailService : IIdentityMessageService
    {
        SySDEAContext db = new SySDEAContext(); 
        public Task SendAsync(IdentityMessage message)
        {
            MailAddress destinationAddress = new MailAddress(message.Destination);
            var smtp = new SmtpClient();

            var credential = new NetworkCredential
            {
                UserName = "",  // replace with valid value
                Password = ""  // replace with valid value
            };
            smtp.Credentials = credential;
            smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
            EmailSySDEA emailSySDEA = new EmailSySDEA(message);
            emailSySDEA.tipoOrigem = "Mensagem de identidade";
            db.EmailSySDEA.Add(emailSySDEA);
            db.SaveChanges();
            return smtp.SendMailAsync("", destinationAddress.ToString(), message.Subject, message.Body);
        }
    }

    public class SmsService : IIdentityMessageService
    {
        public Task SendAsync(IdentityMessage message)
        {
            // Plug in your SMS service here to send a text message.
            return Task.FromResult(0);
        }
    }

    // Configure the application user manager used in this application. UserManager is defined in ASP.NET Identity and is used by the application.
    public class ApplicationUserManager : UserManager<Usuario, int>
    {
        public ApplicationUserManager(IUserStore<Usuario, int> store)
            : base(store)
        {
        }

        public static ApplicationUserManager Create(
            IdentityFactoryOptions<ApplicationUserManager> options, IOwinContext context)
        {
            var manager = new ApplicationUserManager(
                new CustomUserStore(context.Get<SySDEAContext>()));
            // Configure validation logic for usernames 
            manager.UserValidator = new UserValidator<Usuario, int>(manager)
            {
                AllowOnlyAlphanumericUserNames = false,
                RequireUniqueEmail = true
            };
            // Configure validation logic for passwords 
            manager.PasswordValidator = new PasswordValidator
            {
                RequiredLength = 6,
                RequireNonLetterOrDigit = false,
                RequireDigit = false,
                RequireLowercase = false,
                RequireUppercase = false,
            };
            // Register two factor authentication providers. This application uses Phone 
            // and Emails as a step of receiving a code for verifying the user 
            // You can write your own provider and plug in here. 
            manager.RegisterTwoFactorProvider("PhoneCode",
                new PhoneNumberTokenProvider<Usuario, int>
                {
                    MessageFormat = "Your security code is: {0}"
                });
            manager.RegisterTwoFactorProvider("EmailCode",
                new EmailTokenProvider<Usuario, int>
                {
                    Subject = "Security Code",
                    BodyFormat = "Your security code is: {0}"
                });
            manager.EmailService = new EmailService();
            manager.SmsService = new SmsService();
            var dataProtectionProvider = options.DataProtectionProvider;
            if (dataProtectionProvider != null)
            {
                manager.UserTokenProvider =
                    new DataProtectorTokenProvider<Usuario, int>(
                        dataProtectionProvider.Create("ASP.NET Identity"));
            }
            return manager;
        }
    }




    // Configure the application sign-in manager which is used in this application.
    public class ApplicationSignInManager : SignInManager<Usuario, int>
    {
        public ApplicationSignInManager(ApplicationUserManager userManager, IAuthenticationManager authenticationManager)
            : base(userManager, authenticationManager)
        {
        }

        public override Task<ClaimsIdentity> CreateUserIdentityAsync(Usuario user)
        {
            return user.GenerateUserIdentityAsync((ApplicationUserManager)UserManager);
        }

        public static ApplicationSignInManager Create(IdentityFactoryOptions<ApplicationSignInManager> options, IOwinContext context)
        {
            return new ApplicationSignInManager(context.GetUserManager<ApplicationUserManager>(), context.Authentication);
        }
    }

}