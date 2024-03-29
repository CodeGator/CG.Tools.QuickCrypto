﻿
using Microsoft.JSInterop;
using MudBlazor;

namespace CG.Tools.QuickCrypto.Pages.Text;

/// <summary>
/// This class is the view-model for the <see cref="TextPage"/> page.
/// </summary>
public class TextPageViewModel : ObservableObject
{
    // *******************************************************************
    // Fields.
    // *******************************************************************

    #region Fields

    /// <summary>
    /// This field backs the <see cref="Error"/> property.
    /// </summary>
    internal protected string _error = "";

    /// <summary>
    /// This field backs the <see cref="DecryptedText"/> property.
    /// </summary>
    internal protected string _decryptedText = "";

    /// <summary>
    /// This field backs the <see cref="EncryptedText"/> property.
    /// </summary>
    internal protected string _encryptedText = "";

    /// <summary>
    /// This field contains the JS service for this view-model.
    /// </summary>
    internal protected readonly IJSRuntime _jSRuntime;

    /// <summary>
    /// This field contains the snackbar service for this view-model.
    /// </summary>
    internal protected readonly ISnackbar _snackbar;

    /// <summary>
    /// This field contains the cryptography service for this view-model.
    /// </summary>
    internal protected readonly Cryptography _cryptography;

    #endregion

    // *******************************************************************
    // Properties.
    // *******************************************************************

    #region Properties

    /// <summary>
    /// This property contains the decrypted text to be encrypted.
    /// </summary>
    public string DecryptedText
    {
        get { return _decryptedText; }
        set { SetProperty(ref _decryptedText, value); }
    }

    /// <summary>
    /// This property contains the encrypted text to be decrypted.
    /// </summary>
    public string EncryptedText
    {
        get { return _encryptedText; }
        set { SetProperty(ref _encryptedText, value); }
    }

    /// <summary>
    /// This property contains any error messages generated by the view-model.
    /// </summary>
    public string Error
    {
        get { return _error; }
        set { SetProperty(ref _error, value); }
    }

    #endregion

    // *******************************************************************
    // Constructors.
    // *******************************************************************

    #region Constructors

    /// <summary>
    /// This constructor creates a new instance of the <see cref="TextPageViewModel"/>
    /// class.
    /// </summary>
    /// <param name="jSRuntime">The JS service for this view-model.</param>
    /// <param name="snackbar">The snackbar service for this view-model.</param>
    /// <param name="cryptography">The cryptography service for this view-model.</param>
    public TextPageViewModel(
        IJSRuntime jSRuntime,
        ISnackbar snackbar,
        Cryptography cryptography
        )
    {
        // Save the reference(s).
        _jSRuntime = jSRuntime;
        _snackbar = snackbar;
        _cryptography = cryptography;
    }

    #endregion

    // *******************************************************************
    // Public methods.
    // *******************************************************************

    #region Public methods

    /// <summary>
    /// This method copies the decrypted text to the clipboard.
    /// </summary>
    /// <returns></returns>
    public virtual async Task CopyDecryptedTextToClipboard()
    {
        try
        {
            // Copy the text.
            await _jSRuntime.InvokeAsync<string>(
                "clipboardCopy.copyText",
                new object[] { DecryptedText }
                );

            // Tell the caller what we did.
            _snackbar.Add(
                "Decrypted text was copied to the clipboard",
                Severity.Info
                );
        }
        catch (Exception ex)
        {
            // Save the error message.
            Error = ex.Message;
        }        
    }

    // *******************************************************************

    /// <summary>
    /// This method copies the encrypted text to the clipboard.
    /// </summary>
    /// <returns></returns>
    public virtual async Task CopyEncryptedTextToClipboard()
    {
        try
        {
            // Copy the text.
            await _jSRuntime.InvokeAsync<string>(
                "clipboardCopy.copyText",
                new object[] { EncryptedText }
                );

            // Tell the caller what we did.
            _snackbar.Add(
                "Encrypted text was copied to the clipboard",
                Severity.Info
                );
        }
        catch (Exception ex)
        {
            // Save the error message.
            Error = ex.Message;
        }
    }

    // *******************************************************************

    /// <summary>
    /// This method encrypts the <see cref="DecryptedText"/> property and 
    /// stores the results in the <see cref="EncryptedText"/> property.
    /// </summary>
    /// <returns></returns>
    public virtual async Task EncryptText()
    {
        try
        {
            // Clear any old error(s).
            Error = "";

            // Fetch the credentials.
            var password = await SecureStorage.GetAsync(Globals.Storage.Password);
            var salt = await SecureStorage.GetAsync(Globals.Storage.Salt);
            var iterations = await SecureStorage.GetAsync(Globals.Storage.Iterations);

            // Sanity check the credentials.
            if (string.IsNullOrEmpty(password) ||
                string.IsNullOrEmpty(salt) ||
                string.IsNullOrEmpty(iterations))
            {
                Error = "The credentials are missing in the settings!";
                return; // Nothing to do!
            }

            // Generate the key and IV.
            var keyAndIV = await _cryptography.GenerateKeyAndIVAsync(
                password,
                salt,
                int.Parse(iterations)
                );

            // Encrypt the text.
            EncryptedText = await _cryptography.AesEncryptAsync(
                keyAndIV.Item1,
                keyAndIV.Item2,
                DecryptedText
                );
        }
        catch (Exception ex)
        {
            // Save the error message.
            Error = ex.Message;
        }
    }

    // *******************************************************************

    /// <summary>
    /// This method encrypts the <see cref="EncryptedText"/> property and 
    /// stores the results in the <see cref="DecryptedText"/> property.
    /// </summary>
    /// <returns></returns>
    public virtual async Task DecryptText()
    {
        try
        {
            // Clear any old error(s).
            Error = "";

            // Fetch the credentials.
            var password = await SecureStorage.GetAsync(Globals.Storage.Password);
            var salt = await SecureStorage.GetAsync(Globals.Storage.Salt);
            var iterations = await SecureStorage.GetAsync(Globals.Storage.Iterations);

            // Sanity check the credentials.
            if (string.IsNullOrEmpty(password) ||
                string.IsNullOrEmpty(salt) ||
                string.IsNullOrEmpty(iterations))
            {
                Error = "The credentials are missing in the settings!";
                return; // Nothing to do!
            }

            // Generate the key and IV.
            var keyAndIV = await _cryptography.GenerateKeyAndIVAsync(
                password,
                salt,
                int.Parse(iterations)
                );

            // Decrypt the text.
            DecryptedText = await _cryptography.AesDecryptAsync(
                keyAndIV.Item1,
                keyAndIV.Item2,
                EncryptedText
                );
        }
        catch (Exception ex)
        {
            // Save the error message.
            Error = ex.Message;
        }
    }

    #endregion
}
