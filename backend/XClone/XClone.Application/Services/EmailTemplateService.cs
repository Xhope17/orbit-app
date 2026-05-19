using XClone.Application.Interfaces.Services;
using XClone.Application.Models.DTOs;
using XClone.Application.Models.Services;
using XClone.Domain.DataBase.SqlServer;

namespace XClone.Application.Services
{
    //public class EmailTemplateService(EmailTemplateData data, IEmailTemplateRepository repository) : IEmailTemplateService
    public class EmailTemplateService(EmailTemplateData data, IUnitOfWork uow) : IEmailTemplateService

    {
        public async Task<EmailTemplateDto> Get(string name, Dictionary<string, string> variables)
        {
            var template = data.Data.First(x => x.Name == name);

            foreach (var variable in variables)
            {
                template.Body = template.Body.Replace("{{" + variable.Key + "}}", variable.Value);
            }

            return new EmailTemplateDto
            {
                Body = template.Body,
                Subject = template.Subject,
            };
        }

        public async Task Init()
        {
            //var templates = await repository.Get();
            var templates = await uow.emailTemplateRepository.Get();
            data.Data = templates;
        }
    }
}
