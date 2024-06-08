using HelpersDTO.AppointmentDto.Enums;
using HelpersDTO.Patient;
using HelpersDTO.Patient.DTO;
using MainServiceWebApi.Areas.Admin.Models;
using MassTransit;
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


        public async Task<IActionResult> CreateNewContractAsync(Guid id, Guid userId)
        {
            return await Task.Run(() => View(new CheckPatientViewModel(userId, id)));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateNewContractAsync(CheckPatientViewModel model)
        {
            if (ModelState.IsValid && model.Passport != null)
            {
                try
                {
                    var responce = await _client.GetResponse<CreateNewPassportDtoResponse>(new CreateNewPassportRequest()
                    {
                        Guid = Guid.NewGuid(),
                        Passport = new PassportDTO()
                        {
                            IssueDate = model.Passport?.IssueDate,
                            Number = model.Passport?.Number,
                            IssuedBy = model.Passport?.IssuedBy,
                            Series = model.Passport?.Series,
                            SubdivisionCode = model.Passport?.SubdivisionCode
                        },
                        UserId = model.UserId
                    });

                    _logger.LogInformation("Получен ответ CreateNewPassportDtoResponse {responce}", responce.Message);

                    if (responce.Message.Success && string.IsNullOrEmpty(responce.Message.Message))
                    {
                        return RedirectToAction("UpdateStatus", new { id = model.AppointmentId, status = (int)StatusEnum.Waiting });
                    }
                    else
                    {
                        ModelState.AddModelError("", responce.Message.Message ?? "При попытке сохранить данные произошла ошибка. Попробуйте позднее");
                    }
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
