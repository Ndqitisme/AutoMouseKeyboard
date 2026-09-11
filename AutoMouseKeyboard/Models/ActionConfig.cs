
using System.Collections.Generic;
using Newtonsoft.Json;

namespace AutoMouseKeyboard.Models
{
    public class ActionConfig
    {
    [JsonProperty("name")]
    public string Name { get; set; } = "Config";

    [JsonProperty("actions")]
    public List<ActionStep> Actions { get; set; } = new List<ActionStep>();

    public override string ToString() => Name;
    }
}

