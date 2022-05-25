using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Notebook.Core
{
    public class DomainResult
    {
        public string ErrorMessage { get; }
        public bool Succeeded { get; }

        protected DomainResult()
        {
            Succeeded = true;
        }

        protected DomainResult(string error)
        {
            Succeeded = false;
            ErrorMessage = error;
        }

        public static DomainResult Success() => new();
        public static DomainResult Fail(string error) => new(error);
    }


    public sealed class DomainResult<T> : DomainResult
    {
        public T Result { get; }

        private DomainResult(T result)
        {
            Result = result;
        }

        private DomainResult(string error) : base(error) { }

        public static DomainResult<T> Success(T result) => new(result);

        public static new DomainResult<T> Fail(string error) => new(error);
    }        
}
