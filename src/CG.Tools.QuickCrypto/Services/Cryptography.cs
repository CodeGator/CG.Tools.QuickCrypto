
namespace CG.Tools.QuickCrypto.Services;

/// <summary>
/// This class represents a service that performs cryptography operations.
/// </summary>
public class Cryptography
{
    // *******************************************************************
    // Fields.
    // *******************************************************************

    #region Fields

    /// <summary>
    /// This field contains the logger for the service.
    /// </summary>
    internal protected readonly ILogger<Cryptography> _logger;

    #endregion

    // *******************************************************************
    // Constructors.
    // *******************************************************************

    #region Constructors

    /// <summary>
    /// This constructor creates a new instance of the <see cref="Cryptography"/>
    /// class.
    /// </summary>
    /// <param name="logger">The logger to use with the service.</param>
    public Cryptography(
        ILogger<Cryptography> logger
        )
    {
        // Save the reference(s).
        _logger = logger;
    }

    #endregion

    // *******************************************************************
    // Public methods.
    // *******************************************************************

    #region Public methods

    /// <summary>
    /// This method generates a cryptographically secure key and IV using 
    /// the given password, SALT, and iterations count.
    /// </summary>
    /// <param name="password">The password to use for the operation.</param>
    /// <param name="salt">The SALT to use for the operation.</param>
    /// <param name="iterationCount">The minimum iterations to use for 
    /// the operation.</param>
    /// <param name="cancellationToken">A cancellation token that is monitored
    /// throughout the lifetime of the method.</param>
    /// <returns>A tuple contains byte arrays for the key and IV.</returns>
    /// <exception cref="CryptographyException">This exception is thrown 
    /// whenever the method fails to complete.</exception>    
    public virtual ValueTask<Tuple<byte[], byte[]>> GenerateKeyAndIVAsync(
        string password,
        string salt,
        int iterationCount = 30000,
        CancellationToken cancellationToken = default
        )
    {
        try
        {
            // Log what we are about to do.
            _logger.LogDebug(
                "Generating an AES algorithm instance"
                );

            // Create the AES algorithm.
            using var alg = Aes.Create();

            // Log what we are about to do.
            _logger.LogDebug(
                "Setting the key and block sizes"
                );

            // Set the block and key sizes.
            alg.KeySize = 256;
            alg.BlockSize = 128;

            // Log what we are about to do.
            _logger.LogDebug(
                "Converting the password to bytes"
                );

            // Convert the password to bytes.
            var passwordBytes = Encoding.UTF8.GetBytes(
                password
                );

            // Log what we are about to do.
            _logger.LogDebug(
                "Converting the salt to bytes"
                );

            // Convert the salt to bytes.
            var saltBytes = Encoding.UTF8.GetBytes(
                salt
                );

            // Log what we are about to do.
            _logger.LogDebug(
                "Deriving the RFC2898 based cryptographic key"
                );

            // Derive the Key and IV.
            var derivedKey = new Rfc2898DeriveBytes(
                passwordBytes,
                saltBytes,
                iterationCount,
                new HashAlgorithmName("SHA512")
                );

            // Log what we are about to do.
            _logger.LogDebug(
                "Packaging the tuple with Key and IV"
                );

            // Return the results.
            var tuple = new Tuple<byte[], byte[]>(
                derivedKey.GetBytes(alg.KeySize / 8),
                derivedKey.GetBytes(alg.BlockSize / 8)
                );

            // Return the results.
            return ValueTask.FromResult(tuple);
        }
        catch (Exception ex)
        {
            // Log what happened.
            _logger.LogError(
                ex,
                "Failed to generate a cryptographic Key and IV!"
                );

            // Provider better context.
            throw new CryptographyException(
                message: $"Failed to generate a cryptographic Key and IV!",
                innerException: ex
                );
        }
    }

    // *******************************************************************

