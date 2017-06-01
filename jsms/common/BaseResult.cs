using System;

namespace jsms.common
{
    public abstract class BaseResult
    {
        public const int ERROR_CODE_NONE = -1;
        public const int ERROR_CODE_OK = 0;
        public const String ERROR_MESSAGE_NONE = "None error message.";

        public const int RESPONSE_OK = 200;

        private ResponseWrapper responseResult;

        public ResponseWrapper ResponseResult
        {
            get { return responseResult; }
            set { responseResult = value; }
        }

        public abstract bool IsResultOK();

        public int GetRateLimitQuota()
        {
            if (null != responseResult)
            {
                return responseResult.rateLimitQuota;
            }
            return 0;
        }

        public int GetRateLimitRemaining()
        {
            if (null != responseResult)
            {
                return responseResult.rateLimitRemaining;
            }
            return 0;
        }

        public int GetRateLimitReset()
        {
            if (null != responseResult)
            {
                return responseResult.rateLimitReset;
            }
            return 0;
        }
    }
}
