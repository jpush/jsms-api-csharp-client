using System;

namespace jsms.util
{
    class Preconditions
    {
        public static void CheckArgument(bool expression)
        {
            if (!expression)
            {
                throw new ArgumentNullException();
            }
        }

        public static void CheckArgument(bool expression, object errorMessage)
        {
            if (!expression)
            {
                throw new ArgumentException(errorMessage.ToString());
            }
        }
    }
}
