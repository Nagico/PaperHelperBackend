using Microsoft.AspNetCore.Diagnostics;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PaperHelper.Exceptions;

namespace PaperHelper.Extensions;

public static class ExceptionHandlingExtensions
{
    private static JObject? _error = null;

    private static void LoadError()
    {
        if (_error != null) return;
        var file = Path.Combine(AppContext.BaseDirectory, "error.json");
        if (File.Exists(file))
        {
            _error = JObject.Parse(File.ReadAllText(file));
        }
    }
    
    public static void UseApiExceptionHandler(this IApplicationBuilder app)
    {
        app.UseExceptionHandler(builder => {

            builder.Run(async context =>
            {
                // lazy加载错误信息至内存
                LoadError();
                
                // 返回的异常信息
                var resultError = new AppError("B0000", "未知错误");
                
                // 捕获的异常
                var ex = context.Features.Get<IExceptionHandlerFeature>();

                if (ex != null)
                {
                    var exception = ex.Error;
                    
                    // 分类处理异常
                    resultError = exception switch
                    {
                        AppError error => error,
                        _ => new AppError("B0000", exception.Message)
                    };
                }
                
                // 默认缺省的异常信息
                var defaultError = _error?.GetValue(resultError.Code) ?? _error?.GetValue("B0000");
                
                resultError.Message ??= defaultError?[0]?.ToString();
                var resultJson = JsonConvert.SerializeObject(resultError);
                
                context.Response.StatusCode = defaultError?[1]?.ToObject<int>() ?? 500;
                context.Response.ContentType = "application/json";
                await context.Response.WriteAsync(resultJson.ToString());
            });           
        });
    }
}