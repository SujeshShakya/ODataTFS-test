//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;

using System;
using System.Collections.Generic;
namespace OdataTFS
{
    public class WorkItem
    {
        public WorkItem() { }
        public WorkItem(string title)
        {
            this.Title = title;
        }
        public List<string> lstTask { get; set; }

        public int Id { get; set; }

        public string AreaPath { get; set; }

        public string IterationPath { get; set; }

        public int Revision { get; set; }

        public string Priority { get; set; }

        public string Severity { get; set; }

        public string StackRank { get; set; }

        public string Project { get; set; }

        public string AssignedTo { get; set; }

        public DateTime CreatedDate { get; set; }

        public string CreatedBy { get; set; }

        public DateTime ChangedDate { get; set; }

        public string ChangedBy { get; set; }

        public string ResolvedBy { get; set; }

        public string Title { get; set; }

        public string State { get; set; }

        public string Type { get; set; }

        public string Reason { get; set; }

        public string Description { get; set; }

        public string ReproSteps { get; set; }

        public string FoundInBuild { get; set; }

        public string IntegratedInBuild { get; set; }

        public string WebEditorUrl { get; set; }

        public int ParentId { get; set; }

        //[ForeignProperty]
        //public IEnumerable<Attachment> Attachments { get; set; }
    }

//    public class WorkItems
//    {
//        public WorkItem d { get; set; }
//    }

//    public class WorkItem
//    {
//        public List<WorkItemResult> results { get; set; }
//    }

//    public class WorkItemResult
//    {
//        public WorkItemResultMetadata __metadata { get; set; }
//    }

//    public class WorkItemResultMetadata
//    {
//        public string uri { get; set; }
//    }
}
