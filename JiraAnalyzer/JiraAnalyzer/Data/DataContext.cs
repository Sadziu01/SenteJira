using JiraAnalyzer.Models;
using Microsoft.EntityFrameworkCore;

namespace JiraAnalyzer.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options)
            : base(options)
        {
        }

        public DbSet<JiraWorklog> JiraWorklogs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<JiraWorklog>()
                .ToTable("X_JIRA_WORKLOG");

            modelBuilder.Entity<JiraWorklog>()
                .HasKey(j => j.Ref);

            modelBuilder.Entity<JiraWorklog>()
                .Property(j => j.Ref).HasColumnName("REF");
            modelBuilder.Entity<JiraWorklog>()
                .Property(j => j.Author).HasColumnName("AUTHOR").HasMaxLength(1024);
            modelBuilder.Entity<JiraWorklog>()
                .Property(j => j.Project).HasColumnName("PROJECT").HasMaxLength(1024);
            modelBuilder.Entity<JiraWorklog>()
                .Property(j => j.Issue).HasColumnName("ISSUE").HasMaxLength(1024);
            modelBuilder.Entity<JiraWorklog>()
                .Property(j => j.IssueSummary).HasColumnName("ISSUESUMMARY").HasMaxLength(1024);
            modelBuilder.Entity<JiraWorklog>()
                .Property(j => j.Qualification).HasColumnName("QUALIFICATION").HasMaxLength(1024);
            modelBuilder.Entity<JiraWorklog>()
                .Property(j => j.TimeSpent).HasColumnName("TIMESPENT");
            modelBuilder.Entity<JiraWorklog>()
                .Property(j => j.WorklogDate).HasColumnName("WORKLOGDATE");
            modelBuilder.Entity<JiraWorklog>()
                .Property(j => j.WorklogStart).HasColumnName("WORKLOGSTART");
            modelBuilder.Entity<JiraWorklog>()
                .Property(j => j.RegTimestamp).HasColumnName("REGTIMESTAMP");
            modelBuilder.Entity<JiraWorklog>()
                .Property(j => j.Descript).HasColumnName("DESCRIPT").HasMaxLength(10240);
            modelBuilder.Entity<JiraWorklog>()
                .Property(j => j.Components).HasColumnName("COMPONENTS").HasMaxLength(1024);
        }
    }
}
