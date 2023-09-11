namespace GlobalizationDemo.Swagger.Filters;

internal class AcceptLanguageHeaderFilter : IOperationFilter
{
    public void Apply(
        OpenApiOperation operation,
        OperationFilterContext context)
    {
        var acceptLanguageParameter = new OpenApiParameter
        {
            Name = HeaderNames.AcceptLanguage,
            In = ParameterLocation.Header,
            Description = "Inform the expected language",
            Example = new OpenApiString("en-US"),
            Required = false,
            Schema = new OpenApiSchema
            {
                Title = "Accept-Language",
                Type = "string",
                Format = "language-tag",
            },
        };

        operation.Parameters.Add(acceptLanguageParameter);
    }
}