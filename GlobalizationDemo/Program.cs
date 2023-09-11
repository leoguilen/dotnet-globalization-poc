var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(opt => opt.OperationFilter<AcceptLanguageHeaderFilter>());
builder.Services.AddLocalization(opt => opt.ResourcesPath = "Resources");
builder.Services.Configure<RequestLocalizationOptions>(opt =>
{
    var localizationOptions = builder.Configuration.GetRequiredSection("LocalizationOptions");

    var defaultCulture = localizationOptions.GetValue<string>("DefaultCulture");
    var supportedCultures = localizationOptions.GetSection("SupportedCultures").Get<string[]>();

    _ = opt
        .SetDefaultCulture(defaultCulture!)
        .AddSupportedCultures(supportedCultures!)
        .AddSupportedUICultures(supportedCultures!);

    opt.ApplyCurrentCultureToResponseHeaders = true;
    opt.RequestCultureProviders.Clear();
    opt.RequestCultureProviders.Add(new AcceptLanguageHeaderRequestCultureProvider());
});
builder.Services.AddSingleton<IDbContext, DbContext>();
builder.Services.AddSingleton<IStringLocalizerFactory, ExtendedStringLocalizerFactory>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseRequestLocalization(app.Services.GetRequiredService<IOptions<RequestLocalizationOptions>>().Value);

app.Map("/info", ([FromServices] IStringLocalizerFactory localizerFactory) =>
{
    var culture = CultureInfo.CurrentCulture;
    //var localizer = localizerFactory.Create("Messages", "GlobalizationDemo"); Busca nos arquivos de recurso
    var localizer = localizerFactory.Create("translations", ExtendedStringLocalizerFactory.DatabaseLocation); // Busca no banco de dados

    return Results.Ok(new
    {
        Culture = culture.DisplayName,
        Message = localizer.GetString("GreetingMessage").Value,
        Currency = culture.NumberFormat.CurrencySymbol,
        DateTime = DateTimeOffset.UtcNow.ToString($"{culture.DateTimeFormat.ShortDatePattern} {culture.DateTimeFormat.LongTimePattern}"),
    });
});

app.Run();
