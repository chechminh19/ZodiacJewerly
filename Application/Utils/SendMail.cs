using MailKit.Security;
using MailKit.Net.Smtp;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Utils
{
    public class SendMail
    {
        private static string GenerateRandomCode()
        {
            //Random 6 characs 
            string chars = "0123456789";
            Random random = new Random();
            string code = new string(Enumerable.Repeat(chars, 6).Select(s => s[random.Next(s.Length)]).ToArray());
            // Add expiration time (2 minutes) to the code
            DateTime expirationTime = DateTime.UtcNow.AddMinutes(2); // Set expiration time to 2 minutes from now
            string expirationTimeString = expirationTime.ToString("yyyyMMddHHmmss");
            code = expirationTimeString + code;
            return code;
        }
        public static async Task<bool> SendResetPass(string toEmail)
        {
            var userName = "ZodiacJewelry";
            var emailFrom = "minhpcse172904@fpt.edu.vn";
            var password = "lwfr bgex dipf iijf";

            var subjet = "Reset Password Confirmation";
            var code = GenerateRandomCode();
            var body = $"Please enter this code to reset your password: {code}";

            var message = new MimeMessage();
            message.From.Add(new MailboxAddress(userName, emailFrom));
            message.To.Add(new MailboxAddress("", toEmail));
            message.Subject = subjet;
            message.Body = new TextPart("html")
            {
                Text = body   
            };
            using (var client = new SmtpClient())
            {
                client.Connect("smtp.gmail.com", 587, SecureSocketOptions.StartTls);
                //authenticate account email
                client.Authenticate(emailFrom, password);

                try
                {
                    await client.SendAsync(message);
                    await client.DisconnectAsync(true);
                    return true;
                }
                catch (Exception ex)
                {
                    System.Console.WriteLine(ex.Message);
                    return false;
                }
            }
        }
        public static async Task<bool> SendConfirmationEmail(
            string toEmail,
            string confirmationLink
        )
        {
            var userName = "ZodiacJewerly";
            var emailFrom = "minhpcse172904@fpt.edu.vn";
            var password = "lwfr bgex dipf iijf";

            var message = new MimeMessage();
            message.From.Add(new MailboxAddress(userName, emailFrom));
            message.To.Add(new MailboxAddress("", toEmail));
            message.Subject = "Confirmation your email to login";       
            message.Body = new TextPart("html")
            {
                Text =
                    @"
        <html>
            <head>
                <style>
                    body {
                        display: flex;
                        justify-content: center;
                        align-items: center;
                        height: 100vh;
                        margin: 0;
                        font-family: Arial, sans-serif;
                    }
                    .content {
                        text-align: center;
                    }
                    .button {
                        display: inline-block;
                        padding: 10px 20px;
                        background-color: #000;
                        color: #ffffff;
                        text-decoration: none;
                        border-radius: 5px;
                        font-size: 16px;
                    }
                </style>
            </head>
            <body>
                <div class='content'>
                    <p>Please click the button below to confirm your email:</p>                    
                      <a class='button' href='"
                    + confirmationLink
                    + "'>Confirm Email</a>"
                    + @"
                </div>
            </body>
        </html>
    "
            };
            using (var client = new SmtpClient())
            {
                client.Connect("smtp.gmail.com", 587, SecureSocketOptions.StartTls);
                //authenticate account email
                client.Authenticate(emailFrom, password);

                try
                {
                    await client.SendAsync(message);
                    await client.DisconnectAsync(true);
                    return true;
                }
                catch (Exception ex)
                {
                    System.Console.WriteLine(ex.Message);
                    return false;
                }
            }
        }
        //<a class='button' href='"
        //            + confirmationLink
        //            + "'>Confirm Email</a>"
        //            + @"
    }
}
