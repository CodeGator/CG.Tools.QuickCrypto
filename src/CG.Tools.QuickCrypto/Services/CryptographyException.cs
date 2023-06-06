
namespace CG.Tools.QuickCrypto.Services;

/// <summary>
/// This class represents a cryptography related exception.
/// </summary>
public class CryptographyException : Exception
{
    // *******************************************************************
    // Constructors.
    // *******************************************************************

    #region Constructors

    /// <summary>
    /// This constructor creates a new instance of the <see cref="CryptographyException"/>
    /// class.
    /// </summary>
    public CryptographyException()
    {

    }

    // *******************************************************************

    /// <summary>
    /// This constructor creates a new instance of the <see cref="CryptographyException"/>
    /// class.
    /// </summary>
    /// <param name="message">The message to use for the exception.</param>
    /// <param name="innerException">An optional inner exception reference.</param>
    public CryptographyException(
        string message,
        Exception innerException
        ) : base(message, innerException)
    {

    }

    // *******************************************************************

    /// <summary>
    /// This constructor creates a new instance of the <see cref="CryptographyException"/>
    /// class.
    /// </summary>
    /// <param name="message">The message to use for the exception.</param>
    public CryptographyException(
        string message
        ) : base(message)
    {

    }

    #endregion
}
