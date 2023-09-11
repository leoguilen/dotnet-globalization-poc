namespace GlobalizationDemo.Localizations;

internal sealed class ExtendedStringLocalizerFactory : IStringLocalizerFactory
{
    public const string DatabaseLocation = "Database";

    private readonly IDbContext _dbContext;
    private readonly Lazy<ResourceManagerStringLocalizerFactory> _resourceManagerStringLocalizerFactory;

    public ExtendedStringLocalizerFactory(
        IOptions<LocalizationOptions> localizationOptions,
        ILoggerFactory loggerFactory,
        IDbContext dbContext)
    {
        _dbContext = dbContext;
        _resourceManagerStringLocalizerFactory = new(() => new ResourceManagerStringLocalizerFactory(localizationOptions, loggerFactory));
    }

    public IStringLocalizer Create(
        string baseName,
        string location)
        => DatabaseLocation.Equals(location, StringComparison.OrdinalIgnoreCase)
            ? new SqlStringLocalizer(baseName, _dbContext)
            : _resourceManagerStringLocalizerFactory.Value.Create(baseName, location);

    public IStringLocalizer Create(Type resourceSource)
        => _resourceManagerStringLocalizerFactory.Value.Create(resourceSource);
}
