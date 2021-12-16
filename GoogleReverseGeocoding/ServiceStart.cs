using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace GoogleReverseGeocoding
{
    public partial class ServiceStart : ServiceBase
    {
        public ServiceStart()
        {
            InitializeComponent();
        }
        protected override void OnStart(string[] args)
        {
            try
            {
                ThreadInformation._ServiceIsAlive = true;
                int threadCount;
                if (int.TryParse(ConfigurationManager.AppSettings["ThreadCount"], out threadCount))
                {
                    for (int indexThread = 1; indexThread <= threadCount; indexThread++)
                    {
                        ThreadInformation threadInformation = new ThreadInformation(indexThread);
                        threadInformation.ThreadCreating();
                    }
                }
                else
                {
                    ThreadInformation._ServiceIsAlive = false;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        protected override void OnStop()
        {
            ThreadInformation._ServiceIsAlive = false;
        }
    }
}