    /// <summary>
    /// This method decrypts the given stream using AES.
    /// </summary>
    /// <param name="key">The key to use for the operation.</param>
    /// <param name="iv">The IV to use for the operation.</param>
    /// <param name="value">The value to use for the operation.</param>
    /// <param name="cancellationToken">A cancellation token.</param>
    /// <returns>A task to perform the operation that returns a decrypted string.</returns>
    /// <exception cref="ArgumentException">This exception is thrown whenever
    /// one or more arguments are missing, or invalid.</exception>
    /// <exception cref="CryptographicException">This exception is thrown 
    /// whenever the operation fails to complete properly.</exception>
    public virtual async ValueTask<Stream> AesDecryptAsync(
        byte[] key,
        byte[] iv,
        Stream value,
        CancellationToken cancellationToken = default
        )
    {
        try
        {
            // Can we take a shortcut?
            if (value is null || value.Length == 0)
            {
                return new MemoryStream();
            }

            // Log what we are about to do.
            _logger.LogDebug(
                "Generating an AES algorithm instance"
                );

            // Create the algorithm
            using (var alg = Aes.Create())
            {
                // Log what we are about to do.
                _logger.LogDebug(
                    "Setting the key and block sizes"
                    );

                // Set the block and key sizes.
                alg.KeySize = 256;
                alg.BlockSize = 128;

                // Log what we are about to do.
                _logger.LogDebug(
                    "Setting the Key and IV values"
                    );

                // Copy the key and IV.
                alg.Key = key;
                alg.IV = iv;

                // Log what we are about to do.
                _logger.LogDebug(
                    "Creating an AES decryptor"
                    );

                // Create the decryptor.
                using (var dec = alg.CreateDecryptor())
                {
                    // Log what we are about to do.
                    _logger.LogDebug(
                        "Creating a destination stream"
                        );

                    // Create a destination stream.
                    var destStream = new MemoryStream();

                    // Log what we are about to do.
                    _logger.LogDebug(
                        "Creating a cryptography stream"
                        );

                    // Create a cryptography stream.
                    using (var cryptoStream = new CryptoStream(
                        destStream,
                        dec,
                        CryptoStreamMode.Write,
                        true
                        ))
                    {
                        // Log what we are about to do.
                        _logger.LogDebug(
                            "Copying bytes to the cryptography stream"
                            );

                        // Copy the bits.
                        await value.CopyToAsync(
                            cryptoStream
                            ).ConfigureAwait(false);

                        // Log what we are about to do.
                        _logger.LogDebug(
                            "Flushing the final block in the cryptography stream"
                            );

                        // Flush it baby!!
                        cryptoStream.FlushFinalBlock();
                    }

                    // Log what we are about to do.
                    _logger.LogDebug(
                        "Repositioning the destination stream"
                        );

                    // Move back to the beginning.
                    destStream.Seek(0, SeekOrigin.Begin);

                    // Return the results.
                    return destStream;
                }
            }
        }
        catch (Exception ex)
        {
            // Log what happened.
            _logger.LogError(
                "Failed to decrypt a stream with AES!"
                );

            // Provider better context.
            throw new CryptographyException(
                message: $"Failed to decrypt a stream with AES!",
                innerException: ex
                );
        }
    }

    // *******************************************************************

