using MailKit.Net.Smtp;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using MimeKit;
using MimeKit.Text;
using System;
using System.Linq;
using System.Threading.Tasks;
using ZedCrest.Api.Data;
using ZedCrest.Api.Models;

namespace ZedCrest.Api.Handler
{
    public class FileSubmittedHandler : IConsumer<IFileSubmitted>
    {
        private readonly ZedCrestContext _dbContext;
        private readonly EmailSettings _settings;

        public FileSubmittedHandler(ZedCrestContext dbContext, EmailSettings settings)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _settings = settings ?? throw new ArgumentNullException(nameof(settings));
        }

        public async Task Consume(ConsumeContext<IFileSubmitted> context)
        {
            var user = await _dbContext.Users.FirstOrDefaultAsync(x => x.Id == context.Message.UserId);

            if (user is null)
                return;

            var files = await _dbContext.ApplicationFiles.Where(x => context.Message.Files.Any(f => f == x.Id)).ToListAsync();

            var email = new MimeMessage();

            email.From.Add(MailboxAddress.Parse(_settings.From));
            email.To.Add(MailboxAddress.Parse(user.Email));
            email.Subject = "Zedscrest:  File Upload Success";
            var body = new BodyBuilder();
            body.TextBody = new TextPart(TextFormat.Html) { Text = $"Dear {user.FirstName} {user.LastName}, \\n Kindly find attached your documents \\n Regards." }.Text;

            foreach (var file in files)
            {
                var fileBytes = await System.IO.File.ReadAllBytesAsync(file.Path);
                body.Attachments.Add(file.Name, fileBytes, ContentType.Parse(ParserOptions.Default, file.ContentType));
            }

            email.Body = body.ToMessageBody();

            using var smtp = new SmtpClient();
            await smtp.ConnectAsync(_settings.SmtpHost, _settings.Port);
            await smtp.AuthenticateAsync(_settings.User, _settings.Pwd);
            await smtp.SendAsync(email);
            await smtp.DisconnectAsync(true);
        }
    }
}

