using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace SkillAssessmentPlatform.API.Helpers
{
    public class ViewRender
    {
        private readonly IRazorViewEngine _viewEngine;
        private readonly ITempDataProvider _tempDataProvider;
        private readonly IServiceProvider _serviceProvider;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ViewRender(IRazorViewEngine viewEngine, ITempDataProvider tempDataProvider, IServiceProvider serviceProvider, IHttpContextAccessor httpContextAccessor)
        {
            _viewEngine = viewEngine;
            _tempDataProvider = tempDataProvider;
            _serviceProvider = serviceProvider;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<string> RenderViewToStringAsync(ActionContext actionContext, string viewName, object model)
        {
            using var scope = _serviceProvider.CreateScope();
            var razorViewEngine = scope.ServiceProvider.GetRequiredService<IRazorViewEngine>();
            var tempDataProvider = scope.ServiceProvider.GetRequiredService<ITempDataProvider>();
            var serviceProvider = scope.ServiceProvider;

            var viewResult = razorViewEngine.FindView(actionContext, viewName, false);
            if (!viewResult.Success)
            {
                throw new ArgumentNullException($"View '{viewName}' not found");
            }

            using var stringWriter = new StringWriter();
            var viewContext = new ViewContext(
                actionContext,
                viewResult.View,
                new ViewDataDictionary(new EmptyModelMetadataProvider(), new ModelStateDictionary())
                {
                    Model = model
                },
                new TempDataDictionary(actionContext.HttpContext, tempDataProvider),
                stringWriter,
                new HtmlHelperOptions()
            );

            await viewResult.View.RenderAsync(viewContext);
            return stringWriter.ToString();
        }
    }
}
