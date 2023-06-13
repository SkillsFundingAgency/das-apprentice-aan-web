using System.Security.Cryptography;
using System.Text;
using SFA.DAS.ApprenticeAan.Domain.Interfaces;
using SFA.DAS.ApprenticeAan.Web.Configuration;

namespace SFA.DAS.ApprenticeAan.Web.Services;

public class GoogleMapsService : IGoogleMapsService
{
    public const string GoogleMapsUrl = @"https://www.google.com/maps/dir//";
    private readonly string _googleMapsPrivateKey;
    private readonly string _googleMapApiKey;

    public GoogleMapsService(ApplicationConfiguration applicationConfiguration)
    {
        _googleMapApiKey = applicationConfiguration.ApplicationSettings.GoogleMapsApiKey;
        _googleMapsPrivateKey = applicationConfiguration.ApplicationSettings.GoogleMapsPrivateKey;
    }

    public string GetStaticMapUrl(double latitude, double longitude)
    {
        return new GoogleApiStaticMapsUrlBuilder()
                .AddSize("190x125")
                .AddZoomLevel("12")
                .AddLocation(latitude, longitude)
                .AddApiKey(_googleMapApiKey)
                .BuildWithSignature(_googleMapsPrivateKey);
    }

    public string GetFullMapUrl(double latitude, double longitude)
    {
        return $"{GoogleMapsUrl}{latitude},{longitude}";
    }
}

public class GoogleApiStaticMapsUrlBuilder
{
    private const string DefaultBaseUrl = "https://maps.googleapis.com/maps/api/staticmap";
    private readonly Dictionary<string, string> _queryParameters = new Dictionary<string, string>();
    private readonly string _baseUrl;
    public GoogleApiStaticMapsUrlBuilder(string baseUrl = DefaultBaseUrl)
    {
        _baseUrl = baseUrl;
    }
    public GoogleApiStaticMapsUrlBuilder AddSize(string size)
    {
        AddOrReplaceKeyValue(nameof(size), size);
        return this;
    }
    public GoogleApiStaticMapsUrlBuilder AddZoomLevel(string zoom)
    {
        AddOrReplaceKeyValue(nameof(zoom), zoom);
        return this;
    }
    public GoogleApiStaticMapsUrlBuilder AddApiKey(string key)
    {
        AddOrReplaceKeyValue(nameof(key), key);
        return this;
    }
    public GoogleApiStaticMapsUrlBuilder AddLocation(double latitude, double longitude)
    {
        AddOrReplaceKeyValue("markers", $"{latitude},{longitude}");
        return this;
    }
    public string BuildWithSignature(string privateKey)
    {
        var url = BuildUrlWithParameters();
        url = AddSignatureToUrl(url, privateKey);
        return url;
    }
    private void AddOrReplaceKeyValue(string key, string value)
    {
        if (_queryParameters.ContainsKey(key))
        {
            _queryParameters[key] = value;
        }
        else
        {
            _queryParameters.Add(key, value);
        }
    }
    private string BuildUrlWithParameters()
    {
        var builder = new StringBuilder();
        builder.Append(_baseUrl);
        foreach (var x in _queryParameters.Select((Entry, Index) => new { Entry, Index }))
        {
            builder.Append(x.Index == 0 ? "?" : "&");
            builder.Append(x.Entry.Key);
            builder.Append("=");
            builder.Append(x.Entry.Value);
        }
        return builder.ToString();
    }

    private string AddSignatureToUrl(string url, string privateKey)
    {
        var encoding = new ASCIIEncoding();

        // converting key to bytes will throw an exception, need to replace '-' and '_' characters first.
        string usablePrivateKey = privateKey.Replace("-", "+").Replace("_", "/");
        byte[] privateKeyBytes = Convert.FromBase64String(usablePrivateKey);

        Uri uri = new Uri(url);
        byte[] encodedPathAndQueryBytes = encoding.GetBytes(uri.LocalPath + uri.Query);

        // compute the hash
        HMACSHA1 algorithm = new HMACSHA1(privateKeyBytes);
        byte[] hash = algorithm.ComputeHash(encodedPathAndQueryBytes);

        // convert the bytes to string and make url-safe by replacing '+' and '/' characters
        string signature = Convert.ToBase64String(hash).Replace("+", "-").Replace("/", "_");

        // Add the signature to the existing URI.
        return uri.Scheme + "://" + uri.Host + uri.LocalPath + uri.Query + "&signature=" + signature;
    }
}
