using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using Topshelf;

namespace JobRecurring
{
     class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            var exitCode = HostFactory.Run(x =>
            {
                x.Service<RecurringProcess>(s =>
                {
                    s.ConstructUsing(recurring => new RecurringProcess());
                    s.WhenStarted(recurring => recurring.Start());
                    s.WhenStopped(recurring => recurring.Stop());
                });
                x.RunAsLocalService();
                x.SetServiceName("MechillRecurring");
                x.SetDisplayName("Mechill Recurring");
                x.SetDescription("This is mechill recurring movie");
            });
            int exitCodeValue = (int)Convert.ChangeType(exitCode, exitCode.GetTypeCode());
            Environment.ExitCode = exitCodeValue;
        }
    }
}
