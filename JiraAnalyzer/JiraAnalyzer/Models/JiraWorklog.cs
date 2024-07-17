using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JiraAnalyzer.Models
{
    public class JiraWorklog
    {
        [Key]
        [Column("REF")]
        public int Ref { get; set; }

        [Column("AUTHOR")]
        public string Author { get; set; }

        [Column("PROJECT")]
        public string Project { get; set; }

        [Column("ISSUE")]
        public string Issue { get; set; }

        [Column("ISSUESUMMARY")]
        public string IssueSummary { get; set; }

        [Column("QUALIFICATION")]
        public string Qualification { get; set; }

        [Column("TIMESPENT")]
        public double TimeSpent { get; set; }

        [Column("WORKLOGDATE")]
        public DateTime WorklogDate { get; set; }

        [Column("WORKLOGSTART")]
        public DateTime WorklogStart { get; set; }

        [Column("REGTIMESTAMP")]
        public DateTime RegTimestamp { get; set; }

        [Column("DESCRIPT")]
        public string Descript { get; set; }

        [Column("COMPONENTS")]
        public string Components { get; set; }
    }
}
