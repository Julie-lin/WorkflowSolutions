using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Mneme.Data
{
    [Serializable]
    [DataContract]

    public class MnemeRunHeader
    {

        #region Properties

        ///<summary>
        /// Get the number for the first scan in this stream (usually 1)
        ///</summary>
        [DataMember]
        public int FirstSpectrum { get; set; }

        ///<summary>
        /// Get the number for the last scan in this stream
        ///</summary>
        [DataMember]
        public int LastSpectrum { get; set; }

        ///<summary>
        /// Time of first scan in file
        ///</summary>
        [DataMember]
        public double StartTime { get; set; }

        ///<summary>
        /// Time of last scan in file
        ///</summary>
        [DataMember]
        public double EndTime { get; set; }

        ///<summary>
        /// Lowest recored mass in file
        ///</summary>
        [DataMember]
        public double LowMass { get; set; }

        ///<summary>
        /// Highest recorded mass in file
        ///</summary>
        [DataMember]
        public double HighMass { get; set; }

        /// <summary>
        /// Gets the mass resolution value recorded for the current instrument. 
        /// The value is returned as one half of the mass resolution. 
        /// For example, a unit resolution controller would return a value of 0.5.
        /// </summary>
        [DataMember]
        public double MassResolution { get; set; }

        /// <summary>
        /// Gets the expected acquisition run time for the current instrument.
        /// </summary>double ExpectedRunTime { get; }
        [DataMember]
        public double ExpectedRuntime { get; set; }

        /// <summary>
        /// Gets the max integrated intensity.
        /// </summary>
        [DataMember]
        public double MaxIntegratedIntensity { get; set; }

        /// <summary>
        /// Gets the max intensity.
        /// </summary>
        [DataMember]
        public int MaxIntensity { get; set; }

        /// <summary>
        /// Gets the tolerance unit.
        /// </summary>
        [DataMember]
        public ToleranceUnits ToleranceUnit { get; set; }

        #endregion
    }
}
