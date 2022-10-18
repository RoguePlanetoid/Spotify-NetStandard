namespace Spotify.NetStandard.Client.Internal;

/// <summary>
/// Spotify Client
/// </summary>
internal class SpotifyClient : SimpleServiceClient, ISpotifyClient
{
    private static readonly Uri _hostName = new("https://api.spotify.com");
    // Members
    private static ISpotifyApi _api;
    private readonly AuthenticationCache _authenticationCache;

    /// <summary>
    /// Format Request Headers
    /// </summary>
    /// <param name="tokenType">Authentication Token Type</param>
    /// <returns>Dictionary of Headers</returns>
    private async Task<Dictionary<string, string>> FormatRequestHeadersAsync(
        TokenType tokenType = TokenType.Access)
    {
        var headers = new Dictionary<string, string>();
        var access = await
            _authenticationCache.CheckAndRenewTokenAsync(
            tokenType, new CancellationToken(false));
        headers.Add("Authorization", $"Bearer {access.Token}");
        return headers;
    }

    /// <summary>
    /// Format Request Parameters
    /// </summary>
    /// <param name="limit">Limit</param>
    /// <param name="offset">Offset</param>
    /// <param name="country">Country</param>
    /// <param name="locale">Locale</param>
    /// <param name="fields">Fields</param>
    /// <param name="market">Market</param>
    /// <returns>Dictionary of Request Parameters</returns>
    private Dictionary<string, string> FormatRequestParameters(
        int? limit = null,
        int? offset = null,
        string country = null,
        string locale = null,
        string fields = null,
        string market = null)
    {
        var parameters = new Dictionary<string, string>();
        if (limit != null)
            parameters.Add(nameof(limit), $"{limit.Value}");
        if (offset != null)
            parameters.Add(nameof(offset), $"{offset.Value}");
        if (!string.IsNullOrEmpty(country))
            parameters.Add(nameof(country), country);
        if (!string.IsNullOrEmpty(locale))
            parameters.Add(nameof(locale), locale);
        if (!string.IsNullOrEmpty(fields))
            parameters.Add(nameof(fields), fields);
        if (!string.IsNullOrEmpty(market))
            parameters.Add(nameof(market), market);
        return parameters;
    }

    /// <summary>
    /// Format Ids Parameter
    /// </summary>
    /// <param name="itemIds">IDs of the Items</param>
    /// <returns>Ids as String</returns>
    private string FormatIdsParameter(List<string> itemIds) => 
        itemIds.Aggregate(string.Empty,
        (current, id) => current +
        (!string.IsNullOrEmpty(current) ? 
        "," : string.Empty) + id);

    /// <summary>
    /// Format Cursor Parameters
    /// </summary>
    /// <param name="cursor">Format Cursor Parameters</param>
    /// <returns>Dictionary of Request Parameters</returns>
    private Dictionary<string, string> FormatCursorParameters(Cursor cursor)
    { 
        if (cursor?.Next != null)
            return new Uri(cursor.Next).Query.QueryStringAsDictionary();
        var parameters = new Dictionary<string, string>();
        if (cursor?.Offset != null)
            parameters.Add("offset", $"{cursor.Offset.Value}");
        if (cursor?.Limit != null)
            parameters.Add("limit", $"{cursor.Limit.Value}");
        if (cursor?.After != null)
            parameters.Add("after", cursor?.After);
        if (cursor?.Before != null)
            parameters.Add("before", cursor?.Before);
        return parameters;
    }

    /// <summary>
    /// Get Status
    /// </summary>
    /// <typeparam name="TResponse"></typeparam>
    /// <param name="response"></param>
    /// <param name="statusCode"></param>
    /// <param name="successCode"></param>
    private TResponse GetStatus<TResponse>(
        TResponse response,
        HttpStatusCode statusCode,
        HttpStatusCode successCode)
        where TResponse : Status
    {
        var success = statusCode == successCode;
        response ??= (TResponse)new Status();
        response.StatusCode = statusCode;
        response.Success = success;
        return response;
    }

    /// <summary>
    /// Browse
    /// </summary>
    /// <param name="browseCategory">Category</param>
    /// <param name="country">Country</param>
    /// <param name="locale">Locale</param>
    /// <param name="parameters">Parameters</param>
    /// <param name="page">Page</param>
    /// <returns>Content Response</returns>
    private async Task<ContentResponse> GetBrowseAsync(
        string browseCategory,
        string country,
        string locale = null,
        Dictionary<string, string> parameters = null,
        Page page = null)
    {
        var headers = await FormatRequestHeadersAsync();
        var requestParameters = FormatRequestParameters(
            country: country, 
            locale: locale,
            offset: page?.Offset, 
            limit: page?.Limit);
        if (parameters != null)
            foreach (KeyValuePair<string, string> item in parameters)
                requestParameters.Add(item.Key, item.Value);
        return await GetRequestAsync<ContentResponse>(
            _hostName,
            $"/v1/browse/{browseCategory}",
            new CancellationToken(false),
            requestParameters, 
            headers);
    }

    /// <summary>
    /// Lookup API
    /// </summary>
    /// <typeparam name="TResult">Result Type</typeparam>
    /// <param name="itemId">Spotify ID of the Item</param>
    /// <param name="lookupType">Lookup Type</param>
    /// <param name="market">Market</param>
    /// <param name="country">Country</param>
    /// <param name="fields">Fields</param>
    /// <param name="key">Parameter Key</param>
    /// <param name="value">Parameter Value</param>
    /// <param name="page">Page Offset and Limit</param>
    /// <returns>Response of Type</returns>
    private async Task<TResult> LookupApiAsync<TResult>(
        string itemId,
        string lookupType = null,
        string market = null,
        string country = null,
        string fields = null,
        string key = null,
        string value = null,
        Page page = null)
        where TResult : class
    {
        var headers = await FormatRequestHeadersAsync();
        var parameters = FormatRequestParameters(
            offset: page?.Offset, 
            limit: page?.Limit, 
            market: market, 
            country: country, 
            fields: fields);
        if (key != null && value != null)
            parameters.Add(key, value);
        var source = new string[] { lookupType };
        source = source[0].Contains("_") ?
            source[0].Split('_') :
            source;
        var relativeUri = (source.Length == 1) ?
            $"/v1/{source[0]}/{itemId}" :
            $"/v1/{source[0]}/{itemId}/{source[1]}";
        return await GetRequestAsync<TResult>(
            _hostName, 
            relativeUri,
            new CancellationToken(false),
            parameters, 
            headers);
    }

    /// <summary>
    /// Lookup API
    /// </summary>
    /// <param name="itemIds">Spotify IDs of the Items</param>
    /// <param name="lookupType">Lookup Type</param>
    /// <param name="market">Market</param>
    /// <param name="page">Page Offset and Limit</param>
    /// <returns>Lookup Response</returns>
    private async Task<LookupResponse> LookupApiAsync(
        List<string> itemIds,
        string lookupType = null,
        string market = null,
        Page page = null)
    {
        var headers = await FormatRequestHeadersAsync();
        var parameters = FormatRequestParameters(
            offset: page?.Offset, 
            limit: page?.Limit, 
            market: market);
        var ids = FormatIdsParameter(itemIds);
        parameters.Add("ids", ids);
        return await GetRequestAsync<LookupResponse>(
            _hostName, 
            $"/v1/{lookupType}/",
            new CancellationToken(false), 
            parameters, 
            headers);
    }

    /// <summary>
    /// Lookup Cursor API
    /// </summary>
    /// <typeparam name="TResult">Result Type</typeparam>
    /// <param name="lookupType">Lookup Type</param>
    /// <param name="key">Key</param>
    /// <param name="value">Value</param>
    /// <param name="cursor">Cursor Limit and After</param>
    /// <param name="tokenType">Token Type</param>
    /// <returns>Response Object</returns>
    private async Task<TResult> LookupCursorApiAsync<TResult>(
        string lookupType = null,
        string key = null,
        string value = null,
        Cursor cursor = null,
        TokenType tokenType = TokenType.Access)
        where TResult : class
    {
        var headers = await FormatRequestHeadersAsync(tokenType);
        var parameters = FormatCursorParameters(cursor);
        if (key != null && value != null && !parameters.ContainsKey(key)) 
            parameters.Add(key, value);
        var source = new string[] { lookupType };
        source = source[0].Contains("_") ?
            source[0].Split('_') :
            source;
        var relativeUri = (source.Length == 1) ?
            $"/v1/{source[0]}/" :
            $"/v1/{source[0]}/{source[1]}";
        return await GetRequestAsync<TResult>(
            _hostName, 
            relativeUri,
            new CancellationToken(false),
            parameters, headers);
    }

    /// <summary>
    /// Get API
    /// </summary>
    /// <typeparam name="TResponse">Response Type</typeparam>
    /// <param name="itemId">Spotify ID of the Item</param>
    /// <param name="requestType">Request Type</param>
    /// <param name="parameters">Request Parameters</param>
    /// <param name="tokenType">Token Type</param>
    /// <returns>Response Object</returns>
    private async Task<TResponse> GetApiAsync<TResponse>(
        string itemId = null,
        string requestType = null,
        Dictionary<string, string> parameters = null,
        TokenType tokenType = TokenType.Access)
        where TResponse : class
    {
        var headers = await FormatRequestHeadersAsync(tokenType);
        var relativeUri = (itemId == null) ?
            $"/v1/{requestType}" :
            $"/v1/{requestType}/{itemId}";
        return await GetRequestAsync<TResponse>(
            _hostName, 
            relativeUri,
            new CancellationToken(false), 
            parameters, 
            headers);
    }

    /// <summary>
    /// Get Bools
    /// </summary>
    /// <param name="itemIds">Spotify IDs of the Items</param>
    /// <param name="requestType">Request Type</param>
    /// <param name="parameters">Request Parameters</param>
    /// <param name="tokenType">Token Type</param>
    /// <returns>Response Object</returns>
    private async Task<Bools> GetBoolsAsync(
        List<string> itemIds,
        string requestType,
        Dictionary<string, string> parameters = null,
        TokenType tokenType = TokenType.Access)
    {
        var headers = await FormatRequestHeadersAsync(tokenType);
        if (itemIds != null)
        {
            if (parameters == null)
                parameters = new Dictionary<string, string>();
            parameters.Add("ids", FormatIdsParameter(itemIds));
        }
        var response = await GetRequestWithErrorAsync<Bools, InternalResponse>(
            _hostName, 
            $"/v1/{requestType}",
            new CancellationToken(false),
            parameters, 
            headers);
        return response.Result ?? new Bools(response?.ErrorResult?.Error);
    }

