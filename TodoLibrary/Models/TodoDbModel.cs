namespace TodoLibrary.Models;

public class TodoDbModel
{
   // To create a new record in the DB and what is coming from the DB
   public int Id { get; set; }
   public string? Task { get; set; }
   public int AssignedTo { get; set; }
   public bool IsComplete { get; set; }
}
