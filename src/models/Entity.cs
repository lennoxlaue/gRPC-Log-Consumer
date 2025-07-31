using System.ComponentModel.DataAnnotations;

public class Entity
{
    [Key]
    public int Id { get; set; }

    public  required string Log { get; set; }

    public Stage Stage { get; set; }

    public DateTime Timestamp { get; set; }



}