    /// <summary>
    /// Post API Status
    /// </summary>
    /// <typeparam name="TRequest">Request Type</typeparam>
    /// <typeparam name="TResponse">Response Type</typeparam>
    /// <param name="requestType">Request Type</param>
    /// <param name="request">Request</param>
    /// <param name="body">Request Body</param>
    /// <param name="parameters">Request Parameters</param>
    /// <param name="tokenType">Token Type</param>
    /// <param name="successCode">Success Code</param>
    /// <returns>Response Object</returns>
    private async Task<TResponse> PostApiStatusAsync<TRequest, TResponse>(
        string requestType = null,
        TRequest request = null,
        Dictionary<string, string> body = null,
        Dictionary<string, string> parameters = null,
        TokenType tokenType = TokenType.Access,
        HttpStatusCode successCode = HttpStatusCode.OK)
        where TRequest : class
        where TResponse : Status
    {
        var headers = await FormatRequestHeadersAsync(tokenType);
        var relativeUri = $"/v1/{requestType}";
        var (response, statusCode) =
            await PostRequestAsync<TRequest, TResponse>(
            _hostName,
            relativeUri,
            request,
            new CancellationToken(false),
            body,
            parameters,
            headers);
        return GetStatus(response, statusCode, successCode);
    }

    /// <summary>
    /// Post API
    /// </summary>
    /// <typeparam name="TRequest">Request Type</typeparam>
    /// <typeparam name="TResponse">Response Type</typeparam>
    /// <param name="requestType">Request Type</param>
    /// <param name="request">Request Object</param>
    /// <param name="body">Request Body</param>
    /// <param name="parameters">Request Parameters</param>
    /// <param name="tokenType">Token Type</param>
    /// <returns>Response Object</returns>
    private async Task<TResponse> PostApiAsync<TRequest, TResponse>(
        string requestType = null,
        TRequest request = null,
        Dictionary<string, string> body = null,
        Dictionary<string, string> parameters = null,
        TokenType tokenType = TokenType.Access)
        where TRequest : class
        where TResponse : class
    {
        var headers = await FormatRequestHeadersAsync(tokenType);
        var relativeUri = $"/v1/{requestType}";
        var (response, _) =
            await PostRequestAsync<TRequest, TResponse>(
            _hostName,
            relativeUri,
            request,
            new CancellationToken(false),
            body,
            parameters,
            headers);
        return response;
    }

    /// <summary>
    /// Put API
    /// </summary>
    /// <typeparam name="TRequest">Request Type</typeparam>
    /// <typeparam name="TResponse">Response Type</typeparam>
    /// <param name="tokenType">Auth Type</param>
    /// <param name="itemIds">Spotify IDs of the Items</param>
    /// <param name="requestType">Request Type</param>
    /// <param name="request">Request Object</param>
    /// <param name="fileBytes">File Bytes</param>
    /// <param name="parameters">Request Parameters</param>
    /// <param name="successCode">Success Code</param>
    /// <returns>Response Object</returns>
    private async Task<TResponse> PutApiAsync<TRequest, TResponse>(
        List<string> itemIds = null,
        string requestType = null,
        TRequest request = null,
        byte[] fileBytes = null,
        Dictionary<string, string> parameters = null,
        TokenType tokenType = TokenType.Access,
        HttpStatusCode successCode = HttpStatusCode.OK)
        where TRequest : class
        where TResponse : Status
    {
        var headers = await FormatRequestHeadersAsync(tokenType);
        if (itemIds != null)
        {
            if (parameters == null)
                parameters = new Dictionary<string, string>();
            parameters.Add("ids", FormatIdsParameter(itemIds));
        }
        var (response, statusCode) =
            await PutRequestAsync<TRequest, TResponse>(
            _hostName,
            $"/v1/{requestType}",
            request,
            new CancellationToken(false),
            fileBytes,
            parameters,
            headers);
        return GetStatus(response, statusCode, successCode);
    }

    /// <summary>
    /// Delete API
    /// </summary>
    /// <typeparam name="TRequest">Request Type</typeparam>
    /// <typeparam name="TResponse">Response Type</typeparam>
    /// <param name="tokenType">Auth Type</param>
    /// <param name="itemIds">Spotify IDs of the Items</param>
    /// <param name="requestType">Request Type</param>
    /// <param name="request">Request Object</param>
    /// <param name="parameters">Request Parameters</param>
    /// <param name="successCode">Success Code</param>
    /// <returns>Response Object</returns>
    private async Task<TResponse> DeleteApiAsync<TRequest, TResponse>(
        List<string> itemIds = null,
        string requestType = null,
        TRequest request = null,
        Dictionary<string, string> parameters = null,
        TokenType tokenType = TokenType.Access,
        HttpStatusCode successCode = HttpStatusCode.OK)
        where TRequest : class
        where TResponse : Status
    {
        var headers = await FormatRequestHeadersAsync(tokenType);
        if (itemIds != null)
        {
            if (parameters == null)
                parameters = new Dictionary<string, string>();
            parameters.Add("ids", FormatIdsParameter(itemIds));
        }
        var (response, statusCode) =
            await DeleteRequestAsync<TRequest, TResponse>(
            _hostName,
            $"/v1/{requestType}",
            request,
            new CancellationToken(false),
            parameters,
            headers);
        return GetStatus(response, statusCode, successCode);
    }

    /// <summary>
    /// Get Response
    /// </summary>
    /// <typeparam name="TResponse">Response Type</typeparam>
    /// <param name="hostname">Hostname</param>
    /// <param name="endpoint">Endpoint</param>
    /// <param name="parameters">Parameters</param>
    /// <param name="tokenType">Token Type</param>
    /// <returns>Response</returns>
    private async Task<TResponse> GetResponseAsync<TResponse>(
        string hostname, string endpoint,
        Dictionary<string, string> parameters, 
        TokenType tokenType)
        where TResponse : class =>
            await GetRequestAsync<TResponse>(
            new Uri(hostname),
            endpoint,
            new CancellationToken(false),
            parameters,
            await FormatRequestHeadersAsync(tokenType));

    /// <summary>
    /// Get Response
    /// </summary>
    /// <typeparam name="TResponse">Response Type</typeparam>
    /// <param name="source">Source Uri</param>
    /// <param name="tokenType">Token Type</param>
    /// <returns>Response</returns>
    private async Task<TResponse> GetResponseAsync<TResponse>(
        Uri source, 
        TokenType tokenType)
        where TResponse : class =>
            await GetRequestAsync<TResponse>(
            new Uri($"{source.Scheme}://{source.Host}"),
            source.AbsolutePath,
            new CancellationToken(false),
            source.Query.QueryStringAsDictionary(),
            await FormatRequestHeadersAsync(tokenType));

    /// <summary>
    /// Has Fields Parameter
    /// </summary>
    /// <param name="lookupType">Lookup Type</param>
    /// <returns>True if LookupType Supports Fields Parameter</returns>
    private bool HasFieldsParameter(LookupType lookupType) =>
        lookupType == LookupType.Playlist || 
        lookupType == LookupType.PlaylistTracks;

    /// <summary>
    /// Spotify Client
    /// </summary>
    /// <param name="authenticationCache">Authentication Cache</param>
    internal SpotifyClient(AuthenticationCache authenticationCache) =>
        (_api, _authenticationCache) = (new SpotifyApi(this), authenticationCache);

    /// <summary>
    /// Spotify Client
    /// </summary>
    /// <param name="authenticationCache">Authentication Cache</param>
    /// <param name="httpClient">Http Client</param>
    internal SpotifyClient(AuthenticationCache authenticationCache, HttpClient httpClient) =>
        (_api, _authenticationCache, HttpClient) = (new SpotifyApi(this), authenticationCache, httpClient);

    #region Public Properties
    /// <summary>
    /// Spotify API
    /// </summary>
    public ISpotifyApi Api => _api;
    #endregion Public Properties

    #region Public Methods
    /// <summary>
    /// Get Access Token
    /// </summary>
    /// <returns>Access Token</returns>
    public AccessToken GetToken() =>
        _authenticationCache.AccessToken;

    /// <summary>
    /// Set Access Token
    /// </summary>
    /// <param name="value">Access Token</param>
    public void SetToken(AccessToken value) =>
        _authenticationCache.AccessToken = value;

    /// <summary>
    /// Refresh Token
    /// </summary>
    /// <returns>Access Token</returns>
    public async Task<AccessToken> RefreshToken() => 
        await _authenticationCache.CheckAndRenewTokenAsync(
            _authenticationCache?.AccessToken?.TokenType ?? 
            TokenType.Access, 
            new CancellationToken());

    /// <summary>
    /// Refresh Token
    /// </summary>
    /// <param name="value">Access Token</param>
    /// <returns>Access Token</returns>
    public async Task<AccessToken> RefreshToken(AccessToken value) => 
        await _authenticationCache.GetRefreshTokenAsync(
            value.Refresh, value?.TokenType ?? 
            TokenType.Access, 
            new CancellationToken());

    /// <summary>
    /// Authenticated Get
    /// </summary>
    /// <typeparam name="TResponse">Response Type</typeparam>
    /// <param name="hostname">Hostname</param>
    /// <param name="endpoint">Endpoint</param>
    /// <param name="parameters">Parameters</param>
    /// <returns>Response</returns>
    public async Task<TResponse> AuthGetAsync<TResponse>(
        string hostname, string endpoint,
        Dictionary<string, string> parameters)
        where TResponse : class =>
            await GetResponseAsync<TResponse>(
                hostname, 
                endpoint, 
                parameters, 
                TokenType.User);

    /// <summary>
    /// Authenticated Get
    /// </summary>
    /// <typeparam name="TResponse">Response Type</typeparam>
    /// <param name="source">Source Uri</param>
    /// <returns>Response</returns>
    public async Task<TResponse> AuthGetAsync<TResponse>(Uri source)
        where TResponse : class =>
        await GetResponseAsync<TResponse>(
            source, 
            TokenType.User);

    /// <summary>
    /// Authenticated Navigate 
    /// </summary>
    /// <typeparam name="TResponse">Response Type</typeparam>
    /// <param name="cursor">Cursor Object</param>
    /// <param name="navigateType">Navigate Type</param>
    /// <returns>Content Response</returns>
    /// <exception cref="AuthUserTokenRequiredException"></exception>
    public async Task<CursorPaging<TResponse>> AuthNavigateAsync<TResponse>(
        CursorPaging<TResponse> cursor,
        NavigateType navigateType)
    {
        Uri source = null;
        switch (navigateType)
        {
            case NavigateType.None:
                if(cursor.Href != null)
                    source = new Uri(cursor.Href);
                break;
            case NavigateType.Previous:
                if (cursor.Before != null)
                    source = new Uri(cursor.Before);
                break;
            case NavigateType.Next:
                if (cursor.Next != null)
                    source = new Uri(cursor.Next);
                break;
        }
        return source != null ? 
            await AuthGetAsync<CursorPaging<TResponse>>(source) : null;
    }

