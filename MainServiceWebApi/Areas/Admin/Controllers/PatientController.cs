using HelpersDTO.AppointmentDto.Enums;
using HelpersDTO.Patient;
using HelpersDTO.Patient.DTO;
using MainServiceWebApi.Areas.Admin.Models;
using MassTransit;
using MassTransit.Transports;
using Microsoft.AspNetCore.Mvc;

namespace MainServiceWebApi.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class PatientController(
        ILogger<AppointmentController> logger,
        IRequestClient<CreateNewPassportRequest> client) : Controller
    {
        private readonly ILogger<AppointmentController> _logger = logger;
        private readonly IRequestClient<CreateNewPassportRequest> _client = client;


        public IActionResult CreateNewContract(Guid? id = null, Guid? userId = null)
        {
            return View(new CheckPatientViewModel() { UserId = userId, AppointmentId = id });
        }

        [HttpPost]
        public async Task<IActionResult> CreateNewContract(CheckPatientViewModel model)
        {
            if (ModelState.IsValid && model.Passport != null)
            {
                try
                {
                    var data = new CreateNewPassportRequest()
                    {
                        Guid = Guid.NewGuid(),
                        ConnectionId = Guid.NewGuid().ToString(),
                        Passport = new PassportDTO()
                        {
                            IssueDate = model.Passport?.IssueDate,
                            Number = model.Passport?.Number,
                            IssuedBy = model.Passport?.IssuedBy,
                            Series = model.Passport?.Series,
                            SubdivisionCode = model.Passport?.SubdivisionCode
                        },
                        UserId = model.UserId
                    };
                    var responce = await _client.GetResponse<CreateNewPassportDtoResponse>(data);

                    _logger.LogInformation("Получен ответ CreateNewPassportDtoResponse {responce}", responce.Message);
                }
                catch (Exception e)
                {
                    _logger.LogError(e, "Произошла ошибка CreateNewPassportRequest");
                }
            }
            return View(model);
        }
    }
}
