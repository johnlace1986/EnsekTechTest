namespace EnsekTechTest.Core
{
    public class Result<TSuccess, TFailure>
    {
        private readonly TSuccess _success;
        private readonly TFailure _failure;

        public bool IsSuccess { get; private set; }

        public TSuccess Success
        {
            get
            {
                if (IsSuccess is false)
                    throw new InvalidOperationException();

                return _success;
            }
        }

        public TFailure Failure
        {
            get
            {
                if (IsSuccess)
                    throw new InvalidOperationException();

                return _failure;
            }
        }

        public Result(TSuccess success)
        {
            IsSuccess = true;
            _success = success;
        }

        public Result(TFailure failure)
        {
            IsSuccess = false;
            _failure = failure;
        }

        public static implicit operator Result<TSuccess, TFailure>(TSuccess success)
        {
            return new Result<TSuccess, TFailure>(success);
        }

        public static implicit operator Result<TSuccess, TFailure>(TFailure failure)
        {
            return new Result<TSuccess, TFailure>(failure);
        }
    }

    public class Unit
    {
        public static readonly Unit Instance = new();

        private Unit()
        {
        }
    }
}