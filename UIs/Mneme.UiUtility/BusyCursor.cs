using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mneme.UiUtility
{
    using System.Windows.Forms;

    public sealed class BusyCursor : IDisposable
    {
        private readonly Cursor m_previousCursor;

        /// <summary>
        /// Constructor
        /// </summary>
        public BusyCursor()
        {
            m_previousCursor = Cursor.Current;
            Cursor.Current = Cursors.WaitCursor;
        }

        //protected override void Dispose(bool disposing)
        //{
        //    if (disposing)
        //    {
        //        Cursor.Current = m_previousCursor;
        //    }
        //    base.Dispose(disposing);
        //}

        public void Dispose()
        {
            Cursor.Current = m_previousCursor;
        }
    }
}
