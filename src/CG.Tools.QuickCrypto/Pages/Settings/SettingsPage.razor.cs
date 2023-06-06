
using MudBlazor;

namespace CG.Tools.QuickCrypto.Pages.Settings;

/// <summary>
/// This page contains the code-behind for the <see cref="SettingsPage"/> page.
/// </summary>
public partial class SettingsPage
{
    // *******************************************************************
    // Fields.
    // *******************************************************************

    #region Fields

    /// <summary>
    /// This field indicates when to show the password value on the UI.
    /// </summary>
    bool _showPassword;

    /// <summary>
    /// This field contains the input type for the password control.
    /// </summary>
    InputType _passwordInputType = InputType.Password;

    /// <summary>
    /// This field contains the icon for the password control.
    /// </summary>
    string _passwordInputIcon = Icons.Material.Filled.VisibilityOff;

    /// <summary>
    /// This field indicates when to show the salt value on the UI.
    /// </summary>
    bool _showSalt;

    /// <summary>
    /// This field contains the input type for the salt control.
    /// </summary>
    InputType _saltInputType = InputType.Password;

    /// <summary>
    /// This field contains the icon for the salt control.
    /// </summary>
    string _saltInputIcon = Icons.Material.Filled.VisibilityOff;

    #endregion

    // *******************************************************************
    // Properties.
    // *******************************************************************

    #region Properties

    /// <summary>
    /// This property contains the secure storage service for this page.
    /// </summary>
    [Inject]
    protected ISecureStorage SecureStorage { get; set; } = null!;

    #endregion

    // *******************************************************************
    // Protected methods.
    // *******************************************************************

    #region Protected methods

    /// <summary>
    /// This method is called to initialize the page.
    /// </summary>
    /// <returns>A task to perform the operation.</returns>
    #endregion
    protected override async Task OnInitializedAsync()
    {
        // NOTE: we don't do this in the view-model because it would need
        //   to be performed during the view-model's ctor. As we all know,
        //   in C#, the language doesn't allow us to call async methods in
        //   a ctor. We could call Wait() to get around that, but that's
        //   just a deadlock waiting to happen. We could jump through hoops,
        //   or otherwise hack our way through that little dilemma, but,
        //   this is simpler, and cleaner.

        // Set view-model properties from secure storage.
        ViewModel.Password = await SecureStorage.GetAsync(Globals.Storage.Password);
        ViewModel.Salt = await SecureStorage.GetAsync(Globals.Storage.Salt);
        var iterations = await SecureStorage.GetAsync(Globals.Storage.Iterations);
        if (iterations == null)
        {
            ViewModel.Iterations = 30000;
        }
        else
        {
            ViewModel.Iterations = int.Parse(iterations);
        }

        // Give the base class a chance.
        await base.OnInitializedAsync();
    }

    // *******************************************************************

    /// <summary>
    /// This method is called whenever the password value changes.
    /// </summary>
    /// <param name="password">The new password value.</param>
    /// <returns>A task to perform the operation.</returns>
    async Task OnPasswordChanged(string password)
    {
        // Set the view-model property.
        ViewModel.Password = string.IsNullOrEmpty(password) 
            ? "My Password" 
            : password;

        // Save the change to secure storage.
        await SecureStorage.SetAsync(
            Globals.Storage.Password,
            ViewModel.Password
            );
    }

    // *******************************************************************

    /// <summary>
    /// This method is called whenever the salt value changes.
    /// </summary>
    /// <param name="salt">The new salt value.</param>
    /// <returns>A task to perform the operation.</returns>
    async Task OnSaltChanged(string salt)
    {
        // Set the view-model property.
        ViewModel.Salt = string.IsNullOrEmpty(salt) 
            ? "My Salt" 
            : salt;

        // Save the change to secure storage.
        await SecureStorage.SetAsync(
            Globals.Storage.Salt,
            ViewModel.Salt
            );
    }

    // *******************************************************************

    /// <summary>
    /// This method is called whenever the iterations value changes.
    /// </summary>
    /// <param name="iterations">The new iterations value.</param>
    /// <returns>A task to perform the operation.</returns>
    async Task OnIterationsChanged(int iterations)
    {
        // Is the value missing?
        if (iterations == 0) 
        {
            return; // Do nothing.
        }

        // Is the value too low?
        if (iterations < 10000)
        {
            iterations = 10000;
        }

        // Is the value too high?
        if (iterations > 100000)
        {
            iterations = 100000;
        }

        // Set the view-model property.
        ViewModel.Iterations = iterations;

        // Save the change to secure storage.
        await SecureStorage.SetAsync(
            Globals.Storage.Iterations, 
            $"{ViewModel.Iterations}"
            );
    }

    // *******************************************************************

    /// <summary>
    /// This method toggles the visibility of the password control contents.
    /// </summary>
    void TogglePasswordVisibility()
    {
        if (_showPassword)
        {
            _showPassword = false;
            _passwordInputIcon = Icons.Material.Filled.VisibilityOff;
            _passwordInputType = InputType.Password;
        }
        else
        {
            _showPassword = true;
            _passwordInputIcon = Icons.Material.Filled.Visibility;
            _passwordInputType = InputType.Text;
        }
    }

    // *******************************************************************

    /// <summary>
    /// This method toggles the visibility of the salt control contents.
    /// </summary>
    void ToggleSaltVisibility()
    {
        if (_showSalt)
        {
            _showSalt = false;
            _saltInputIcon = Icons.Material.Filled.VisibilityOff;
            _saltInputType = InputType.Password;
        }
        else
        {
            _showSalt = true;
            _saltInputIcon = Icons.Material.Filled.Visibility;
            _saltInputType = InputType.Text;
        }
    }
}