    /// <summary>
    /// Paging
    /// </summary>
    /// <typeparam name="TResponse">Response Type</typeparam>
    /// <param name="paging">Paging</param>
    /// <param name="navigateType">Navigate Type</param>
    /// <returns>Content Response</returns>
    /// <exception cref="AuthAccessTokenRequiredException"></exception>
    public async Task<Paging<TResponse>> PagingAsync<TResponse>(
        Paging<TResponse> paging,
        NavigateType navigateType)
    {
        Uri source = null;
        switch (navigateType)
        {
            case NavigateType.None:
                if (paging.Href != null)
                    source = new Uri(paging.Href);
                break;
            case NavigateType.Previous:
                if (paging.Previous != null)
                    source = new Uri(paging.Previous);
                break;
            case NavigateType.Next:
                if (paging.Next != null)
                    source = new Uri(paging.Next);
                break;
        }
        return source != null ?
            await GetAsync<Paging<TResponse>>(source) : null;
    }

    /// <summary>
    /// Get
    /// </summary>
    /// <typeparam name="TResponse">Response Type</typeparam>
    /// <param name="hostname">Hostname</param>
    /// <param name="endpoint">Endpoint</param>
    /// <param name="parameters">Parameters</param>
    /// <returns>Response</returns>
    public async Task<TResponse> GetAsync<TResponse>(
        string hostname, string endpoint,
        Dictionary<string, string> parameters)
        where TResponse : class => 
            await GetResponseAsync<TResponse>(
                hostname, 
                endpoint, 
                parameters, 
                TokenType.Access);

    /// <summary>
    /// Get
    /// </summary>
    /// <typeparam name="TResponse">Response Type</typeparam>
    /// <param name="source">Source Uri</param>
    /// <returns>Response</returns>
    public async Task<TResponse> GetAsync<TResponse>(Uri source) 
        where TResponse : class =>
        await GetResponseAsync<TResponse>(
            source, 
            TokenType.Access);

    /// <summary>
    /// Navigate 
    /// </summary>
    /// <typeparam name="TResponse">Response Type</typeparam>
    /// <param name="paging">Paging Object</param>
    /// <param name="navigateType">Navigate Type</param>
    /// <returns>Content Response</returns>
    /// <exception cref="AuthAccessTokenRequiredException"></exception>
    public async Task<ContentResponse> NavigateAsync<TResponse>(
        Paging<TResponse> paging,
        NavigateType navigateType)
    {
        Uri source = null;
        switch (navigateType)
        {
            case NavigateType.None:
                if (paging.Href != null)
                    source = new Uri(paging.Href);
                break;
            case NavigateType.Previous:
                if (paging.Previous != null)
                    source = new Uri(paging.Previous);
                break;
            case NavigateType.Next:
                if (paging.Next != null)
                    source = new Uri(paging.Next);
                break;
        }
        return source != null ? 
            await GetAsync<ContentResponse>(source) : null;
    }

    /// <summary>
    /// Search
    /// </summary>
    /// <param name="query">(Required) Search Query</param>
    /// <param name="searchType">(Required) Search results include hits from all the specified item types.</param>
    /// <param name="market">(Optional) An ISO 3166-1 alpha-2 country code or the string from_token</param>
    /// <param name="external">(Optional) Include any relevant audio content that is hosted externally. </param>
    /// <param name="page">(Optional) Limit: The maximum number of items to return - Offset: The index of the first item to return</param>
    /// <returns>Content Response</returns>
    /// <exception cref="AuthAccessTokenRequiredException"></exception>
    public async Task<ContentResponse> SearchAsync(
        string query,
        SearchType searchType,
        string market = null,
        bool? external = null,
        Page page = null)
    {
        Dictionary<string, string> headers =
            await FormatRequestHeadersAsync();
        Dictionary<string, string> parameters =
            FormatRequestParameters(
                offset: page?.Offset, limit: page?.Limit, market: market);
        if (searchType != null)
            parameters.Add("type", searchType.Get()?.AsDelimitedString());
        if (!string.IsNullOrEmpty(query))
            parameters.Add("q", Uri.EscapeDataString(query));
        if (external == true)
            parameters.Add("include_external", "audio");
        return await GetRequestAsync<ContentResponse>(
            _hostName, 
            "/v1/search/",
            new CancellationToken(false),
            parameters, 
            headers);
    }

    /// <summary>
    /// Lookup
    /// </summary>
    /// <typeparam name="TResponse">Response Type</typeparam>
    /// <param name="itemId">(Required) The Spotify ID for the album.</param>
    /// <param name="lookupType">(Required) Item Type</param>
    /// <param name="market">(Optional) ISO 3166-1 alpha-2 country code or the string from_token</param>
    /// <param name="country">(Optional) ISO 3166-1 alpha-2 country code or the string from_token</param>
    /// <param name="fields">(Optional) Filters for the query: a comma-separated list of the fields to return for Playlist and PlaylistTracks LookupType if omitted, all fields are returned</param>
    /// <param name="key">(Optional) Query Parameter Key</param>
    /// <param name="value">(Optional) Query Parameter Value</param>
    /// <param name="page">(Optional) Limit: The maximum number of items to return - Offset: The index of the first item to return</param>
    /// <returns>Lookup Response by Type</returns>
    /// <exception cref="AuthAccessTokenRequiredException"></exception>
    public async Task<TResponse> LookupAsync<TResponse>(
        string itemId,
        LookupType lookupType,
        string market = null,
        string country = null,
        string fields = null,
        string key = null,
        string value = null,
        Page page = null)
        where TResponse : class =>
            await LookupApiAsync<TResponse>(
            itemId: itemId,
            lookupType: lookupType.GetDescription(),
            market: market,
            country: country,
            fields: HasFieldsParameter(lookupType) ? fields : null,
            key: key,
            value: value,
            page: page);

    /// <summary>
    /// Lookup
    /// </summary>
    /// <param name="itemIds">(Required) List of Spotify ID for the items</param>
    /// <param name="lookupType">(Required) Lookup Item Type</param>
    /// <param name="market">(Optional) ISO 3166-1 alpha-2 country code or the string from_token</param>
    /// <param name="page">(Optional) Limit: The maximum number of items to return - Offset: The index of the first item to return</param>
    /// <returns>Lookup Response</returns>
    /// <exception cref="AuthAccessTokenRequiredException"></exception>
    public async Task<LookupResponse> LookupAsync(
        List<string> itemIds,
        LookupType lookupType,
        string market = null,
        Page page = null) => 
            await LookupApiAsync(
                itemIds: itemIds, 
                lookupType: lookupType.GetDescription(),
                market: market, 
                page: page);

    /// <summary>
    /// Lookup Featured Playlists
    /// </summary>
    /// <param name="country">(Optional) A country: an ISO 3166-1 alpha-2 country code. </param>
    /// <param name="locale">(Optional) The desired language, consisting of a lowercase ISO 639-1 language code and an uppercase ISO 3166-1 alpha-2 country code, joined by an underscore</param>
    /// <param name="timestamp">(Optional) Use this parameter to specify the user’s local time to get results tailored for that specific date and time in the day.</param>
    /// <param name="page">(Optional) Limit: The maximum number of items to return. Default: 20. Minimum: 1. Maximum: 50. - Offset: The index of the first item to return. Default: 0</param>
    /// <returns>Content Response</returns>
    /// <exception cref="AuthAccessTokenRequiredException"></exception>
    public async Task<ContentResponse> LookupFeaturedPlaylistsAsync(
        string country = null,
        string locale = null,
        DateTime? timestamp = null,
        Page page = null)
    {
        Dictionary<string, string> parameters =
            new Dictionary<string, string>();
        if (timestamp != null)
            parameters.Add(nameof(timestamp), timestamp.Value.ToString("yyyy-MM-ddTHH:mm:ss"));
        return await GetBrowseAsync(
            browseCategory: "featured-playlists", 
            country: country, 
            locale: locale,
            parameters: parameters, 
            page: page);
    }

    /// <summary>
    /// Lookup New Releases
    /// </summary>
    /// <param name="country">(Optional) A country: an ISO 3166-1 alpha-2 country code. </param>
    /// <param name="page">(Optional) Limit: The maximum number of items to return. Default: 20. Minimum: 1. Maximum: 50. - Offset: The index of the first item to return. Default: 0</param>
    /// <returns>Content Response</returns>
    /// <exception cref="AuthAccessTokenRequiredException"></exception>
    public async Task<ContentResponse> LookupNewReleasesAsync(
        string country = null,
        Page page = null) => 
            await GetBrowseAsync(
                browseCategory: "new-releases", 
                country: country,
                page: page);

    /// <summary>
    /// Lookup Artist's Albums
    /// </summary>
    /// <param name="itemId">(Required) The Spotify ID for the artist.</param>
    /// <param name="includeGroup">(Optional) Filters the response. If not supplied, all album types will be returned</param>
    /// <param name="market">(Optional) An ISO 3166-1 alpha-2 country code</param>
    /// <param name="page">(Optional) Limit: The number of album objects to return. Default: 20. Minimum: 1. Maximum: 50 - Offset: The index of the first album to return. Default: 0 (i.e., the first album).</param>
    /// <returns>Paging List of Album</returns>
    /// <exception cref="AuthAccessTokenRequiredException"></exception>
    public async Task<Paging<Album>> LookupArtistAlbumsAsync(
        string itemId,
        IncludeGroup includeGroup = null,
        string market = null,
        Page page = null)
    {
        string key = null;
        string value = null;
        if (includeGroup != null)
        {
            key = "include_groups";
            value = includeGroup.Get()?.AsDelimitedString();
        }
        return await LookupAsync<Paging<Album>>(
            itemId: itemId, 
            lookupType: LookupType.ArtistAlbums, 
            market: market,
            key: key, 
            value: value, 
            page: page);
    }

    /// <summary>
    /// Lookup Artist's Top Tracks
    /// </summary>
    /// <param name="itemId">(Required) The Spotify ID for the artist.</param>
    /// <param name="market">(Required) A country: an ISO 3166-1 alpha-2 country code or the string from_token</param>
    /// <returns>Lookup Response</returns>
    /// <exception cref="AuthAccessTokenRequiredException"></exception>
    public async Task<LookupResponse> LookupArtistTopTracksAsync(
        string itemId,
        string market) => 
            await LookupApiAsync<LookupResponse>(
                itemId: itemId, 
                lookupType: "artists_top-tracks",
                market: market);

    /// <summary>
    /// Lookup Artist's Related Artists
    /// </summary>
    /// <param name="itemId">(Required) The Spotify ID for the artist.</param>
    /// <returns>Lookup Response</returns>
    /// <exception cref="AuthAccessTokenRequiredException"></exception>
    public async Task<LookupResponse> LookupArtistRelatedArtistsAsync(
        string itemId) => 
            await LookupApiAsync<LookupResponse>(
                itemId: itemId,
                lookupType: "artists_related-artists");

