using System.Activities.Tracking;
using System.Collections.Generic;

namespace AcquisitionActivities.RealTime
{
    public static class DataHelperExtensions
    {
        public static T GetDataOrDefault<T>(this CustomTrackingRecord record, string key)
        {
            object value;
            if(record.Data.TryGetValue(key, out value))
            {
                return (T) value;
            }
            return default(T);
        }

        public static T GetData<T>(this CustomTrackingRecord record, string key)
        {
            return (T)record.Data[key];
        }
    }
}