#pragma warning disable CS1591
namespace OYMLCN.Encrypt
{
    public enum MD5Length
    {
        L16 = 16,
        L32 = 32
    }
    public enum RsaSize
    {
        R2048 = 2048,
        R3072 = 3072,
        R4096 = 4096
    }
    public enum RsaKeyType
    {
        XML,
        JSON
    }
    public class AESKey
    {
        public string Key { get; set; }
        public string IV { get; set; }
    }
    public class RSAKey
    {
        public string PublicKey { get; set; }
        public string PrivateKey { get; set; }
        public string Exponent { get; set; }
        public string Modulus { get; set; }
    }
    internal class RSAParametersJson
    {
        public string Modulus { get; set; }
        public string Exponent { get; set; }
        public string P { get; set; }
        public string Q { get; set; }
        public string DP { get; set; }
        public string DQ { get; set; }
        public string InverseQ { get; set; }
        public string D { get; set; }
    }
}
