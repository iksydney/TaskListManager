using AutoMapper;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using TaskListManager.Business.Factory;
using TaskListManager.Business.Interface;
using TaskListManager.Common.Contracts;
using TaskListManager.Data.ViewModel;

namespace TaskListManager.Business.Implementation
{
    public class TaskListManagerService : ITaskListManagerService
    {
        private readonly IHttpService _httpService;
        private const string BaseUrl = "https://686626a889803950dbb16571.mockapi.io";
        private const string RequestUri = "/api/TaskLists";
        private readonly IMapper _mapper;
        private readonly ILogger<TaskListManagerService> _logger;
        public TaskListManagerService(IHttpService httpService, IMapper mapper, ILogger<TaskListManagerService> logger)
        {
            _httpService = httpService;
            _mapper = mapper;
            _logger = logger;
        }
        public async Task<Response<List<TaskViewModel>>> GetAllTasks()
        {
            try
            {
                var tasks = await _httpService.Get<List<TaskModel>>(BaseUrl, RequestUri, null, null);
                var mappedTasks = _mapper.Map<List<TaskViewModel>>(tasks);
                return Response<List<TaskViewModel>>.Success(mappedTasks);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting all tasks");
                return Response<List<TaskViewModel>>.Failed("An error occurred while retrieving tasks");
            }
        }
        public async Task<Response<TaskViewModel>> GetTaskById(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                return Response<TaskViewModel>.Failed("Task ID is required");
            }

            try
            {
                var task = await _httpService.Get<TaskModel>(BaseUrl, $"{RequestUri}/{id}", null, null);
                var mappedTask = _mapper.Map<TaskViewModel>(task);
                return Response<TaskViewModel>.Success(mappedTask);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting task with ID {TaskId}", id);
                return Response<TaskViewModel>.Failed($"An error occurred while retrieving task with ID {id}");
            }
        }
        public async Task<Response<TaskViewModel>> CreateTask(CreateTaskDto createTaskDto)
        {
            if (createTaskDto == null)
            {
                return Response<TaskViewModel>.Failed("Task data is required");
            }

            try
            {
                var taskModel = _mapper.Map<TaskModel>(createTaskDto);
                var json = JsonConvert.SerializeObject(taskModel);

                var createdTask = await _httpService.Post<TaskModel>(json, BaseUrl, RequestUri, null, null);
                var mappedTask = _mapper.Map<TaskViewModel>(createdTask);

                return Response<TaskViewModel>.Success(mappedTask, "Task created successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating a new task");
                return Response<TaskViewModel>.Failed("An error occurred while creating the task");
            }
        }
        public async Task<Response<TaskViewModel>> UpdateTask(string id, UpdateTaskDto updateTaskDto)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                return Response<TaskViewModel>.Failed("Task ID is required");
            }

            if (updateTaskDto == null)
            {
                return Response<TaskViewModel>.Failed("Update data is required");
            }

            try
            {
                var existingTask = await _httpService.Get<TaskModel>(BaseUrl, $"{RequestUri}/{id}", null, null);
                _mapper.Map(updateTaskDto, existingTask);

                var json = JsonConvert.SerializeObject(existingTask);
                var updatedTask = await _httpService.Put<TaskModel>(json, BaseUrl, $"{RequestUri}/{id}", null, null);

                var mappedTask = _mapper.Map<TaskViewModel>(updatedTask);
                return Response<TaskViewModel>.Success(mappedTask, "Task updated successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while updating task with ID {TaskId}", id);
                return Response<TaskViewModel>.Failed($"An error occurred while updating task with ID {id}");
            }
        }
        public async Task<Response<bool>> DeleteTask(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                return Response<bool>.Failed("Task ID is required");
            }

            try
            {
                await _httpService.Delete<TaskModel>(string.Empty, BaseUrl, $"{RequestUri}/{id}", null, null);
                return Response<bool>.Success(true, "Task deleted successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while deleting task with ID {TaskId}", id);
                return Response<bool>.Failed($"An error occurred while deleting task with ID {id}");
            }
        }
    }
}
