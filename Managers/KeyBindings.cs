using System;
using System.Collections.Generic;

namespace RatasInit.Managers;

public class KeyBindings
{
    public static KeyBindings DefaultBindings => new()
    {
        Bindings = [],
        SticksThreshold = 0.3f,
        TriggersTreshold = 0.15f
    };

    public Dictionary<Input, List<InputCode>> Bindings { get; set; } = [];

    public float SticksThreshold { get; set; }
    public float TriggersTreshold { get; set; }

    public void TryAddBinding(InputCode code, Input input)
    {
        if (Bindings.TryGetValue(input, out List<InputCode> value))
        {
            if (!value.Contains(code))
                value.Add(code);
        }
        else 
        {
            Bindings.Add(input, [code]);
        }
    }

    public List<Input>[] Reverse()
    {
        List<Input>[] inputs = new List<Input>[Enum.GetValues<InputCode>().Length - 1];
        foreach (var pair in Bindings)
        {
            foreach (var code in pair.Value)
            {
                if (inputs[(int)code] == null) inputs[(int)code] = [];
                inputs[(int)code].Add(pair.Key);
            }
        }
        for (int i = 0; i < inputs.Length; i++)
        {
            if (inputs[i] == null) inputs[i] = [];
        }
        return inputs;
    }

    public List<InputCode> GetInputCodes(Input input)
    {
        return Bindings.TryGetValue(input, out var code) ? code : [];
    }

    public List<Input> GetInputs(InputCode input)
    {
        List<Input> result = [];
        foreach (var pair in Bindings)
        {
            if (pair.Value.Contains(input))
            {
                result.Add(pair.Key);
            }
        }
        return result;
    }
}