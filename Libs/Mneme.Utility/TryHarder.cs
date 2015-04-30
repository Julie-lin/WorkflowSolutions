using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mneme.Utility
{
    public static class TryHarder
    {
        public static void Retry(Action tryHarderWhat, int howHard, TimeSpan restPeriod, params Type[] expectedExceptions)
        {
            int remainingTries = howHard;
            while (true)
            {
                try
                {
                    tryHarderWhat();
                    break;
                }
                catch (Exception ex)
                {
                    remainingTries--;
                    Console.WriteLine(ex.Message);
                    if (remainingTries == 0 || RetryOnExceptionType(expectedExceptions, ex) == false)
                    {
                        throw;
                    }
                    System.Threading.Thread.Sleep(restPeriod);
                }
            }
        }

        public static void Retry(Action tryHarderWhat, TimeSpan howLong, TimeSpan restPeriod, params Type[] expectedExceptions)
        {
            var firstTryTime = DateTime.Now;
            while (true)
            {
                try
                {
                    tryHarderWhat();
                    break;
                }
                catch (Exception ex)
                {
                    //jlin need to log error
                    Console.WriteLine(ex.Message);
                    if ((DateTime.Now - firstTryTime) > howLong || RetryOnExceptionType(expectedExceptions, ex) == false)
                    {

                        throw;
                    }
                    System.Threading.Thread.Sleep(restPeriod);
                }
            }
        }


        private static bool RetryOnExceptionType(Type[] expectedExceptions,
            Exception exception)
        {
            return expectedExceptions.Any(
                expected => expected.IsAssignableFrom(exception.GetType()));
        }
    }
}
