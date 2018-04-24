using Newtonsoft.Json;

namespace YellowAntDemo.CommandCenter
{
    public class ButtonCommandsClass
    {
        [JsonProperty(PropertyName = "function_name")]
        public string FunctionName { get; set; }

        [JsonProperty(PropertyName = "service_application")]
        public int ServiceApplication { get; set; }

        [JsonProperty(PropertyName = "data")]
        public Command Data { get; set; }
    }

    public class Command
    {
        [JsonProperty(PropertyName = "Name")]
        public string Name { get; set; }

    }
}