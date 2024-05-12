using TodoLibrary.Models;

namespace TodoLibrary.DataAccess
{
   public interface ITodoData
   {
      Task CompleteTodo(int assignedTo, int todoId);
      Task<TodoDbModel?> Create(int assignedTo, string task);
      Task Delete(int assignedTo, int todoId);
      Task<List<TodoDbModel>> GetAllAssigned(int assignedTo);
      Task<TodoDbModel?> GetOneAssigned(int assignedTo, int todoId);
      Task UpdateTask(int assignedTo, int todoId, string task);
   }
}