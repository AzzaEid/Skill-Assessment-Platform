public class CreateLevelDTO
{
    public string Name { get; set; }
    public string Description { get; set; }
    public int Order { get; set; }
    public bool IsActive { get; set; } = true;
}