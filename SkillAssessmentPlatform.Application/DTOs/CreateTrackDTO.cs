public class CreateTrackDTO
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string Objectives { get; set; }
    public string AssociatedSkills { get; set; }
    public string Image { get; set; }
    public string SeniorExaminerID { get; set; }
    public bool IsActive { get; set; }
}