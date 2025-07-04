using TaskListManager.Common.Contracts;
using TaskListManager.Data.ViewModel;

namespace TaskListManager.Business.Interface
{
    public interface ITaskListManagerService
    {
        Task<Response<TaskViewModel>> CreateTask(CreateTaskDto createTaskDto);
        Task<Response<bool>> DeleteTask(string id);
        Task<Response<List<TaskViewModel>>> GetAllTasks();
        Task<Response<TaskViewModel>> GetTaskById(string id);
        Task<Response<TaskViewModel>> UpdateTask(string id, UpdateTaskDto updateTaskDto);
    }
}
