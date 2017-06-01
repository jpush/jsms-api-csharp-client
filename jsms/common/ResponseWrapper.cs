using System;
using System.Net;

namespace jsms.common
{
    public class ResponseWrapper
    {
        public HttpStatusCode responseCode = HttpStatusCode.BadRequest;

        private const int RESPONSE_CODE_NONE = -1;
        private String _responseContent;

        public String ResponseContent
        {
            get
            {
                return _responseContent;
            }
            set
            {
                _responseContent = value;
            }
        }

        public void SetErrorObject()
        {
            if (!string.IsNullOrEmpty(_responseContent))
            {

            }
        }

        public int rateLimitQuota;
        public int rateLimitRemaining;
        public int rateLimitReset;

        public bool IsServerResponse()
        {
            return responseCode == HttpStatusCode.OK;
        }

        public String exceptionString;

        public ResponseWrapper()
        {
        }
    }
}
