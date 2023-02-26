using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using SFA.DAS.ApprenticeAan.Domain.Interfaces;

namespace SFA.DAS.ApprenticeAan.Web.Filters;

[ExcludeFromCodeCoverage]
public class RequiredSessionModelAttribute : ActionFilterAttribute
{
    public Type ModelType { get; set; }

    public string ActionName { get; set; }

    public string ControllerName { get; set; }

    public RequiredSessionModelAttribute(Type modelType, string actionName = "Index", string controllerName = "Home")
    {
        ModelType = modelType;
        ActionName = actionName;
        ControllerName = controllerName;
    }

    public override void OnActionExecuting(ActionExecutingContext context)
    {
        var sessionService = context.HttpContext.RequestServices.GetService<ISessionService>();

        var sessionModel = sessionService!.Get(ModelType.Name);

        if (sessionModel == null)
        {
            context.Result = new RedirectToActionResult(ActionName, ControllerName, null);
        }
    }
}
