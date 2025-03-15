using System.Collections.Generic;

namespace EducationCenter.Models
{
    public class StudentProgressViewModel
    {
        public int EnrollmentId { get; set; }
        public int CourseId { get; set; }
        public string CourseTitle { get; set; }
        public int CurrentProgress { get; set; }
        public List<MaterialProgressItem> MaterialProgresses { get; set; }
    }

    public class MaterialProgressItem
    {
        public int MaterialId { get; set; }
        public string MaterialTitle { get; set; }
        public bool IsCompleted { get; set; }
    }
}