    /// <summary>
    /// Lookup All Categories
    /// </summary>
    /// <param name="country">(Optional) A country: an ISO 3166-1 alpha-2 country code. </param>
    /// <param name="locale">(Optional) The desired language, consisting of a lowercase ISO 639-1 language code and an uppercase ISO 3166-1 alpha-2 country code, joined by an underscore</param>
    /// <param name="page">(Optional) Limit: The maximum number of categories to return. Default: 20. Minimum: 1. Maximum: 50. - Offset: The index of the first item to return. Default: 0</param>
    /// <returns>Content Response</returns>
    /// <exception cref="AuthAccessTokenRequiredException"></exception>
    public async Task<ContentResponse> LookupAllCategoriesAsync(
        string country = null,
        string locale = null,
        Page page = null) => 
            await GetBrowseAsync(
                browseCategory: "categories", 
                country: country, 
                locale: locale, 
                page: page);

    /// <summary>
    /// Lookup Category 
    /// </summary>
    /// <param name="categoryId">The Spotify category ID for the category.</param>
    /// <param name="country">(Optional) A country: an ISO 3166-1 alpha-2 country code. </param>
    /// <param name="locale">(Optional) The desired language, consisting of an ISO 639-1 language code and an ISO 3166-1 alpha-2 country code, joined by an underscore.</param>
    /// <returns>Category Object</returns>
    /// <exception cref="AuthAccessTokenRequiredException"></exception>
    public async Task<Category> LookupCategoryAsync(
        string categoryId,
        string country = null,
        string locale = null)
    {
        string key = null;
        string value = null;
        if (locale != null)
        {
            key = nameof(locale);
            value = locale;
        }
        return await LookupAsync<Category>(
            itemId: categoryId, 
            lookupType: LookupType.Categories, 
            country: country, 
            key: key, 
            value: value);
    }

    /// <summary>
    /// Lookup Recommendations
    /// </summary>
    /// <param name="seedArtists">List of Spotify IDs for seed artists</param>
    /// <param name="seedGenres">List of any genres in the set of available genre seeds</param>
    /// <param name="seedTracks">List of Spotify IDs for a seed track</param>
    /// <param name="limit">The target size of the list of recommended tracks. Default: 20. Minimum: 1. Maximum: 100.</param>
    /// <param name="market">An ISO 3166-1 alpha-2 country code</param>
    /// <param name="minTuneableTrack">Multiple values. For each tunable track attribute, a hard floor on the selected track attribute’s value can be provided</param>
    /// <param name="maxTuneableTrack">Multiple values. For each tunable track attribute, a hard ceiling on the selected track attribute’s value can be provided.</param>
    /// <param name="targetTuneableTrack">Multiple values. For each of the tunable track attributes a target value may be provided.</param>
    /// <returns>Recommendation Response Object</returns>
    /// <exception cref="AuthAccessTokenRequiredException"></exception>
    public async Task<RecommendationsResponse> LookupRecommendationsAsync(
        List<string> seedArtists = null,
        List<string> seedGenres = null,
        List<string> seedTracks = null,
        int? limit = null,
        string market = null,
        TuneableTrack minTuneableTrack = null,
        TuneableTrack maxTuneableTrack = null,
        TuneableTrack targetTuneableTrack = null)
    {
        var parameters = new Dictionary<string, string>();
        if (seedArtists != null)
            parameters.Add("seed_artists",
                seedArtists.ToArray().AsDelimitedString());
        if (seedGenres != null)
            parameters.Add("seed_genres",
                seedGenres.ToArray().AsDelimitedString());
        if (seedTracks != null)
            parameters.Add("seed_tracks",
                seedTracks.ToArray().AsDelimitedString());
        if (limit != null)
            parameters.Add(nameof(limit), $"{limit.Value}");
        if (market != null)
            parameters.Add(nameof(market), market);
        if (minTuneableTrack != null)
            minTuneableTrack.SetParameter(parameters, "min");
        if (maxTuneableTrack != null)
            maxTuneableTrack.SetParameter(parameters, "max");
        if (targetTuneableTrack != null)
            targetTuneableTrack.SetParameter(parameters, "target");
        return await GetApiAsync<RecommendationsResponse>(
            requestType: "recommendations", 
            parameters: parameters);
    }

    /// <summary>
    /// Lookup Recommendation Genres
    /// </summary>
    /// <returns>Available Genre Seeds Object</returns>
    /// <exception cref="AuthAccessTokenRequiredException"></exception>
    public async Task<AvailableGenreSeeds> LookupRecommendationGenres() => 
        await GetApiAsync<AvailableGenreSeeds>(
            requestType: "recommendations/available-genre-seeds");

    /// <summary>
    /// Get a Playlist
    /// </summary>
    /// <param name="playlistId">(Required) The Spotify ID for the playlist.</param>
    /// <param name="market">(Optional) An ISO 3166-1 alpha-2 country code or the string from_token</param>
    /// <param name="fields">(Optional) Filters for the query: a comma-separated list of the fields to return. If omitted, all fields are returned.</param>
    /// <param name="additionalTypes">(Optional) List of item types that your client supports besides the default track type. Valid types are track and episode. An unsupported type in the response is expected to be represented as null value in the item field. This parameter was introduced to allow existing clients to maintain their current behaviour and might be deprecated in the future.</param>
    /// <returns>Playlist Object</returns>
    /// <exception cref="AuthAccessTokenRequiredException"></exception>
    public async Task<Playlist> LookupPlaylistAsync(
                string playlistId,
                string market = null,
                string fields = null,
                List<string> additionalTypes = null)
    {
        string key = null;
        string value = null;
        if (additionalTypes != null)
        {
            key = "additional_types";
            value = additionalTypes?.ToArray()?.AsDelimitedString();
        }
        return await LookupAsync<Playlist>(
            itemId: playlistId,
            lookupType: LookupType.Playlist,
            market: market,
            key: key,
            value: value,
            fields: fields);
    }

    /// <summary>
    /// Get a Playlist's Items
    /// </summary>
    /// <param name="id">(Required) The Spotify ID for the playlist.</param>
    /// <param name="market">(Optional) An ISO 3166-1 alpha-2 country code or the string from_token</param>
    /// <param name="page">(Optional) Limit: The maximum number of items to return. Default: 100. Minimum: 1. Maximum: 100. - Offset: The index of the first item to return. Default: 0</param>
    /// <param name="fields">(Optional) Filters for the query: a comma-separated list of the fields to return. If omitted, all fields are returned.</param>
    /// <param name="additionalTypes">(Optional) List of item types that your client supports besides the default track type. Valid types are track and episode. An unsupported type in the response is expected to be represented as null value in the item field. This parameter was introduced to allow existing clients to maintain their current behaviour and might be deprecated in the future.</param>
    /// <returns>Paging List of Playlist Track Object</returns>
    /// <exception cref="AuthAccessTokenRequiredException"></exception>
    public async Task<Paging<PlaylistTrack>> LookupPlaylistItemsAsync(
        string id,
        string market = null,
        Page page = null,
        string fields = null,
        List<string> additionalTypes = null)
    {
        string key = null;
        string value = null;
        if (additionalTypes != null)
        {
            key = "additional_types";
            value = additionalTypes?.ToArray()?.AsDelimitedString();
        }
        return await LookupAsync<Paging<PlaylistTrack>>(
            id,
            LookupType.PlaylistTracks,
            market: market,
            fields: fields,
            key: key,
            value: value,
            page: page);
    }

    /// <summary>
    /// Get Available Markets
    /// </summary>
    /// <returns>List of Markets where Spotify is available</returns>
    /// <exception cref="AuthAccessTokenRequiredException"></exception>
    public async Task<AvailableMarkets> LookupAvailableMarkets() =>
        await GetApiAsync<AvailableMarkets>(
            requestType: "markets");
    #endregion Public Methods

    #region Authenticate
    /// <summary>
    /// Auth User - Authorisation Code Flow
    /// </summary>
    /// <param name="redirectUri">(Required) Redirect Uri</param>
    /// <param name="state">(Recommended) State used to mitigate cross-site request forgery attacks</param>
    /// <param name="scope">(Optional) Authorisation Scopes</param>
    /// <param name="codeVerifier">Generated Code Verifier for Proof Key For Code Exchange</param>
    /// <param name="showDialog">(Optional) Whether or not to force the user to approve the app again if they’ve already done so.</param>
    /// <returns>Uri</returns>
    public Uri AuthUser(
        Uri redirectUri,
        string state,
        Scope scope,
        out string codeVerifier,
        bool showDialog = false) => 
            _authenticationCache.GetAuthorisationCodeUri(
                redirectUri, state, scope.Get(), showDialog, out codeVerifier);

    /// <summary>
    /// Auth User - Authorisation Code Flow
    /// </summary>
    /// <param name="responseUri">(Required) Response Uri</param>
    /// <param name="redirectUri">(Required) Redirect Uri</param>
    /// <param name="state">(Recommended) State used to mitigate cross-site request forgery attacks</param>
    /// <param name="codeVerifier">Generated Code Verifier for Proof Key For Code Exchange</param>
    /// <returns>AccessToken on Success, Null if Not</returns>
    /// <exception cref="AuthCodeVerifierRequiredException">AuthCodeVerifierRequiredException</exception>
    /// <exception cref="AuthCodeValueException">AuthCodeValueException</exception>
    /// <exception cref="AuthCodeStateException">AuthCodeStateException</exception>
    public async Task<AccessToken> AuthUserAsync(
        Uri responseUri,
        Uri redirectUri,
        string state,
        string codeVerifier) =>
            await _authenticationCache.GetAuthorisationCodeAuthAsync(
                responseUri, redirectUri, state, codeVerifier);

    /// <summary>
    /// Auth - Client Credentials Flow
    /// </summary>
    /// <returns>AccessToken on Success, Null if Not</returns>
    public async Task<AccessToken> AuthAsync() =>
        await _authenticationCache.GetClientCredentialsTokenAsync(
            new CancellationToken(false));

    /// <summary>
    /// Auth User Implicit - Implicit Grant Flow
    /// </summary>
    /// <param name="redirectUri">(Required) Redirect Uri</param>
    /// <param name="state">(Recommended) State used to mitigate cross-site request forgery attacks</param>
    /// <param name="scope">(Optional) Authorisation Scopes</param>
    /// <param name="showDialog">(Optional) Whether or not to force the user to approve the app again if they’ve already done so.</param>
    /// <returns>Uri</returns>
    public Uri AuthUserImplicit(
        Uri redirectUri,
        string state,
        Scope scope,
        bool showDialog = false) =>
            _authenticationCache.GetImplicitGrantAuth(
                redirectUri, state, scope.Get(), showDialog);

