using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace DiagramDesigner.ViewModels
{
    public class GridResultTest : ViewModelBase
    {
        public GridResultTest(string s, double mass,  double rt, int iTest)
        {
            TestString = s;
            Mass = mass;
            RetentionTime = rt;
            TestInterger = iTest;

        }
        private string _testString;
        public string TestString
        {
            get { return _testString; }
            set
            {
                _testString = value;
                InvokePropertyChanged("TestString");
            }
        }

        private double _mass;
        public double Mass
        {
            get { return _mass; }
            set
            {
                _mass = value;
                InvokePropertyChanged("Mass");
            }
        }


        private double _retentionTime;
        public double RetentionTime
        {
            get { return _retentionTime; }
            set
            {
                _retentionTime = value;
                InvokePropertyChanged("RetentionTime");
            }
        }


        private int _testInterger;
        public int TestInterger
        {
            get { return _testInterger; }
            set
            {
                _testInterger = value;
                InvokePropertyChanged("TestInterger");
            }
        }


    }
}
