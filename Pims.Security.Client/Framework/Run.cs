using System;

namespace Streamline.Pims.Security.Client.Framework
{
    public static class Run
    {
        public static void IgnoreExceptions(Action code)
        {
            try
            {
                code();
            }
            catch
            {
            }
        }
    }
}