    /// <summary>
    /// Auth User Implicit - Implicit Grant Flow
    /// </summary>
    /// <param name="responseUri">(Required) Response Uri</param>
    /// <param name="redirectUri">(Required) Redirect Uri</param>
    /// <param name="state">(Recommended) State used to mitigate cross-site request forgery attacks</param>
    /// <returns>AccessToken on Success, Null if Not</returns>
    /// <exception cref="AuthTokenValueException">AuthCodeValueException</exception>
    /// <exception cref="AuthTokenStateException">AuthCodeStateException</exception>
    public AccessToken AuthUserImplicit(
        Uri responseUri,
        Uri redirectUri,
        string state) =>
            _authenticationCache.GetImplicitGrantAuth(
                responseUri, redirectUri, state);
    #endregion Authenticate

    #region Authenticated Follow API
    /// <summary>
    /// Get Following State for Artists/Users
    /// <para>Scopes: FollowRead</para>
    /// </summary>
    /// <param name="itemIds">(Required) List of the artist or the user Spotify IDs to check.</param>
    /// <param name="followType">(Required) Either artist or user.</param>
    /// <returns>List of true or false values</returns>
    /// <exception cref="AuthUserTokenRequiredException"></exception>
    public async Task<Bools> AuthLookupFollowingStateAsync(
        List<string> itemIds,
        FollowType followType)
    {
        var parameters = new Dictionary<string, string>()
        {
            { "type", followType.GetDescription() }
        };
        return await GetBoolsAsync(
            itemIds: itemIds,
            requestType: "me/following/contains",
            parameters: parameters, 
            tokenType: TokenType.User);
    }

    /// <summary>
    /// Check if Users Follow a Playlist
    /// <para>Scopes: PlaylistReadPrivate</para>
    /// </summary>
    /// <param name="itemIds">(Required) List of Spotify User IDs, the ids of the users that you want to check to see if they follow the playlist. Maximum: 5 ids.</param>
    /// <param name="playlistId">(Required) The Spotify ID of the playlist.</param>
    /// <returns>List of true or false values</returns>
    /// <exception cref="AuthUserTokenRequiredException"></exception>
    public async Task<Bools> AuthLookupUserFollowingPlaylistAsync(
        List<string> itemIds,
        string playlistId) => 
            await GetBoolsAsync(
                itemIds: itemIds,
                requestType: $"playlists/{playlistId}/followers/contains",
                tokenType: TokenType.User);

    /// <summary>
    /// Follow Artists or Users
    /// <para>Scopes: FollowModify</para>
    /// </summary>
    /// <param name="itemIds">(Required) List of the artist or the user Spotify IDs.</param>
    /// <param name="followType">(Required) Either artist or user</param>
    /// <returns>Status Object</returns>
    /// <exception cref="AuthUserTokenRequiredException"></exception>
    public async Task<Status> AuthFollowAsync(
        List<string> itemIds,
        FollowType followType)
    {
        var parameters = new Dictionary<string, string>()
        {
            { "type", followType.GetDescription() }
        };
        return await PutApiAsync<Status, Status>(
            itemIds: itemIds,
            requestType: "me/following",
            parameters: parameters, 
            tokenType: TokenType.User,
            successCode: HttpStatusCode.NoContent);
    }

    /// <summary>
    /// Follow a Playlist
    /// <para>Scopes: FollowModify</para>
    /// </summary>
    /// <param name="playlistId">(Required) The Spotify ID of the playlist. Any playlist can be followed, regardless of its public/private status, as long as you know its playlist ID.</param>
    /// <param name="isPublic">(Optional) Defaults to true. If true the playlist will be included in user’s public playlists, if false it will remain private. To be able to follow playlists privately, the user must have granted the playlist-modify-private scope.</param>
    /// <returns>Status Object</returns>
    /// <exception cref="AuthUserTokenRequiredException"></exception>
    public async Task<Status> AuthFollowPlaylistAsync(
        string playlistId,
        bool isPublic = true)
    {
        var request = new PublicRequest()
        {
            IsPublic = isPublic
        };
        return await PutApiAsync<PublicRequest, Status>(
            requestType: $"playlists/{playlistId}/followers",
            request: request, 
            tokenType: TokenType.User);
    }

    /// <summary>
    /// Get User's Followed Artists
    /// <para>Scopes: FollowRead</para>
    /// </summary>
    /// <param name="cursor">(Optional) Limit: The maximum number of items to return. Default: 20. Minimum: 1. Maximum: 50. - After: The last artist ID retrieved from the previous request.</param>
    /// <returns>CursorPaging of Artist Object</returns>
    /// <exception cref="AuthUserTokenRequiredException"></exception>
    public async Task<CursorPaging<Artist>> AuthLookupFollowedArtistsAsync(
        Cursor cursor = null) => 
            (await LookupCursorApiAsync<ContentCursorResponse>(
                lookupType: "me/following",
                key: "type", 
                value: "artist", 
                cursor: cursor,
                tokenType: TokenType.User))?.Artists;

    /// <summary>
    /// Unfollow Artists or Users
    /// <para>Scopes: FollowModify</para>
    /// </summary>
    /// <param name="itemIds">(Required) List of the artist or the user Spotify IDs.</param>
    /// <param name="followType">(Required) Either artist or user</param>
    /// <returns>Status Object</returns>
    /// <exception cref="AuthUserTokenRequiredException"></exception>
    public async Task<Status> AuthUnfollowAsync(
        List<string> itemIds,
        FollowType followType)
    {
        var parameters = new Dictionary<string, string>()
        {
            { "type", followType.GetDescription() }
        };
        return await DeleteApiAsync<Status, Status>(
            itemIds: itemIds,
            requestType: "me/following",
            parameters: parameters, 
            tokenType: TokenType.User, 
            successCode: HttpStatusCode.NoContent);
    }

    /// <summary>
    /// Unfollow Playlist
    /// <para>Scopes: PlaylistModifyPublic, PlaylistModifyPrivate</para>
    /// </summary>
    /// <param name="playlistId">(Required) The Spotify ID of the playlist that is to be no longer followed.</param>
    /// <returns>Status Object</returns>
    /// <exception cref="AuthUserTokenRequiredException"></exception>
    public async Task<Status> AuthUnfollowPlaylistAsync(
        string playlistId) => 
            await DeleteApiAsync<Status, Status>(
                requestType: $"playlists/{playlistId}/followers",
                tokenType: TokenType.User);
    #endregion Authenticated Follow API

    #region Authenticated Playlists API
    /// <summary>
    /// Add Items to a Playlist
    /// <para>Scopes: PlaylistModifyPublic, PlaylistModifyPrivate</para>
    /// </summary>
    /// <param name="playlistId">(Required) The Spotify ID for the playlist.</param>
    /// <param name="uris">(Optional) List of Spotify URIs to add, can be track or episode URIs</param>
    /// <param name="position">(Optional) The position to insert the tracks, a zero-based index. If omitted, the items will be appended to the playlist. Items are added in the order they are listed.</param>
    /// <returns>Snapshot Object</returns>
    /// <exception cref="AuthUserTokenRequiredException"></exception>
    public async Task<Snapshot> AuthAddTracksToPlaylistAsync(
        string playlistId,
        UriListRequest uris = null,
        int? position = null)
    {
        var parameters = new Dictionary<string, string>();
        if (position != null)
            parameters.Add(nameof(position), $"{position}");
        return await PostApiStatusAsync<UriListRequest, Snapshot>(
            requestType: $"playlists/{playlistId}/tracks",
            request: uris, 
            parameters: parameters, 
            tokenType: TokenType.User, 
            successCode: HttpStatusCode.Created);
    }

    /// <summary>
    /// Remove Items from a Playlist
    /// <para>Scopes: PlaylistModifyPublic, PlaylistModifyPrivate</para>
    /// </summary>
    /// <param name="playlistId">(Required) The Spotify ID for the playlist.</param>
    /// <param name="request">(Optional) Tracks: An array of objects containing Spotify URIs of the tracks or episodes to remove. Snapshot ID : The playlist’s snapshot ID against which you want to make the changes. The API will validate that the specified tracks exist and in the specified positions and make the changes, even if more recent changes have been made to the playlist.</param>
    /// <returns>Snapshot Object</returns>
    /// <exception cref="AuthUserTokenRequiredException"></exception>
    public async Task<Snapshot> AuthRemoveTracksFromPlaylistAsync(
        string playlistId,
        PlaylistTracksRequest request = null) => 
            await DeleteApiAsync<PlaylistTracksRequest, Snapshot>(
                requestType: $"playlists/{playlistId}/tracks",
                request: request, 
                tokenType: TokenType.User);

    /// <summary>
    /// Get a Playlist Cover Image
    /// </summary>
    /// <param name="playlistId">(Required) The Spotify ID for the playlist.</param>
    /// <returns>List of Image Object</returns>
    /// <exception cref="AuthUserTokenRequiredException"></exception>
    public async Task<List<Image>> AuthGetPlaylistCoverImageAsync(
        string playlistId) => 
            await GetApiAsync<List<Image>>(
                requestType: $"playlists/{playlistId}/images",
                tokenType: TokenType.User);

    /// <summary>
    /// Upload a Custom Playlist Cover Image
    /// <para>Scopes: UserGeneratedContentImageUpload, PlaylistModifyPublic, PlaylistModifyPrivate</para>
    /// </summary>
    /// <param name="playlistId">(Required) The Spotify ID for the playlist.</param>
    /// <param name="file">(Required) JPEG Image File Bytes</param>
    /// <returns>Status Object</returns>
    /// <exception cref="AuthUserTokenRequiredException"></exception>
    public async Task<Status> AuthUploadCustomPlaylistImageAsync(
        string playlistId,
        byte[] file) => 
            await PutApiAsync<Status, Status>(
                requestType: $"playlists/{playlistId}/images",
                fileBytes: file, 
                tokenType: TokenType.User, 
                successCode: HttpStatusCode.Accepted);

    /// <summary>
    /// Get a List of Current User's Playlists
    /// <para>Scopes: PlaylistReadPrivate, PlaylistReadCollaborative</para>
    /// </summary>
    /// <param name="cursor">(Optional) Limit: The maximum number of playlists to return. Default: 20. Minimum: 1. Maximum: 50. - The index of the first playlist to return. Default: 0 (the first object). Maximum offset: 100. Use with limit to get the next set of playlists.</param>
    /// <returns>CursorPaging of Simplified Playlist Object</returns>
    /// <exception cref="AuthUserTokenRequiredException"></exception>
    public async Task<CursorPaging<SimplifiedPlaylist>> AuthLookupUserPlaylistsAsync(
        Cursor cursor = null) => 
            await LookupCursorApiAsync<CursorPaging<SimplifiedPlaylist>>(
                lookupType: "me/playlists",
                cursor: cursor, 
                tokenType: TokenType.User);

