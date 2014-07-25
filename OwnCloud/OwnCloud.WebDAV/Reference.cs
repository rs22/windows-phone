namespace OwnCloud.WebDAV {
    /// <summary>
    /// HTTP Status codes. See RFC 1945 (HTTP), RFC 2616 (HTTP/1.1) and RFC 2518 (WebDAV)
    /// </summary>
    internal enum ServerStatus
    {
        // this is necessary since enum cannot be inherited and System.Net.HttpStatusCode is insufficient.
        LocalFailure = 0,
        Continue = 100,
        SwitchingProtocols = 101,
        Processing = 102, // removed but stay here for compatibility
        OK = 200,
        Created = 201,
        Accepted = 202,
        NonAuthoritativeInformation = 203,
        NoContent = 204,
        ResetContent = 205,
        PartialContent = 206,
        MultiStatus = 207,
        ContentDifferent = 210,
        MultipleChoices = 300,
        MovedPermanently = 301,
        Found = 302,
        SeeOther = 303,
        NotModifed = 304,
        UseProxy = 305,
        Unused = 306,
        TemporaryRedirect = 307,
        BadRequest = 400,
        Unauthorized = 401,
        PaymentRequired = 402,
        Forbidden = 403,
        NotFound = 404,
        MethodNotAllowed = 405,
        NotAcceptable = 406,
        ProxyAuthenticationRequired = 407,
        RequestTimeout = 408,
        Conflict = 409,
        Gone = 410,
        LengthRequired = 411,
        PreconditionFailed = 412,
        RequestEntityTooLarge = 413,
        RequestURITooLarge = 414,
        UnsupportedMediaType = 415,
        RequestRangeNotSatisfiable = 416,
        ExpectationFailed = 417,
        UnprocessableEntity = 422,
        Locked = 423,
        FailedDependency = 424,
        InsufficentspaceOnResource = 425,
        InvalidCollblob = 475,
        InternalServerError = 500,
        NotImplemented = 501,
        BadGateway = 502,
        ServiceUnavailable = 503,
        GatewayTimeout = 504,
        HTTPVersionNotSupported = 505,
        InsufficientStorage = 507
    }

    /// <summary>
    /// DAV HTTP Headers.
    /// </summary>
    struct Header
    {
        /// <summary>
        /// A compilance class list given by the server. Usually feedback of OPTIONS method.
        /// </summary>
        internal const string DAV = "DAV";

        /// <summary>
        /// (MethodDepthAttribute) Defindes on which level methods should by executed. Applies: PROPFIND (1,noroot); DELETE,MOVE,COPY,LOCK,UNLOCK.
        /// </summary>
        internal const string Depth = "Depth";

        /// <summary>
        /// (URI) Absolute uri of destination. Applies: COPY,DELETE,MOVE.
        /// </summary>
        internal const string Destination = "Destination";

        /// <summary>
        /// (IfAttribute) A complex status query.
        /// </summary>
        internal const string If = "If";

        /// <summary>
        /// (URI:UUID = "urn:uuid:") Used to idenfity a file lock. Applies: LOCK,UNLOCK.
        /// </summary>
        internal const string LockToken = "Lock-Token";

        /// <summary>
        /// (T|F) Specifies whetever a server should overwrite a non-null destination. Applies: COPY,MOVE.
        /// </summary>
        internal const string Overwrite = "Overwrite";

        /// <summary>
        /// ("Second-"|"Infinite") Client can specify a timeout for a locking operation but a server is not required to care of this. Applies: LOCK.
        /// </summary>
        internal const string Timeout = "Timeout";

        /// <summary>
        /// The HTTP-Hostname.
        /// </summary>
        internal const string Host = "Host";

        /// <summary>
        /// The HTTP-Content-Type.
        /// </summary>
        internal const string ContentType = "Content-Type";

        /// <summary>
        /// The HTTP-Content-Length
        /// </summary>
        internal const string ContenLength = "Content-Length";
    }

    /// <summary>
    /// Complex types for headers.
    /// </summary>
    internal static class HeaderAttribute
    {
        /// <summary>
        /// Defines the depth of method applying on resources.
        /// </summary>
        internal static class MethodDepth
        {
            /// <summary>
            /// Applies to Resource only.
            /// </summary>
            internal const string ApplyResourceOnly = "0";

            /// <summary>
            /// Applies to Resouce and its immediate children.
            /// </summary>
            internal const string ApplyResourceAndChildren = "1";

            /// <summary>
            /// Applies only to resouce subordinate but not the resource target.
            /// </summary>
            internal const string ApplyResourceOnlyNoRoot = "1,noroot";

            /// <summary>
            /// Applies to all.
            /// </summary>
            internal const string ApplyInfinity = "infinity";

            /// <summary>
            /// Applies only to resouce subordinate recursively but not on the resource target.
            /// </summary>
            internal const string ApplyInfinityNoRoot = "infinity,noroot";
        }
    }

    /// <summary>
    /// Properties which can be requested by the client.
    /// </summary>
    internal static class RequestProperties
    {
        /// <summary>
        /// (DateTime) Records the time and date the resouce was created.
        /// </summary>
        internal const string CreationDate = "creationdate";

        /// <summary>
        /// (string) Provides a name for the resource that is suitable for presentation to a user.
        /// </summary>
        internal const string DisplayName = "displayname";

        /// <summary>
        /// (string language-tag) Contains the content language header value.
        /// </summary>
        internal const string GetContentLanguage = "getcontentlanguage";

        /// <summary>
        /// (long) Contains the content-length header.
        /// </summary>
        internal const string GetContentLength = "getcontentlength";

        /// <summary>
        /// (string) Contains the mime-type.
        /// </summary>
        internal const string GetContentType = "getcontenttype";

        /// <summary>
        /// (string) Contains the ETag header value.
        /// </summary>
        internal const string GetETag = "getetag";

        /// <summary>
        /// (DateTime) Records the timd and date the resource was last modified.
        /// </summary>
        internal const string GetLastModified = "getlastmodified";

        /// <summary>
        /// (LockDiscovery) Returns the active locks on a resource.
        /// </summary>
        internal const string LockDiscovery = "lockdiscovery";

        /// <summary>
        /// (ResourceType) Specifies the nature of the resource.
        /// </summary>
        internal const string ResourceType = "resourcetype";

        /// <summary>
        /// RFC3253
        /// </summary>
        internal const string SupportedLivePropertySet = "supported-live-property-set";

        /// <summary>
        /// To provide a listing of the lock capabilities supported by the resource.
        /// </summary>
        internal const string SupportedLock = "supportedlock";

        /// <summary>
        /// RFC3253
        /// </summary>
        internal const string SupportedReportSet = "supported-report-set";

        /// <summary>
        /// RFC 4331 Extension. Used quota in bytes.
        /// </summary>
        internal const string QuotaUsedBytes = "quota-used-bytes";

        /// <summary>
        /// RFC 4331 Extension. Available quota in bytes.
        /// </summary>
        internal const string QuotaAvailableBytes = "quota-available-bytes";
    }

    /// <summary>
    /// DAV-Element definitions from RFC 4918.
    /// </summary>
    internal static class Elements
    {
        /// <summary>
        /// Describtes a lock on a resource.
        /// </summary>
        internal const string ActiveLock = "activelock";

        /// <summary>
        /// Specifies that all names and values of dead properties and the live properties defined by this document existing on the resource are to be returned.
        /// </summary>
        internal const string AllProperties = "allprop";

        /// <summary>
        /// Identifies the associated resource as a collection. The DAV:resourcetype property of a collection resource MUST contain this element. It is normally empty but extensions may add sub-elements.
        /// </summary>
        internal const string Collection = "collection";

        /// <summary>
        /// Used for representing depth values in XML content (e.g., in lock information).
        /// </summary>
        internal const string Depth = "depth";

        /// <summary>
        /// Error responses, particularly 403 Forbidden and 409 Conflict, sometimes need more information to indicate what went wrong. In these cases, servers MAY return an XML response body with a document element of 'error', containing child elements identifying particular condition codes.
        /// </summary>
        internal const string Error = "error";

        /// <summary>
        /// Specifies an exclusive lock.
        /// </summary>
        internal const string ExclusiveLocking = "exclusive";

        /// <summary>
        /// Any child element represents the name of a property to be included in the PROPFIND response. All elements inside an 'include' XML element MUST define properties related to the resource, although possible property names are in no way limited to those property names defined in this document or other standards. This element MUST NOT contain text or mixed content.
        /// </summary>
        internal const string Include = "include";

        /// <summary>
        /// Contains a single href element with the same value that would be used in a Location header.
        /// </summary>
        internal const string Location = "location";

        /// <summary>
        /// Defines the types of locks that can be used with the resource.
        /// </summary>
        internal const string LockEntry = "lockentry";

        /// <summary>
        /// Describes the active locks on a resource.
        /// </summary>
        internal const string LockDiscovery = "lockdiscovery";

        /// <summary>
        /// The 'lockinfo' XML element is used with a LOCK method to specify the type of lock the client wishes to have created.
        /// </summary>
        internal const string LockInfo = "lockinfo";

        /// <summary>
        /// The href element contains the root of the lock. The server SHOULD include this in all DAV:lockdiscovery property values and the response to LOCK requests.
        /// </summary>
        internal const string LockRoot = "lockroot";

        /// <summary>
        /// Specifies whether a lock is an exclusive lock, or a shared lock.
        /// </summary>
        internal const string LockScope = "lockscope";

        /// <summary>
        /// The href contains a single lock token URI, which refers to the lock.
        /// </summary>
        internal const string LockToken = "locktoken";

        /// <summary>
        /// Specifies the access type of a lock. At present, this specification only defines one lock type, the write lock.
        /// </summary>
        internal const string LockType = "locktype";

        /// <summary>
        /// The 'responsedescription' element at the top level is used to provide a general message describing the overarching nature of the response. If this value is available, an application may use it instead of presenting the individual response descriptions contained within the responses.
        /// </summary>
        internal const string MultiStatus = "multistatus";

        /// <summary>
        /// Holds client-supplied information about the creator of a lock.
        /// </summary>
        internal const string Owner = "owner";

        /// <summary>
        /// Contains properties related to a resource.
        /// </summary>
        internal const string Properties = "prop";

        /// <summary>
        /// Contains a request to alter the properties on a resource.
        /// </summary>
        internal const string PropertyUpdate = "propertyupdate";

        /// <summary>
        /// Specifies the properties to be returned from a PROPFIND method. Four special elements are specified for use with 'propfind': 'prop', 'allprop', 'include', and 'propname'. If 'prop' is used inside 'propfind', it MUST NOT contain property values.
        /// </summary>
        internal const string PropertyFind = "propfind";

        /// <summary>
        /// Specifies that only a list of property names on the resource is to be returned.
        /// </summary>
        internal const string PropertyName = "propname";

        /// <summary>
        /// Groups together a prop and status element that is associated with a particular 'href' element.
        /// </summary>
        internal const string PropertyState = "propstat";

        /// <summary>
        /// There may be limits on the value of 'href' depending on the context of its use. Refer to the specification text where 'href' is used to see what limitations apply in each case.
        /// </summary>
        internal const string Reference = "href";

        /// <summary>
        /// Lists the properties to be removed from a resource.
        /// </summary>
        internal const string Remove = "remove";

        /// <summary>
        /// olds a single response describing the effect of a method on resource and/or its properties.
        /// </summary>
        internal const string Response = "response";

        /// <summary>
        /// Contains information about a status response within a Multi-Status.
        /// </summary>
        internal const string ResponseDescription = "responsedescription";

        /// <summary>
        /// Lists the property values to be set for a resource.
        /// </summary>
        internal const string Set = "set";

        /// <summary>
        /// Specifies a shared lock.
        /// </summary>
        internal const string SharedLocking = "shared";

        /// <summary>
        /// Holds a single HTTP status-line.
        /// </summary>
        internal const string Status = "status";

        /// <summary>
        /// The number of seconds remaining before a lock expires.
        /// </summary>
        internal const string Timeout = "timeout";

        /// <summary>
        /// Specifies a write lock.
        /// </summary>
        internal const string WriteLocking = "write";
    }

    /// <summary>
    /// Resource content type. Other resource types as this are not supported.
    /// Unknown Resoure type should be treated as Collection.
    /// </summary>
    internal enum ResourceType
    {
        None = 0,
        Collection = 1,
        Calendar = 2
    }
}