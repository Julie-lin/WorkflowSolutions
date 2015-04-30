using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Configuration;
using System.Data.SqlClient;
using System.Data.EntityClient;

namespace DBConnectControl
{
    public partial class DBConnectControl: UserControl
    {
        public DBConnectControl()
        {
            InitializeComponent();
            textBoxAccessID.Text = Properties.Settings.Default.USER;
            //textBoxAccessPW.Text = Mnene.Transmitter.Properties.Settings.Default.SECRET;
            textBoxServer.Text = Properties.Settings.Default.SERVER;
            textBoxNamespace.Text = Properties.Settings.Default.NAMESPACE;
            buttonEnter.Enabled = true;
            TestTable = @"[RawData].[DbVersions]";
        }

        public String Username {get {return textBoxAccessID.Text;}}
        public String Namespace {get{ return textBoxNamespace.Text;}}
        public String Server { get { return textBoxServer.Text; } }

        public event EventHandler OK;
        public SqlConnectionStringBuilder BaseConnectionStringBuilder { get; set; }
        public EntityConnectionStringBuilder EfConnectionStringBuilder { get; set; }
        private void buttonEnter_Click(object sender, EventArgs e)
        {
            BaseConnectionStringBuilder = new System.Data.SqlClient.SqlConnectionStringBuilder();
            EfConnectionStringBuilder = new EntityConnectionStringBuilder();
            BaseConnectionStringBuilder["Data Source"] = textBoxServer.Text;
            BaseConnectionStringBuilder["integrated security"] = false;
            BaseConnectionStringBuilder["Initial Catalog"] = textBoxNamespace.Text;
            BaseConnectionStringBuilder["user id"] = textBoxAccessID.Text;
            BaseConnectionStringBuilder["password"] = textBoxAccessPW.Text;
            BaseConnectionStringBuilder["persist security info"] = "True";
            EfConnectionStringBuilder.Metadata = @"res://*/MnemeRawData.csdl|res://*/MnemeRawData.ssdl|res://*/MnemeRawData.msl";
            EfConnectionStringBuilder.Provider = @"System.Data.SqlClient";
            EfConnectionStringBuilder.ProviderConnectionString = BaseConnectionStringBuilder.ConnectionString;
            //Logger.Instance.Info(builder.ConnectionString);
            //Logger.Instance.Info("Connected to " + textBoxServer.Text + "/" + textBoxNamespace.Text);
            if (getDBVersion().Equals("Access error"))
            {
                MessageBox.Show("Access Error");
            }
            else
            {
                // save
                Properties.Settings.Default.USER = textBoxAccessID.Text;
                Properties.Settings.Default.SERVER = textBoxServer.Text;
                Properties.Settings.Default.NAMESPACE = textBoxNamespace.Text;
                Properties.Settings.Default.SECRET = textBoxAccessPW.Text;
                Properties.Settings.Default.Save();
                if (OK != null)
                {
                    OK(this, new EventArgs());
                }
            }
        }

        [Browsable(true), Category("AccessMode"), ReadOnly(false)]
        public String TestTable { get; set; }

        private String getDBVersion()
        {
            String version = "Access error";


            try
            {
                SqlConnection conn = new System.Data.SqlClient.SqlConnection(BaseConnectionStringBuilder.ConnectionString);
                conn.Open();
                SqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = "SELECT count(*) From  "+TestTable;
                version = cmd.ExecuteScalar().ToString();
                conn.Close();
            }
            catch (Exception ex)
            {

            }
            return version;
        }

        private void OnKeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                buttonEnter_Click(null, null);
            }
        }
    }
}
