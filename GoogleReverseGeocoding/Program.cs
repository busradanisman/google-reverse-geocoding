using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace GoogleReverseGeocoding
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main()
        {
            //ThreadInformation threadInformation = new ThreadInformation(2);
            //threadInformation.ThreadCreating();

            ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[]
            {
                new ServiceStart()
            };
            ServiceBase.Run(ServicesToRun);
        }
    }
}
