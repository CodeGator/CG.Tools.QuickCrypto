
using MudBlazor;

namespace CG.Tools.QuickCrypto.Pages.Text;

/// <summary>
/// This class is the code-behind for the <see cref="TextPage"/> page.
/// </summary>
public partial class TextPage
{
    // *******************************************************************
    // Properties.
    // *******************************************************************

    #region Properties

    /// <summary>
    /// This property contains the navigation manager for this page.
    /// </summary>
    [Inject]
    protected NavigationManager Navigation { get; set; }

    /// <summary>
    /// This property contains the dialog service for this page.
    /// </summary>
    [Inject]
    protected IDialogService DialogService { get; set; }

    /// <summary>
    /// This property contains the secure storage service for this page.
    /// </summary>
    [Inject]
    protected ISecureStorage SecureStorage { get; set; }

    /// <summary>
    /// This property contains the snackbar service for this page.
    /// </summary>
    [Inject]
    protected ISnackbar Snackbar { get; set; }

    #endregion

    // *******************************************************************
    // Protected methods.
    // *******************************************************************

    #region Protected methods

    /// <summary>
    /// This method initializes the page.
    /// </summary>
    /// <returns>A task to perform the operation.</returns>
    protected override async Task OnInitializedAsync()
    {
        // Give the base class a chance,
        await base.OnInitializedAsync();

        // Should we add default settings?
        if (string.IsNullOrEmpty(await SecureStorage.GetAsync(Globals.Storage.Password)) ||
            string.IsNullOrEmpty(await SecureStorage.GetAsync(Globals.Storage.Salt)) ||
            string.IsNullOrEmpty(await SecureStorage.GetAsync(Globals.Storage.Iterations)))
        {
            // If we get here, the application hasn't yet saved settings
            //   so we need to create some default values.

            // Add default settings.
            await SecureStorage.SetAsync(Globals.Storage.Password, "My Password");
            await SecureStorage.SetAsync(Globals.Storage.Salt, "My Salt");
            await SecureStorage.SetAsync(Globals.Storage.Iterations, "30000");

            // Tell the caller what we did.
            await DialogService.ShowMessageBox(
                "Oh by the way ...",
                "We created default credentials in the settings. " +
                "You can change those credentials at any time by " +
                "going to the settings page."
                );
        }
    }

    #endregion
}
