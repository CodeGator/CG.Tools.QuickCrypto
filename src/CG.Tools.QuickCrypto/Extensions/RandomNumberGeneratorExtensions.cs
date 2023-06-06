
// I found this idea here: https://stackoverflow.com/questions/1344221/how-can-i-generate-random-alphanumeric-strings

namespace System.Security.Cryptography;

/// <summary>
/// This class contains extension methods related to the <see cref="RandomNumberGenerator"/>
/// type.
/// </summary>
public static partial class RandomNumberGeneratorExtensions
{
    // *******************************************************************
    // Fields.
    // *******************************************************************

    #region Fields

    /// <summary>
    /// This field contains mixed case alphanumeric characters for generating strings.
    /// </summary>
    private static readonly char[] _allChars =
        "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890~!@#$%^&*()[];:<>,.-=_+".ToCharArray();

    #endregion

    // *******************************************************************
    // Public methods.
    // *******************************************************************

    #region Public methods

    /// <summary>
    /// This method generates a string of random alphanumeric characters.
    /// </summary>
    /// <param name="random">The random number generator to use for the 
    /// operation.</param>
    /// <param name="size">The number of characters to include in the 
    /// string.</param>
    /// <returns>A random alphanumeric string.</returns>
    /// <example>
    /// This example shows how to call the <see cref="RandomNumberGeneratorExtensions.NextString(RandomNumberGenerator, int)"/>
    /// method.
    /// <code>
    /// class TestClass
    /// {
    ///     static void Main()
    ///     {
    ///         var random = RandomNumberGenerator.Create();
    ///         var str = random.NextString(10);
    /// 
    ///         // str contains a 10 character string.
    ///     }
    /// }
    /// </code>
    /// </example>
    public static string NextString(
        this RandomNumberGenerator random,
        int size
        )
    {
        // Get some random bytes.
        byte[] data = new byte[sizeof(int) * size];
        random.GetBytes(data);

        // Loop and convert the bytes to characters.
        var sb = new StringBuilder(size);
        for (int i = 0; i < size; i++)
        {
            // Get the next byte.
            var rnd = BitConverter.ToUInt32(data, i * sizeof(int));

            // Convert the byte to an index.
            var idx = rnd % _allChars.Length;

            // Add the corresponding character to the string.
            sb.Append(_allChars[idx]);
        }

        // Get the resulting string.
        var result = sb.ToString();

        // Return the results.
        return result;
    }

    // *******************************************************************

    /// <summary>
    /// This method returns a random integer who's value has been constrained
    /// to be within the range specified by the <paramref name="min"/> and
    /// <paramref name="max"/> parameters.
    /// </summary>
    /// <param name="random">The random number generator to use for the 
    /// operation.</param>
    /// <param name="min">The lower value for the range.</param>
    /// <param name="max">The upper value for the range.</param>
    /// <returns>A randon integer who's value has been constrained
    /// to be within the range specified by the <paramref name="min"/> and
    /// <paramref name="max"/> parameters.</returns>
    /// <example>
    /// This example shows how to call the <see cref="RandomNumberGeneratorExtensions.Next(RandomNumberGenerator, int, int)"/>
    /// method.
    /// <code>
    /// class TestClass
    /// {
    ///     static void Main()
    ///     {
    ///         var random = RandomNumberGenerator.Create();
    ///         var num = random.Next(1, 10);
    /// 
    ///         // num contains a number between 1 and 10.
    ///     }
    /// }
    /// </code>
    /// </example>
    public static int Next(
        this RandomNumberGenerator random,
        int min,
        int max
        )
    {
        // Fixup max.
        max--;

        // Get some random bytes.
        var bytes = new byte[sizeof(int)];
        random.GetNonZeroBytes(bytes);

        // Convert to a value.
        var val = BitConverter.ToInt32(bytes);

        // Constrain the value.
        var result = ((val - min) % (max - min + 1) + (max - min + 1)) %
            (max - min + 1) + min;

        // Return the results.
        return result;

    }

    #endregion
}
