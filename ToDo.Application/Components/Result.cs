namespace ToDo.Application.Components
{ 
    public class Result
    {
        protected Result(bool isSuccess, string error)
        {
            if (isSuccess && !string.IsNullOrEmpty(error))
                throw new InvalidOperationException();

            if (!isSuccess && string.IsNullOrEmpty(error))
                throw new InvalidOperationException();

            IsSuccess = isSuccess;
            Error = error;
        }

        public bool IsSuccess { get; private set; }

        public bool IsFailure => !IsSuccess;

        public string Error { get; private set; }

        public static Result Ok()
        {
            return new Result(true, string.Empty);
        }

        public static Result Fail(string errorMessage) 
        { 
            return new Result(false, errorMessage);
        }

        public static Result<T> Ok<T>(T value)
        {
            return new Result<T>(true, string.Empty, value);
        }

        public static Result<T> Fail<T>(string errorMessage)
        {
            return new Result<T>(false, errorMessage, default(T));
        }
    }

    public class Result<T> : Result
    {
        protected internal Result(bool isSuccess, string error, T value) : base(isSuccess, error)
        {
            Value = value;
        }

        public T Value { get; private set; }
    }
}