using System.Reflection;
using System.Windows.Input;
using CalendarProject.Contracts.Services;
using CalendarProject.EntityFramework;
using CalendarProject.Helpers;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.UI.Xaml;
using Windows.ApplicationModel;
using Windows.Globalization;

namespace CalendarProject.ViewModels;

public partial class SettingsViewModel : ObservableRecipient
{
    private readonly IThemeSelectorService _themeSelectorService;

    [ObservableProperty]
    private ElementTheme _elementTheme;

    [ObservableProperty]
    private string _versionDescription;

    [ObservableProperty]
    private bool _isEnglishChecked;

    [ObservableProperty]
    private bool _isRussianChecked;

    [ObservableProperty]
    private bool _isLocaleChanged;

    public ICommand SwitchThemeCommand { get; }

    public ICommand SwitchLocaleCommand { get; }

    DbWorker worker;

    public SettingsViewModel(IThemeSelectorService themeSelectorService)
    {
        worker = App.GetService<DbWorker>();

        _themeSelectorService = themeSelectorService;
        _elementTheme = _themeSelectorService.Theme;
        _versionDescription = GetVersionDescription();

        GetCurrentLocaleSelection();

        SwitchThemeCommand = new RelayCommand<ElementTheme>(
            async (param) =>
            {
                if (ElementTheme != param)
                {
                    ElementTheme = param;
                    await _themeSelectorService.SetThemeAsync(param);
                }
            });

        SwitchLocaleCommand = new RelayCommand<string>(
            (param) =>
            {
                if (param != null)
                {
                    ApplicationLanguages.PrimaryLanguageOverride = param;
                    GetCurrentLocaleSelection();
                }
            });
    }

    private void GetCurrentLocaleSelection()
    {
        var currentLocale = ApplicationLanguages.PrimaryLanguageOverride;

        switch (currentLocale)
        {
            case "en-US":
                IsLocaleChanged = !"en-US".Equals(SessionContext.StartLangId);
                SessionContext.CurrentSettings.LangId = "en-US";
                IsEnglishChecked = true;
                break;
            case "ru":
                IsLocaleChanged = !"ru".Equals(SessionContext.StartLangId);
                SessionContext.CurrentSettings.LangId = "ru";
                IsRussianChecked = true;
                break;
            default:
                break;
        }

        worker.DbUpdate<Settings>(SessionContext.CurrentSettings);
    }

    private static string GetVersionDescription()
    {
        Version version;

        if (RuntimeHelper.IsMSIX)
        {
            var packageVersion = Package.Current.Id.Version;

            version = new(packageVersion.Major, packageVersion.Minor, packageVersion.Build, packageVersion.Revision);
        }
        else
        {
            version = Assembly.GetExecutingAssembly().GetName().Version!;
        }

        return $"{"AppDisplayName".GetLocalized()} - {version.Major}.{version.Minor}.{version.Build}.{version.Revision}";
    }
}
