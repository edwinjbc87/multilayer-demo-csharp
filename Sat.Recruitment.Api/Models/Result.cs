using System;
namespace Sat.Recruitment.Api.Models
{
    public class Result<T>
    {
        public bool IsSuccess { get; set; }
        public T Value { get; set; }
        public string Errors { get; set; }
        public string Message { get; set; }
    }

    public class Result
    {
        public bool IsSuccess { get; set; }
        public string Errors { get; set; }
        public string Message { get; set; }
    }
}
