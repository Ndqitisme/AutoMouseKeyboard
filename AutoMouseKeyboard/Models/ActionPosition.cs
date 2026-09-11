
using Newtonsoft.Json;

namespace AutoMouseKeyboard.Models
{
    public class ActionPosition
    {
    [JsonProperty("x")]
    public int X { get; set; }

    [JsonProperty("y")]
    public int Y { get; set; }
    }
}