    /// <summary>
    /// Change a Playlist's Details
    /// <para>Scopes: PlaylistModifyPublic, PlaylistModifyPrivate</para>
    /// </summary>
    /// <param name="playlistId">(Required) The Spotify ID for the playlist.</param>
    /// <param name="request">(Optional) Playlist Request Object</param>
    /// <returns>Status Object</returns>
    /// <exception cref="AuthUserTokenRequiredException"></exception>
    public async Task<Status> AuthChangePlaylistDetailsAsync(
        string playlistId,
        PlaylistRequest request) => 
            await PutApiAsync<PlaylistRequest, Status>(
                requestType: $"playlists/{playlistId}", 
                request: request, 
                tokenType: TokenType.User);

    /// <summary>
    /// Get a List of a User's Playlists
    /// <para>Scopes: PlaylistReadPrivate, PlaylistReadCollaborative</para>
    /// </summary>
    /// <param name="userId">(Required) The user’s Spotify user ID.</param>
    /// <param name="cursor">(Optional) Limit: The maximum number of playlists to return. Default: 20. Minimum: 1. Maximum: 50. - Offset: The index of the first playlist to return. Default: 0 (the first object). Maximum offset: 100</param>
    /// <returns>CursorPaging of Simplified Playlist Object</returns>
    /// <exception cref="AuthUserTokenRequiredException"></exception>
    public async Task<CursorPaging<SimplifiedPlaylist>> AuthLookupUserPlaylistsAsync(
        string userId,
        Cursor cursor = null) => 
            await LookupCursorApiAsync<CursorPaging<SimplifiedPlaylist>>(
                lookupType: $"users/{userId}/playlists",
                cursor: cursor, 
                tokenType: TokenType.User);

    /// <summary>
    /// Replace a Playlist's Tracks
    /// <para>Scopes: PlaylistModifyPublic, PlaylistModifyPrivate</para>
    /// </summary>
    /// <param name="playlistId">(Required) The Spotify ID for the playlist.</param>
    /// <param name="uris">(Optional) Uri List Request.</param>
    /// <exception cref="AuthUserTokenRequiredException"></exception>
    public async Task<Status> AuthReplacePlaylistTracksAsync(
        string playlistId,
        UriListRequest uris) => 
            await PutApiAsync<UriListRequest, Status>(
                requestType: $"playlists/{playlistId}/tracks",
                request: uris, 
                tokenType: TokenType.User, 
                successCode: HttpStatusCode.Created);

    /// <summary>
    /// Create a Playlist
    /// <para>Scopes: PlaylistModifyPublic, PlaylistModifyPrivate</para>
    /// </summary>
    /// <param name="userId">(Required) The user’s Spotify user ID.</param>
    /// <param name="request">(Required) Playlist Request</param>
    /// <returns>Playlist Object</returns>
    /// <exception cref="AuthUserTokenRequiredException"></exception>
    public async Task<Playlist> AuthCreatePlaylistAsync(
        string userId,
        PlaylistRequest request) => 
            await PostApiAsync<PlaylistRequest, Playlist>(
                requestType: $"users/{userId}/playlists",
                request: request, 
                tokenType: TokenType.User);

    /// <summary>
    /// Reorder a Playlist's Tracks
    /// <para>Scopes: PlaylistModifyPublic, PlaylistModifyPrivate</para>
    /// </summary>
    /// <param name="playlistId">(Required) The Spotify ID for the playlist.</param>
    /// <param name="request">(Required) Playlist Reorder Request</param>
    /// <returns>Snapshot Object</returns>
    /// <exception cref="AuthUserTokenRequiredException"></exception>
    public async Task<Snapshot> AuthReorderPlaylistTracksAsync(
        string playlistId,
        PlaylistReorderRequest request) => 
            await PutApiAsync<PlaylistReorderRequest, Snapshot>(
                requestType: $"playlists/{playlistId}/tracks",
                request: request,
                tokenType: TokenType.User);
    #endregion Authenticated Playlists API 

    #region Authenticated Library API
    /// <summary>
    /// Check User's Saved Albums
    /// <para>Scopes: LibraryRead</para>
    /// </summary>
    /// <param name="itemIds">(Required) List of the Spotify IDs for the albums</param>
    /// <returns>List of true or false values</returns>
    /// <exception cref="AuthUserTokenRequiredException"></exception>
    public async Task<Bools> AuthLookupCheckUserSavedAlbumsAsync(
        List<string> itemIds) => 
            await GetBoolsAsync(
                itemIds: itemIds,
                requestType: "me/albums/contains",
                tokenType: TokenType.User);

    /// <summary>
    /// Save Tracks for User
    /// <para>Scopes: LibraryModify</para>
    /// </summary>
    /// <param name="itemIds">(Required) List of the Spotify IDs for the tracks</param>
    /// <returns>Status Object</returns>
    /// <exception cref="AuthUserTokenRequiredException"></exception>
    public async Task<Status> AuthSaveUserTracksAsync(
        List<string> itemIds) => 
            await PutApiAsync<Status, Status>(
                itemIds: itemIds,
                requestType: "me/tracks",
                tokenType: TokenType.User);

    /// <summary>
    /// Remove Albums for Current User
    /// <para>Scopes: LibraryModify</para>
    /// </summary>
    /// <param name="itemIds">(Required) List of the Spotify IDs for the albums</param>
    /// <returns>Status Object</returns>
    /// <exception cref="AuthUserTokenRequiredException"></exception>
    public async Task<Status> AuthRemoveUserAlbumsAsync(
        List<string> itemIds) => 
            await DeleteApiAsync<Status, Status>(
                itemIds: itemIds,
                requestType: "me/albums",
                tokenType: TokenType.User);

    /// <summary>
    /// Save Albums for Current User
    /// <para>Scopes: LibraryModify</para>
    /// </summary>
    /// <param name="itemIds">(Required) List of the Spotify IDs for the albums</param>
    /// <returns>Status Object</returns>
    /// <exception cref="AuthUserTokenRequiredException"></exception>
    public async Task<Status> AuthSaveUserAlbumsAsync(
        List<string> itemIds) => 
            await PutApiAsync<Status, Status>(
                itemIds: itemIds,
                requestType: "me/albums",
                tokenType: TokenType.User);

    /// <summary>
    /// Remove User's Saved Tracks
    /// <para>Scopes: LibraryModify</para>
    /// </summary>
    /// <param name="itemIds">(Required) List of the Spotify IDs for the tracks</param>
    /// <returns>Status Object</returns>
    /// <exception cref="AuthUserTokenRequiredException"></exception>
    public async Task<Status> AuthRemoveUserTracksAsync(
        List<string> itemIds) => 
            await DeleteApiAsync<Status, Status>(
                itemIds: itemIds,
                requestType: "me/tracks",
                tokenType: TokenType.User);

    /// <summary>
    /// Get User's Saved Albums
    /// <para>Scopes: LibraryRead</para>
    /// </summary>
    /// <param name="market">(Optional) An ISO 3166-1 alpha-2 country code or the string from_token. Provide this parameter if you want to apply Track Relinking.</param>
    /// <param name="cursor">(Optional) Limit: The maximum number of objects to return. Default: 20. Minimum: 1. Maximum: 50. - Offset: The index of the first object to return. Default: 0 (i.e., the first object). Use with limit to get the next set of objects.</param>
    /// <returns>Cursor Paging of Saved Album Object</returns>
    /// <exception cref="AuthUserTokenRequiredException"></exception>
    public async Task<CursorPaging<SavedAlbum>> AuthLookupUserSavedAlbumsAsync(
        string market = null,
        Cursor cursor = null) => 
            await LookupCursorApiAsync<CursorPaging<SavedAlbum>>(
                lookupType: "me/albums", 
                key: "market", 
                value: market, 
                cursor: cursor, 
                tokenType: TokenType.User);

    /// <summary>
    /// Get User's Saved Tracks
    /// <para>Scopes: LibraryRead</para>
    /// </summary>
    /// <param name="market">(Optional) An ISO 3166-1 alpha-2 country code or the string from_token. Provide this parameter if you want to apply Track Relinking.</param>
    /// <param name="cursor">(Optional) Limit: The maximum number of objects to return. Default: 20. Minimum: 1. Maximum: 50. - Offset: The index of the first object to return. Default: 0 (i.e., the first object). Use with limit to get the next set of objects.</param>
    /// <returns>Cursor Paging of Saved Track Object</returns>
    /// <exception cref="AuthUserTokenRequiredException"></exception>
    public async Task<CursorPaging<SavedTrack>> AuthLookupUserSavedTracksAsync(
        string market = null,
        Cursor cursor = null) => 
            await LookupCursorApiAsync<CursorPaging<SavedTrack>>(
                lookupType: "me/tracks", 
                key: "market",
                value: market, 
                cursor: cursor,
                tokenType: TokenType.User);

    /// <summary>
    /// Check User's Saved Tracks
    /// <para>Scopes: LibraryRead</para>
    /// </summary>
    /// <param name="itemIds">(Required) List of the Spotify IDs for the tracks</param>
    /// <returns>List of true or false values</returns>
    /// <exception cref="AuthUserTokenRequiredException"></exception>
    public async Task<Bools> AuthLookupCheckUserSavedTracksAsync(
        List<string> itemIds) => 
        await GetBoolsAsync(
            itemIds: itemIds,
            requestType: "me/tracks/contains",
            tokenType: TokenType.User);

    /// <summary>
    /// Get User's Saved Shows
    /// <para>Scopes: LibraryRead</para>
    /// </summary>
    /// <param name="cursor">(Optional) Limit: The maximum number of objects to return. Default: 20. Minimum: 1. Maximum: 50. - Offset: The index of the first object to return. Default: 0 (i.e., the first object). Use with limit to get the next set of objects.</param>
    /// <returns>Cursor Paging of Saved Show Object</returns>
    /// <exception cref="AuthUserTokenRequiredException"></exception>
    public async Task<CursorPaging<SavedShow>> AuthLookupUserSavedShowsAsync(
        Cursor cursor = null) =>
            await LookupCursorApiAsync<CursorPaging<SavedShow>>(
                lookupType: "me/shows",
                cursor: cursor,
                tokenType: TokenType.User);

    /// <summary>
    /// Check User's Saved Shows
    /// <para>Scopes: LibraryRead</para>
    /// </summary>
    /// <param name="itemIds">(Required) List of the Spotify IDs for the shows</param>
    /// <returns>List of true or false values</returns>
    /// <exception cref="AuthUserTokenRequiredException"></exception>
    public async Task<Bools> AuthLookupCheckUserSavedShowsAsync(
        List<string> itemIds) =>
        await GetBoolsAsync(
            itemIds: itemIds,
            requestType: "me/shows/contains",
            tokenType: TokenType.User);

