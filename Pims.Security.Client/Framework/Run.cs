using System;

namespace Pims.Security.Client.Core.Framework
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