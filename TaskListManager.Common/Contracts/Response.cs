using System.Collections.Immutable;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace TaskListManager.Common.Contracts
{
    public enum ResultType
    {
        [Description("Success")]
        Success = 1,
        [Description("Error")]
        Error = 2,
        [Description("ValidationError")]
        ValidationError = 3,
        [Description("Warning")]
        Warning = 4,
        [Description("CustomerInformation")]
        CustomerInformation = 5,
        [Description("Empty")]
        Empty = 6,
        [Description("Failed")]
        Failed,
        [Description("Request was successful")]
        Ok = 0,
        [Description("No record found")]
        NotFound = 2,
        [Description("Authentication failed. Please try again with the right credentials")]
        AuthorizationError = 4,
        [Description("Processing Error")]
        ProcessingError
    }
    public class Response<T>
    {
        public bool HasResult
        {
            get
            {
                return Result != null;
            }
        }
        public T Result { get; set; }
        public ResultType ResultType { get; set; }
        public string Message { get; set; }
        public ImmutableList<string> ValidationMessages { get; set; }

        public bool Successful
        {
            get
            {
                return ResultType == ResultType.Success || ResultType == ResultType.Warning;
            }
        }

        public static Response<T> Success(T result, string message = "Successful")
        {
            var response = new Response<T>
            {
                ResultType = ResultType.Success,
                Result = result,
                Message = message
            };
            return response;
        }

        public static Response<T> Failed(string errorMessage)
        {
            var response = new Response<T> { ResultType = ResultType.Error, Message = errorMessage };

            return response;
        }

        public static Response<T> ValidationError(IEnumerable<ValidationResult> validationMessages)
        {
            var response = new Response<T>
            {
                ResultType = ResultType.ValidationError,
                Message = validationMessages?.FirstOrDefault()?.ErrorMessage ?? "Response has validation errors",
                ValidationMessages = validationMessages.Select(error => error.ErrorMessage).ToImmutableList()
            };

            return response;
        }

        public static Response<T> ValidationError(List<string> validationMessages)
        {
            var response = new Response<T> { ResultType = ResultType.ValidationError, Message = "Response has validation errors", ValidationMessages = validationMessages.ToImmutableList() };

            return response;
        }

        public static Response<T> Warning(string warningMessage, T result)
        {
            var response = new Response<T>
            {
                ResultType = ResultType.Warning,
                Message = warningMessage,
                Result = result
            };

            return response;
        }

        public static Response<T> Failed(string errorMessage, ResultType responseCode = ResultType.Failed)
        {
            var response = new Response<T> { ResultType = ResultType.Error, Message = errorMessage };

            return response;
        }
        public static Response<T> Empty()
        {
            var response = new Response<T> { ResultType = ResultType.Empty };

            return response;
        }
    }

    public class Response
    {
        public Response()
        {
            this.ResultType = ResultType.Success;
        }

        public virtual bool HasResult
        {
            get
            {
                return false;
            }
        }

        public object Result { get; protected set; }

        public bool Successful
        {
            get
            {
                return this.ResultType == ResultType.Success || this.ResultType == ResultType.Warning;
            }
        }
        public ResultType ResultType { get; set; }
        public string Message { get; set; }
        public List<string> ValidationMessages { get; set; }

        public static Response Success()
        {
            var response = new Response
            {
                ResultType = ResultType.Success
            };
            return response;
        }

        public static Response Failed(string message)
        {
            var response = new Response { ResultType = ResultType.Error, Message = message };
            return response;
        }
        public static Response ValidationError(List<string> validationMessages)
        {
            var response = new Response { ResultType = ResultType.ValidationError, ValidationMessages = validationMessages };

            return response;
        }
        public static Response Warning(string warningMessage)
        {
            var response = new Response { ResultType = ResultType.Warning, Message = warningMessage };

            return response;
        }
        public static Response CustomerInformation(string customerInformationMessage)
        {
            var response = new Response
            {
                ResultType = ResultType.CustomerInformation,
                Message = customerInformationMessage
            };

            return response;
        }
        public static Response Empty()
        {
            var response = new Response { ResultType = ResultType.Empty };

            return response;
        }
        public class PagedList<T> where T : class
        {
            public PagedList()
            {
                Data = new List<T>();
            }
            public List<T> Data { get; set; }
            public int TotalCount { get; set; }
            public int PageSize { get; set; }
            public int CurrentPage { get; set; }
        }
        public class SalesPaginatedResponse<T>
        {
            public List<T> Data { get; set; }
            public int PageNumber { get; set; }
            public int PageSize { get; set; }
            public int TotalRecords { get; set; }
            public int TotalPages { get; set; }
        }
    }
}
