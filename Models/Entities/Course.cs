using System;
using System.Collections.Generic;

namespace back_sv_users.Models.Entities;

public partial class Course
{
    public int Places { get; set; }

    public float? Score { get; set; }

    public DateTime Date { get; set; }

    public long Id { get; set; }

    public string Description { get; set; } = null!;

    public string Link { get; set; } = null!;

    public string Title { get; set; } = null!;

    public string Modality { get; set; } = null!;

    public virtual ICollection<CourseEntrepreneurship> CourseEntrepreneurships { get; set; } = new List<CourseEntrepreneurship>();
}
