using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using Mneme.Utility.FastSerializer;

namespace Mneme.Data
{
    [Serializable]
    public class TracePoint // : ISerializable 
    {
        public double MZ { get; set; }
        public float RT { get; set; }
        public float Intensity { get; set; }
        public int ScanNumber { get; set; }
        public int Charge { get; set; }
        public bool IsFullScan { get; set; }

        public TracePoint()
        {
        }

        public TracePoint(SerializationInfo info, StreamingContext ctxt)
        {
            SerializationReader sr = SerializationReader.GetReader (info);
            MZ = sr.ReadDouble();
            RT = sr.ReadSingle();
            Intensity = sr.ReadSingle();
            ScanNumber = sr.ReadInt32(); //jlin
            Charge = sr.ReadInt32();
            IsFullScan = sr.ReadBoolean();
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            SerializationWriter sw = SerializationWriter.GetWriter();
            sw.Write(MZ);
            sw.Write(RT);
            sw.Write(Intensity);
            sw.Write(ScanNumber);
            sw.Write(Charge);
            sw.Write(IsFullScan);
            sw.AddToInfo(info);

        }
    }
    [Serializable]
    public class TraceHelper // : ISerializable
    {
        public double MZHigh { get; private set; }
        public double MZLow { get; private set; }
        public List<TracePoint> Points { get; private set; }

        public TraceHelper()
        {
            Points = new List<TracePoint>();
        }
        public TraceHelper(double mzlow, double mzhigh)
        {
            Points = new List<TracePoint>();
            MZHigh = mzhigh;
            MZLow = mzlow;
        }


        public TraceHelper(SerializationInfo info, StreamingContext ctxt)
        {
            SerializationReader sr = SerializationReader.GetReader(info);
            MZHigh = sr.ReadDouble();
            MZLow = sr.ReadDouble();
            Points = sr.ReadList<TracePoint>().ToList();
        }


        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            SerializationWriter sw = SerializationWriter.GetWriter();
            sw.Write(MZHigh);
            sw.Write(MZLow);
            sw.Write(Points);
            sw.AddToInfo(info);
        }
    }

    public class TraceMaker
    {
        private List<TracePoint> m_points;
        private List<int> m_vscans;
        public TraceMaker()
        {
            m_points = new List<TracePoint>();
            m_vscans = null;
        }
        public TraceMaker(List<int> validscans)
        {
            m_points = new List<TracePoint>();
            m_vscans = validscans;
        }

        public void Add(List<TracePoint> tps)
        {
            m_points.AddRange(tps);
        }

        public XIC MakeXic(double rtstart, double rtstop, double mzstart, double mzstop)
        {
            XIC xic = new XIC();
            Dictionary<int,TraceMakerHelper> map = new Dictionary<int,TraceMakerHelper>();
            // build consolidated list
            foreach(TracePoint tp in m_points){
                if (m_vscans != null)
                {
                    if (tp.RT >= rtstart && tp.RT < rtstop && tp.MZ >= mzstart && tp.MZ <= mzstop)
                    {
                        if (m_vscans != null && !map.ContainsKey(tp.ScanNumber))
                        {

                            bool valid = false;
                            // is this a valid scan?
                            foreach (int vscan in m_vscans)
                            {
                                if (vscan == tp.ScanNumber)
                                {
                                    valid = true;
                                    break;
                                }
                            }
                            if (!valid) continue;

                            map[tp.ScanNumber] = new TraceMakerHelper();
                        }
                        map[tp.ScanNumber].Add(tp);
                    }
                }
                else
                {
                    if (tp.RT >= rtstart && tp.RT < rtstop && tp.MZ >= mzstart && tp.MZ <= mzstop)
                    {
                        if (!map.ContainsKey(tp.ScanNumber))
                        {
                            map[tp.ScanNumber] = new TraceMakerHelper();
                        }
                        map[tp.ScanNumber].Add(tp);
                    }
                }
            }
            foreach( int scan in map.Keys)
            {
                TraceMakerHelper tmh = map[scan];
                xic.Charges.Add(tmh.Charge());
                xic.Intensities.Add(tmh.Intensity());
                xic.MZs.Add(tmh.MZ());
                xic.Times.Add(tmh.RT);
            }
            return xic;
        }

        public class TraceMakerHelper
        {
            public TraceMakerHelper()
            {
                ISum = 0;
                MZSum = 0;
                N = 0;
                Chg = 0;
                MaxI = 0;
            }
            private double ISum {get;set;}
            private double MZSum { get; set; }
            private  double N { get; set; }
            private double MaxI { get; set; }
            private int Chg { get; set; }
            public double RT { get; set; }
            public void Add(TracePoint tp)
            {
                if (tp.Intensity > MaxI)
                {
                    MaxI = tp.Intensity;
                    if (tp.Charge > 0)
                    {
                        Chg = tp.Charge;
                    }
                }
                N++;
                ISum += tp.Intensity;
                MZSum += tp.MZ*tp.Intensity;
                RT = tp.RT;
            }
            public int Charge()
            {
                return Chg;
            }
            public double Intensity()
            {
                return ISum ;
            }
            public double MZ()
            {
                return MZSum / ISum;
            }
        }
    }
}
