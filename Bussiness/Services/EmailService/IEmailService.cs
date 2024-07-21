using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Internship.Business.Services.EmailService
{
    public interface IEmailService
    {
        Task SendEmail( EmailSendingFormat sendingFormat);
    }
}