    /// <summary>
    /// Save Shows for Current User
    /// <para>Scopes: LibraryModify</para>
    /// </summary>
    /// <param name="itemIds">(Required) List of the Spotify IDs for the shows</param>
    /// <returns>Status Object</returns>
    /// <exception cref="AuthUserTokenRequiredException"></exception>
    public async Task<Status> AuthSaveUserShowsAsync(
        List<string> itemIds) =>
            await PutApiAsync<Status, Status>(
                itemIds: itemIds,
                requestType: "me/shows",
                tokenType: TokenType.User);

    /// <summary>
    /// Remove User's Saved Shows
    /// <para>Scopes: LibraryModify</para>
    /// </summary>
    /// <param name="itemIds">(Required) List of the Spotify IDs for the shows</param>
    /// <param name="market">(Optional) An ISO 3166-1 alpha-2 country code. If a country code is specified, only shows that are available in that market will be removed. If a valid user access token is specified in the request header, the country associated with the user account will take priority over this parameter</param>
    /// <returns>Status Object</returns>
    /// <exception cref="AuthUserTokenRequiredException"></exception>
    public async Task<Status> AuthRemoveUserShowsAsync(
        List<string> itemIds, string market = null)
    {
        var parameters = new Dictionary<string, string>();
        if (market != null)
            parameters.Add(nameof(market), market);
        return await DeleteApiAsync<Status, Status>(
            itemIds: itemIds,
            parameters: parameters,
            requestType: "me/shows",
            tokenType: TokenType.User);
    }

    /// <summary>
    /// Get User's Saved Episodes
    /// <para>Scopes: LibraryRead</para>
    /// </summary>
    /// <param name="cursor">(Optional) Limit: The maximum number of objects to return. Default: 20. Minimum: 1. Maximum: 50. - Offset: The index of the first object to return. Default: 0 (i.e., the first object). Use with limit to get the next set of objects.</param>
    /// <returns>Cursor Paging of Saved Episode Object</returns>
    /// <exception cref="AuthUserTokenRequiredException"></exception>
    public async Task<CursorPaging<SavedEpisode>> AuthLookupUserSavedEpisodesAsync(
        Cursor cursor = null) =>
            await LookupCursorApiAsync<CursorPaging<SavedEpisode>>(
                lookupType: "me/episodes",
                cursor: cursor,
                tokenType: TokenType.User);

    /// <summary>
    /// Save Episodes for User
    /// <para>Scopes: LibraryModify</para>
    /// </summary>
    /// <param name="itemIds">(Required) List of the Spotify IDs for the episodes</param>
    /// <returns>Status Object</returns>
    /// <exception cref="AuthUserTokenRequiredException"></exception>
    public async Task<Status> AuthSaveUserEpisodesAsync(
        List<string> itemIds) =>
            await PutApiAsync<Status, Status>(
                itemIds: itemIds,
                requestType: "me/episodes",
                tokenType: TokenType.User);

    /// <summary>
    /// Remove User's Saved Episodes
    /// <para>Scopes: LibraryModify</para>
    /// </summary>
    /// <param name="itemIds">(Required) List of the Spotify IDs for the episodes</param>
    /// <param name="market">(Optional) An ISO 3166-1 alpha-2 country code. If a country code is specified, only episodes that are available in that market will be removed. If a valid user access token is specified in the request header, the country associated with the user account will take priority over this parameter</param>
    /// <returns>Status Object</returns>
    /// <exception cref="AuthUserTokenRequiredException"></exception>
    public async Task<Status> AuthRemoveUserEpisodesAsync(
        List<string> itemIds, string market = null)
    {
        var parameters = new Dictionary<string, string>();
        if (market != null)
            parameters.Add(nameof(market), market);
        return await DeleteApiAsync<Status, Status>(
            itemIds: itemIds,
            parameters: parameters,
            requestType: "me/episodes",
            tokenType: TokenType.User);
    }

    /// <summary>
    /// Check User's Saved Episodes
    /// <para>Scopes: LibraryRead</para>
    /// </summary>
    /// <param name="itemIds">(Required) List of the Spotify IDs for the episodes</param>
    /// <returns>List of true or false values</returns>
    /// <exception cref="AuthUserTokenRequiredException"></exception>
    public async Task<Bools> AuthLookupCheckUserSavedEpisodesAsync(
        List<string> itemIds) =>
        await GetBoolsAsync(
            itemIds: itemIds,
            requestType: "me/episodes/contains",
            tokenType: TokenType.User);
    #endregion Authenticated Library API

    #region Authenticated Player API
    /// <summary>
    /// Add an Item to the User's Playback Queue
    /// <para>Scopes: ConnectModifyPlaybackState</para>
    /// </summary>
    /// <param name="uri">(Required) The uri of the item to add to the queue. Must be a track or an episode uri.</param>
    /// <param name="deviceId">(Optional) The id of the device this command is targeting. If not supplied, the user’s currently active device is the target.</param>
    /// <returns>Status Object</returns>
    /// <exception cref="AuthUserTokenRequiredException"></exception>
    public async Task<Status> AuthUserPlaybackAddToQueueAsync(
        string uri, string deviceId = null)
    {
        var parameters = new Dictionary<string, string>
        {
            { "uri", uri }
        };
        if (deviceId != null)
            parameters.Add("device_id", deviceId);
        return await PostApiStatusAsync<Status, Status>(
            requestType: "me/player/queue",
            parameters: parameters,
            tokenType: TokenType.User,
            successCode: HttpStatusCode.NoContent);
    }

    /// <summary>
    /// Skip User’s Playback To Next Track
    /// <para>Scopes: ConnectModifyPlaybackState</para>
    /// </summary>
    /// <param name="deviceId">(Optional) The id of the device this command is targeting. If not supplied, the user’s currently active device is the target.</param>
    /// <returns>Status Object</returns>
    /// <exception cref="AuthUserTokenRequiredException"></exception>
    public async Task<Status> AuthUserPlaybackNextTrackAsync(
        string deviceId = null)
    {
        var parameters = new Dictionary<string, string>();
        if (deviceId != null)
            parameters.Add("device_id", deviceId);
        return await PostApiStatusAsync<Status, Status>(
            requestType: "me/player/next",
            parameters: parameters, 
            tokenType: TokenType.User, 
            successCode: HttpStatusCode.NoContent);
    }

    /// <summary>
    /// Seek To Position In Currently Playing Track
    /// <para>Scopes: ConnectModifyPlaybackState</para>
    /// </summary>
    /// <param name="position">(Required) The position in milliseconds to seek to. Must be a positive number. Passing in a position that is greater than the length of the track will cause the player to start playing the next song.</param>
    /// <param name="deviceId">(Optional) The id of the device this command is targeting. If not supplied, the user’s currently active device is the target.</param>
    /// <returns>Status Object</returns>
    /// <exception cref="AuthUserTokenRequiredException"></exception>
    public async Task<Status> AuthUserPlaybackSeekTrackAsync(
         int position,
         string deviceId = null)
    {
        var parameters = new Dictionary<string, string>
        {
            { "position_ms", position.ToString() }
        };
        if (deviceId != null)
            parameters.Add("device_id", deviceId);
        return await PutApiAsync<Status, Status>(
            requestType: "me/player/seek", 
            parameters: parameters, 
            tokenType: TokenType.User, 
            successCode: HttpStatusCode.NoContent);
    }

    /// <summary>
    /// Get a User's Available Devices
    /// <para>Scopes: ConnectReadPlaybackState</para>
    /// </summary>
    /// <returns>Devices Object</returns>
    /// <exception cref="AuthUserTokenRequiredException"></exception>
    public async Task<Devices> AuthLookupUserPlaybackDevicesAsync() => 
        await GetApiAsync<Devices>(
         requestType: "me/player/devices",
         tokenType: TokenType.User);

    /// <summary>
    /// Toggle Shuffle For User’s Playback
    /// <para>Scopes: ConnectModifyPlaybackState</para>
    /// </summary>
    /// <param name="state">(Required) true : Shuffle user’s playback, false : Do not shuffle user’s playback</param>
    /// <param name="deviceId">(Optional) The id of the device this command is targeting. If not supplied, the user’s currently active device is the target.</param>
    /// <returns>Status Object</returns>
    /// <exception cref="AuthUserTokenRequiredException"></exception>
    public async Task<Status> AuthUserPlaybackToggleShuffleAsync(
         bool state,
         string deviceId = null)
    {
        var parameters = new Dictionary<string, string>
        {
            { "state", state.ToString().ToLower() }
        };
        if (deviceId != null)
            parameters.Add("device_id", deviceId);
        return await PutApiAsync<Status, Status>(
            requestType: "me/player/shuffle",
            parameters: parameters, 
            tokenType: TokenType.User, 
            successCode: HttpStatusCode.NoContent);
    }

    /// <summary>
    /// Transfer a User's Playback
    /// <para>Scopes: ConnectModifyPlaybackState</para>
    /// </summary>
    /// <param name="request">(Required) Devices Request Object</param>
    /// <returns>Status Object</returns>
    /// <exception cref="AuthUserTokenRequiredException"></exception>
    public async Task<Status> AuthUserPlaybackTransferAsync(
         DevicesRequest request) => 
        await PutApiAsync<DevicesRequest, Status>(
            requestType: "me/player",
            request: request, 
            tokenType: TokenType.User, 
            successCode: HttpStatusCode.NoContent);

    /// <summary>
    /// Get Current User's Recently Played Tracks
    /// <para>Scopes: ListeningRecentlyPlayed</para>
    /// </summary>
    /// <param name="cursor">(Optional) Limit: The maximum number of items to return. Default: 20. Minimum: 1. Maximum: 50. - After: A Unix timestamp in milliseconds. Returns all items after (but not including) this cursor position. If after is specified, before must not be specified. Before - (Optional) A Unix timestamp in milliseconds. Returns all items before (but not including) this cursor position. If before is specified, after must not be specified.</param>
    /// <returns>Cursor Paging of Play History Object</returns>
    /// <exception cref="AuthUserTokenRequiredException"></exception>
    public async Task<CursorPaging<PlayHistory>> AuthLookupUserRecentlyPlayedTracksAsync(
        Cursor cursor = null) => 
        await LookupCursorApiAsync<CursorPaging<PlayHistory>>(
            lookupType: "me/player/recently-played",
            cursor: cursor, 
            tokenType: TokenType.User);

