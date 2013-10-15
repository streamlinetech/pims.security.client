using System;

namespace Streamline.Pims.Apis.Common
{
    public static class Run
    {
        public static void IgnoreExceptions(Action code)
        {
            try
            {
                code();
            }
            catch { }
        }
    }
}