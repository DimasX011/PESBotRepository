using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConsoleApp1.Tasks.Elements;

namespace ConsoleApp1.Tasks
{
    public class TaskElement
    {
        public string self { get; set; }

        public string id { get; set; }

        public string key { get; set; }

        public int version { get; set; }

        public string summary { get; set; }

        public string statusStartTime { get; set; }

        public update updatedBy { get; set; }

        public statustype statusType { get; set; }

        public string description { get; set; }

        public typetask type { get; set; }

        public typetask priority { get; set; }

        public string createdAt { get; set; }

        public update createdBy { get; set; }

        public int commentWithoutExternalMessageCount { get; set; }

        public int votes { get; set; }

        public int commentWithExternalMessageCount { get; set; }

        public update assignee { get; set; }

        public string updatedAt { get; set; }

        public statusElements status { get; set; }

        public queque queue { get; set; }

    }
}