    /// <summary>
    /// This method decrypts the given string using AES.
    /// </summary>
    /// <param name="key">The key to use for the operation.</param>
    /// <param name="iv">The IV to use for the operation.</param>
    /// <param name="value">The value to use for the operation.</param>
    /// <param name="cancellationToken">A cancellation token.</param>
    /// <returns>A task to perform the operation that returns a decrypted string.</returns>
    /// <exception cref="ArgumentException">This exception is thrown whenever
    /// one or more arguments are missing, or invalid.</exception>
    /// <exception cref="CryptographicException">This exception is thrown 
    /// whenever the operation fails to complete properly.</exception>
    public virtual ValueTask<string> AesDecryptAsync(
        byte[] key,
        byte[] iv,
        string? value,
        CancellationToken cancellationToken = default
        )
    {
        try
        {
            // Can we take a shortcut?
            if (string.IsNullOrEmpty(value))
            {
                return ValueTask.FromResult(string.Empty);
            }

            // Log what we are about to do.
            _logger.LogDebug(
                "Generating an AES algorithm instance"
                );

            // Create the algorithm
            using (var alg = Aes.Create())
            {

                // Log what we are about to do.
                _logger.LogDebug(
                    "Setting the key and block sizes"
                    );

                // Set the block and key sizes.
                alg.KeySize = 256;
                alg.BlockSize = 128;

                // Log what we are about to do.
                _logger.LogDebug(
                    "Setting the Key and IV values"
                    );

                // Copy the key and IV.
                alg.Key = key;
                alg.IV = iv;

                // Log what we are about to do.
                _logger.LogDebug(
                    "Creating an AES decryptor"
                    );

                // Create the decryptor.
                using (var dec = alg.CreateDecryptor())
                {
                    // Log what we are about to do.
                    _logger.LogDebug(
                        "Converting the string to bytes"
                        );

                    // Convert the encrypted value to bytes.
                    var encryptedBytes = Convert.FromBase64String(
                        value
                        );

                    // Log what we are about to do.
                    _logger.LogDebug(
                        "Creating a memory stream"
                        );

                    // Create a temporary stream.
                    using (var stream = new MemoryStream(
                        encryptedBytes
                        ))
                    {
                        // Log what we are about to do.
                        _logger.LogDebug(
                            "Creating a cryptography stream"
                            );

                        // Create a cryptography stream.
                        using (var cryptoStream = new CryptoStream(
                            stream,
                            dec,
                            CryptoStreamMode.Read
                            ))
                        {
                            // Log what we are about to do.
                            _logger.LogDebug(
                                "Creating a stream reader"
                                );

                            // Create a cryptography reader.
                            using (var reader = new StreamReader(
                                cryptoStream
                                ))
                            {
                                // Log what we are about to do.
                                _logger.LogDebug(
                                    "Reading the plain text"
                                    );

                                // Get the decrypted text.
                                var plainText = reader.ReadToEnd();

                                // Return the results.
                                return ValueTask.FromResult(plainText);
                            }
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            // Log what happened.
            _logger.LogError(
                "Failed to decrypt a string with AES!"
                );

            // Provider better context.
            throw new CryptographyException(
                message: $"Failed to decrypt a string with AES!",
                innerException: ex
                );
        }
    }

    // *******************************************************************

    /// <summary>
    /// This method encrypts the given stream using AES.
    /// </summary>
    /// <param name="key">The key to use for the operation.</param>
    /// <param name="iv">The IV to use for the operation.</param>
    /// <param name="value">The value to use for the operation.</param>
    /// <param name="cancellationToken">A cancellation token.</param>
    /// <returns>A task to perform the operation that returns an encrypted string.</returns>
    /// <exception cref="ArgumentException">This exception is thrown whenever
    /// one or more arguments are missing, or invalid.</exception>
    /// <exception cref="CryptographicException">This exception is thrown 
    /// whenever the operation fails to complete properly.</exception>
    public virtual async ValueTask<Stream> AesEncryptAsync(
        byte[] key,
        byte[] iv,
        Stream value,
        CancellationToken cancellationToken = default
        )
    {
        try
        {
            // Can we take a shortcut?
            if (value is null || value.Length == 0)
            {
                return new MemoryStream();
            }

            // Log what we are about to do.
            _logger.LogDebug(
                "Generating an AES algorithm instance"
                );

            // Create the algorithm
            using (var alg = Aes.Create())
            {
                // Log what we are about to do.
                _logger.LogDebug(
                    "Setting the key and block sizes"
                    );

                // Set the block and key sizes.
                alg.KeySize = 256;
                alg.BlockSize = 128;

                // Log what we are about to do.
                _logger.LogDebug(
                    "Setting the key and IV values"
                    );

                // Copy the Key and IV.
                alg.Key = key;
                alg.IV = iv;

                // Log what we are about to do.
                _logger.LogDebug(
                    "Creating an AES encryptor"
                    );

                // Create the encryptor.
                using (var enc = alg.CreateEncryptor())
                {
                    // Create a cryptographic stream.
                    using (var cryptoStream = new CryptoStream(
                        value,
                        enc,
                        CryptoStreamMode.Read
                        ))
                    {
                        // Log what we are about to do.
                        _logger.LogDebug(
                            "Creating a destination stream"
                            );

                        // Create the destination stream.
                        var destStream = new MemoryStream();

                        // Log what we are about to do.
                        _logger.LogDebug(
                            "Copying bytes from the cryptography stream"
                            );

                        // Copy the bits.
                        await cryptoStream.CopyToAsync(
                            destStream
                            ).ConfigureAwait(false);

                        // Log what we are about to do.
                        _logger.LogDebug(
                            "Repositioning the destination stream"
                            );

                        // Move back to the beginning.
                        destStream.Seek(0, SeekOrigin.Begin);

                        // Return the results.
                        return destStream;
                    }
                }
            }
        }
        catch (Exception ex)
        {
            // Log what happened.
            _logger.LogError(
                "Failed to encrypt a stream with AES!"
                );

            // Provider better context.
            throw new CryptographyException(
                message: $"Failed to encrypt a stream with AES!",
                innerException: ex
                );
        }
    }

    // *******************************************************************

    /// <summary>
    /// This method encrypts the given string using AES.
    /// </summary>
    /// <param name="key">The key to use for the operation.</param>
    /// <param name="iv">The IV to use for the operation.</param>
    /// <param name="value">The value to use for the operation.</param>
    /// <param name="cancellationToken">A cancellation token.</param>
    /// <returns>A task to perform the operation that returns an encrypted string.</returns>
    /// <exception cref="ArgumentException">This exception is thrown whenever
    /// one or more arguments are missing, or invalid.</exception>
    /// <exception cref="CryptographicException">This exception is thrown 
    /// whenever the operation fails to complete properly.</exception>
    public virtual async ValueTask<string> AesEncryptAsync(
        byte[] key,
        byte[] iv,
        string? value,
        CancellationToken cancellationToken = default
        )
    {
        try
        {
            // Can we take a shortcut?
            if (string.IsNullOrEmpty(value))
            {
                return string.Empty;
            }

            // Log what we are about to do.
            _logger.LogDebug(
                "Generating an AES algorithm instance"
                );

            // Create the algorithm
            using (var alg = Aes.Create())
            {

                // Log what we are about to do.
                _logger.LogDebug(
                    "Setting the key and block sizes"
                    );

                // Set the block and key sizes.
                alg.KeySize = 256;
                alg.BlockSize = 128;

                // Log what we are about to do.
                _logger.LogDebug(
                    "Setting the key and IV values"
                    );

                // Copy the Key and IV.
                alg.Key = key;
                alg.IV = iv;

                // Log what we are about to do.
                _logger.LogDebug(
                    "Creating an AES encryptor"
                    );

                // Create the encryptor.
                using (var enc = alg.CreateEncryptor())
                {
                    // Log what we are about to do.
                    _logger.LogDebug(
                        "Creating a memory stream"
                        );

                    // Create a temporary stream.
                    using (var stream = new MemoryStream())
                    {
                        // Log what we are about to do.
                        _logger.LogDebug(
                            "Creating a cryptography stream"
                            );

                        // Create a cryptographic stream.
                        using (var cryptoStream = new CryptoStream(
                            stream,
                            enc,
                            CryptoStreamMode.Write
                            ))
                        {
                            // Log what we are about to do.
                            _logger.LogDebug(
                                "Creating a stream writer"
                                );

                            // Create a writer
                            using (var writer = new StreamWriter(
                                cryptoStream
                                ))
                            {
                                // Log what we are about to do.
                                _logger.LogDebug(
                                    "Writing the plain bytes"
                                    );

                                // Write the text.
                                await writer.WriteAsync(
                                    value
                                    ).ConfigureAwait(false);
                            }

                            // Log what we are about to do.
                            _logger.LogDebug(
                                "Reading the encrypted bytes"
                                );

                            // Get the bytes.
                            var encryptedBytes = stream.ToArray();

                            // Log what we are about to do.
                            _logger.LogDebug(
                                "Converting the encrypted bytes to a base-64 string"
                                );

                            // Convert the bytes to base-64.
                            var encryptedValue = Convert.ToBase64String(
                                encryptedBytes
                                );

                            // Return the results.
                            return encryptedValue;
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            // Log what happened.
            _logger.LogError(
                "Failed to encrypt a string with AES!"
                );

            // Provider better context.
            throw new CryptographyException(
                message: $"Failed to encrypt a string with AES!",
                innerException: ex
                );
        }
    }

    #endregion
}