    /// <summary>
    /// Start/Resume a User's Playback
    /// <para>Scopes: ConnectModifyPlaybackState</para>
    /// </summary>
    /// <param name="request">(Optional) Playback Request Object</param>
    /// <param name="deviceId">(Optional) The id of the device this command is targeting. If not supplied, the user’s currently active device is the target.</param>
    /// <returns>Status Object</returns>
    /// <exception cref="AuthUserTokenRequiredException"></exception>
    public async Task<Status> AuthUserPlaybackStartResumeAsync(
        PlaybackRequest request = null,
        string deviceId = null)
    {
        var parameters = new Dictionary<string, string>();
        if (deviceId != null)
            parameters.Add("device_id", deviceId);
        return await PutApiAsync<PlaybackRequest, Status>(
            requestType : "me/player/play",
            request: request, 
            parameters: parameters, 
            tokenType: TokenType.User, 
            successCode: HttpStatusCode.NoContent);
    }

    /// <summary>
    /// Set Repeat Mode On User’s Playback
    /// <para>Scopes: ConnectModifyPlaybackState</para>
    /// </summary>
    /// <param name="state">(Required) track, context or off. track will repeat the current track. context will repeat the current context. off will turn repeat off.</param>
    /// <param name="deviceId">(Optional) The id of the device this command is targeting. If not supplied, the user’s currently active device is the target.</param>
    /// <returns>Status Object</returns>
    /// <exception cref="AuthUserTokenRequiredException"></exception>
    public async Task<Status> AuthUserPlaybackSetRepeatModeAsync(
        RepeatState state,
        string deviceId = null)
    {
        var parameters = new Dictionary<string, string>
        {
            { "state", state.GetDescription() }
        };
        if (deviceId != null)
            parameters.Add("device_id", deviceId);
        return await PutApiAsync<Status, Status>(
            requestType: "me/player/repeat",
            parameters: parameters, 
            tokenType: TokenType.User, 
            successCode: HttpStatusCode.NoContent);
    }

    /// <summary>
    /// Pause a User's Playback
    /// <para>Scopes: ConnectModifyPlaybackState</para>
    /// </summary>
    /// <param name="deviceId">(Optional) The id of the device this command is targeting. If not supplied, the user’s currently active device is the target.</param>
    /// <returns>Status Object</returns>
    /// <exception cref="AuthUserTokenRequiredException"></exception>
    public async Task<Status> AuthUserPlaybackPauseAsync(
        string deviceId = null)
    {
        var parameters = new Dictionary<string, string>();
        if (deviceId != null)
            parameters.Add("device_id", deviceId);
        return await PutApiAsync<Status, Status>(
            requestType: "me/player/pause",
            parameters: parameters, 
            tokenType: TokenType.User, 
            successCode: HttpStatusCode.NoContent);
    }

    /// <summary>
    /// Skip User’s Playback To Previous Track
    /// <para>Scopes: ConnectModifyPlaybackState</para>
    /// </summary>
    /// <param name="deviceId">(Optional) The id of the device this command is targeting. If not supplied, the user’s currently active device is the target.</param>
    /// <returns>Status Object</returns>
    /// <exception cref="AuthUserTokenRequiredException"></exception>
    public async Task<Status> AuthUserPlaybackPreviousTrackAsync(
        string deviceId = null)
    {
        var parameters = new Dictionary<string, string>();
        if (deviceId != null)
            parameters.Add("device_id", deviceId);
        return await PostApiStatusAsync<Status, Status>(
            requestType: "me/player/previous",
            parameters: parameters, 
            tokenType: TokenType.User, 
            successCode: HttpStatusCode.NoContent);
    }

    /// <summary>
    /// Get Information About The User's Current Playback
    /// <para>Scopes: ConnectReadPlaybackState</para>
    /// </summary>
    /// <param name="market">(Optional) An ISO 3166-1 alpha-2 country code or the string from_token. Provide this parameter if you want to apply Track Relinking.</param>
    /// <param name="additionalTypes">(Optional) List of item types that your client supports besides the default track type. Valid types are track and episode. An unsupported type in the response is expected to be represented as null value in the item field. This parameter was introduced to allow existing clients to maintain their current behaviour and might be deprecated in the future.</param>
    /// <returns>Currently Playing Object</returns>
    /// <exception cref="AuthUserTokenRequiredException"></exception>
    public async Task<CurrentlyPlaying> AuthLookupUserPlaybackCurrentAsync(
        string market = null, List<string> additionalTypes = null)
    {
        var parameters = new Dictionary<string, string>();
        if (market != null)
            parameters.Add(nameof(market), market);
        if (additionalTypes != null)
            parameters.Add("additional_types", additionalTypes?.ToArray()?.AsDelimitedString());
        return await GetApiAsync<CurrentlyPlaying>(
            requestType: "me/player",
            parameters: parameters, 
            tokenType: TokenType.User);
    }

    /// <summary>
    /// Get the User's Currently Playing Track
    /// <para>Scopes: ConnectReadCurrentlyPlaying, ConnectReadPlaybackState</para>
    /// </summary>
    /// <param name="market">(Optional) An ISO 3166-1 alpha-2 country code or the string from_token. Provide this parameter if you want to apply Track Relinking.</param>
    /// <param name="additionalTypes">(Optional) List of item types that your client supports besides the default track type. Valid types are track and episode. An unsupported type in the response is expected to be represented as null value in the item field. This parameter was introduced to allow existing clients to maintain their current behaviour and might be deprecated in the future.</param>
    /// <returns>Simplified Currently Playing Object</returns>
    /// <exception cref="AuthUserTokenRequiredException"></exception>
    public async Task<SimplifiedCurrentlyPlaying> AuthLookupUserPlaybackCurrentTrackAsync(
        string market = null, List<string> additionalTypes = null)
    {
        var parameters = new Dictionary<string, string>();
        if (market != null)
            parameters.Add(nameof(market), market);
        if (additionalTypes != null)
            parameters.Add("additional_types", additionalTypes?.ToArray()?.AsDelimitedString());
        return await GetApiAsync<SimplifiedCurrentlyPlaying>(
            requestType: "me/player/currently-playing",
            parameters: parameters, 
            tokenType: TokenType.User);
    }

    /// <summary>
    /// Set Volume For User's Playback
    /// <para>Scopes: ConnectModifyPlaybackState</para>
    /// </summary>
    /// <param name="percent">(Required) The volume to set. Must be a value from 0 to 100 inclusive.</param>
    /// <param name="deviceId">(Optional) The id of the device this command is targeting. If not supplied, the user’s currently active device is the target.</param>
    /// <returns>Status Object</returns>
    /// <exception cref="AuthUserTokenRequiredException"></exception>
    public async Task<Status> AuthUserPlaybackSetVolumeAsync(
         int percent,
         string deviceId = null)
    {
        var parameters = new Dictionary<string, string>
        {
            { "volume_percent", $"{percent}" }
        };
        if (deviceId != null)
            parameters.Add("device_id", deviceId);
        return await PutApiAsync<Status, Status>(
            requestType: "me/player/volume",
            parameters: parameters, 
            tokenType: TokenType.User, 
            successCode: HttpStatusCode.NoContent);
    }

    /// <summary>
    /// Get the User's Queue
    /// <para>Scopes: ConnectReadCurrentlyPlaying</para>
    /// </summary>
    /// <returns>Queue Object</returns>
    /// <exception cref="AuthUserTokenRequiredException"></exception>
    public async Task<Queue> AuthLookupUserQueueAsync() => 
        await GetApiAsync<Queue>(
        requestType: "me/player/queue",
        parameters: null,
        tokenType: TokenType.User);
    #endregion Authenticated Player API

    #region Authenticated Personalisation API
    /// <summary>
    /// Get a User's Top Artists
    /// <para>Scopes: ListeningTopRead</para>
    /// </summary>
    /// <param name="timeRange">(Optional) Over what time frame the affinities are computed. Long Term: alculated from several years of data and including all new data as it becomes available, Medium Term: (Default) approximately last 6 months, Short Term: approximately last 4 weeks</param>
    /// <param name="cursor">(Optional) Limit: The number of entities to return. Default: 20. Minimum: 1. Maximum: 50. For example - Offset: he index of the first entity to return. Default: 0. Use with limit to get the next set of entities.</param>
    /// <returns>Cursor Paging of Artist Object</returns>
    /// <exception cref="AuthUserTokenRequiredException"></exception>
    public async Task<CursorPaging<Artist>> AuthLookupUserTopArtistsAsync(
        TimeRange? timeRange = null,
        Cursor cursor = null)
    {
        string key = null;
        string value = null;
        if (timeRange != null)
        {
            key = "time_range";
            value = timeRange.Value.GetDescription();
        }
        return await LookupCursorApiAsync<CursorPaging<Artist>>(
            lookupType: "me/top/artists",
            key: key, 
            value: value, 
            cursor: cursor, 
            tokenType: TokenType.User);
    }

    /// <summary>
    /// Get a User's Top Tracks
    /// <para>Scopes: ListeningTopRead</para>
    /// </summary>
    /// <param name="timeRange">(Optional) Over what time frame the affinities are computed. Long Term: alculated from several years of data and including all new data as it becomes available, Medium Term: (Default) approximately last 6 months, Short Term: approximately last 4 weeks</param>
    /// <param name="cursor">(Optional) Limit: The number of entities to return. Default: 20. Minimum: 1. Maximum: 50. For example - Offset: he index of the first entity to return. Default: 0. Use with limit to get the next set of entities.</param>
    /// <returns>Cursor Paging of Track Object</returns>
    /// <exception cref="AuthUserTokenRequiredException"></exception>
    public async Task<CursorPaging<Track>> AuthLookupUserTopTracksAsync(
        TimeRange? timeRange = null,
        Cursor cursor = null)
    {
        string key = null;
        string value = null;
        if (timeRange != null)
        {
            key = "time_range";
            value = timeRange.Value.GetDescription();
        }
        return await LookupCursorApiAsync<CursorPaging<Track>>(
            lookupType: "me/top/tracks",
            key: key, 
            value: value, 
            cursor: cursor, 
            tokenType: TokenType.User);
    }
    #endregion Authenticated Personalisation API

    #region Authenticated User Profile API
    /// <summary>
    /// Get a User's Profile
    /// </summary>
    /// <param name="userId">The user’s Spotify user ID.</param>
    /// <returns>Public User Object</returns>
    /// <exception cref="AuthUserTokenRequiredException"></exception>
    public async Task<PublicUser> AuthLookupUserProfileAsync(
        string userId) => 
            await GetApiAsync<PublicUser>(
                itemId: userId,
                requestType: "users",
                tokenType: TokenType.User);

    /// <summary>
    /// Get Current User's Profile
    /// <para>Scopes: UserReadEmail, UserReadPrivate</para>
    /// </summary>
    /// <returns>Private User Object</returns>
    /// <exception cref="AuthUserTokenRequiredException"></exception>
    public async Task<PrivateUser> AuthLookupUserProfileAsync() => 
        await GetApiAsync<PrivateUser>(
            requestType: "me", 
            tokenType: TokenType.User);
    #endregion Authenticated User Profile API
}