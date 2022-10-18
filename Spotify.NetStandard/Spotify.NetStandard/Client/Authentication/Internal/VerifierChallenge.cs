namespace Spotify.NetStandard.Client.Authentication.Internal;

/// <summary>
/// Verifier Challenge Request for Proof Key for Code Exchange
/// </summary>
internal class VerifierChallenge
{
    private const int verifier_length = 128;
    private const string verifier_content = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789-._~";
    private static readonly Random random = new((int)DateTime.Now.Ticks);

    /// <summary>
    /// Get Verifier
    /// </summary>
    /// <returns>Cryptographically random string between 43 and 128 characters in length</returns>
    private string GetVerifier()
    {
        var verifier = new StringBuilder();
        for(var i = 0; i < verifier_length; i++)
            verifier.Append(verifier_content[random.Next(0, verifier_content.Length)]);
        return verifier.ToString();
    }

    /// <summary>
    /// Base 64 Url Encode
    /// </summary>
    /// <param name="value">Values to Encode</param>
    /// <returns>Encoded Values</returns>
    private static string Base64UrlEncode(byte[] value) => 
        Convert.ToBase64String(value)
          .Replace('+', '-')
          .Replace('/', '_')
          .Replace("=", "");

    /// <summary>
    /// Get Challenge
    /// </summary>
    /// <returns>Base 64 Encoded Verifier Hashed with SHA256</returns>
    private string GetChallenge()
    {
        var crypt = new SHA256Managed();
        var hash = crypt.ComputeHash(Encoding.UTF8.GetBytes(Verifier));
        return Base64UrlEncode(hash);
    }

    /// <summary>
    /// Cryptographically random string between 43 and 128 characters in length. It can contain letters, digits, underscores, periods, hyphens, or tildes
    /// </summary>
    public string Verifier { get; private set; }

    /// <summary>
    /// Base 64 Encoded Hash of Code Verifier using the SHA256 algorithm
    /// </summary>
    public string Challenge { get; private set; }

    /// <summary>
    /// Constructor
    /// </summary>
    public VerifierChallenge()
    {
        Verifier = GetVerifier();
        Challenge = GetChallenge();
    }
}