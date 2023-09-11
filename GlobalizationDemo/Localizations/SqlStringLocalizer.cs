namespace GlobalizationDemo.Localizations;

internal sealed class SqlStringLocalizer : IStringLocalizer
{
    private const string _getTextQuery =
    @"
        SELECT text
        FROM {0}
        WHERE
            key = @key AND
            culture_code IN (@culture, @parentCulture)
        ORDER BY culture_code DESC
        LIMIT 1;
    ";

    private const string _getAllQuery =
    @"
        SELECT key, text
        FROM {0}
        WHERE culture_code LIKE @searchableCulture
    ";

    private readonly string _baseName;
    private readonly IDbContext _dbContext;

    public SqlStringLocalizer(
        string baseName,
        IDbContext dbContext)
    {
        _baseName = baseName;
        _dbContext = dbContext;
    }

    public LocalizedString this[string name]
    {
        get
        {
            var culture = CultureInfo.CurrentCulture;
            var parentCulture = culture.Parent.TwoLetterISOLanguageName;

            var value = _dbContext
                .Connection
                .QuerySingleOrDefaultAsync<string?>(
                    sql: string.Format(_getTextQuery, _baseName),
                    param: new
                    {
                        baseName = _baseName,
                        key = name,
                        culture = culture.Name,
                        parentCulture
                    })
                .GetAwaiter()
                .GetResult();

            return new LocalizedString(name, value ?? name, string.IsNullOrWhiteSpace(value));
        }
    }

    public LocalizedString this[string name, params object[] arguments]
    {
        get
        {
            var localizedString = this[name];
            return new LocalizedString(name, string.Format(localizedString.Value, arguments), localizedString.ResourceNotFound);
        }
    }

    public IEnumerable<LocalizedString> GetAllStrings(bool includeParentCultures)
    {
        var culture = CultureInfo.CurrentCulture;
        var searchableCulture = includeParentCultures ? $"{culture.Parent.TwoLetterISOLanguageName}%" : culture.Name;

        var values = _dbContext
            .Connection
            .QueryAsync<(string Key, string Text)>(
                sql: string.Format(_getAllQuery, _baseName),
                param: new
                {
                    baseName = _baseName,
                    searchableCulture
                })
            .GetAwaiter()
            .GetResult();

        return values.Select(v => new LocalizedString(v.Key, v.Text, string.IsNullOrEmpty(v.Text)));
    }
}
