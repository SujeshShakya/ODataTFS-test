using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OdataTFS
{
    public class ChangeSets
    {
        
        public int Id { get; set; }

        public string ArtifactUri { get; set; }

        public string Comment { get; set; }

        public string Committer { get; set; }

        public DateTime CreationDate { get; set; }

        public string Owner { get; set; }

        public string Branch { get; set; }

        public string WebEditorUrl { get; set; }

        //[ForeignProperty]
        //public IEnumerable<Change> Changes { get; set; }

        //[ForeignProperty]
        //public IEnumerable<WorkItem> WorkItems { get; set; }

    }
}
