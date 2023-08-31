using System;
using Utis.Minex.WrokerIntegration;

namespace AdvanticaAutomationTestClient.Models
{
    public class WorkerServiceModel
    {
        public long Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }
        public DateTime Birthday { get; set; }
        public Sex Sex { get; set; }
        public bool HaveChildren { get; set; }
    }
}
