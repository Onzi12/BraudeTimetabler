using Microsoft.Extensions.Configuration;

namespace BraudeTimetabler.Tutorial
{
    public interface IGreeter
    {
        string GetGreeting();
    }
    public class Greeter : IGreeter
    {
        private string _greeting;

        public Greeter(IConfiguration configuration)
        {
            _greeting = configuration["HelloWorldH1"];
        }
        public string GetGreeting()
        {
            return _greeting;
        }
    }
}