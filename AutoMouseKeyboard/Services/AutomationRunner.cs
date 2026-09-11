
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using AutoMouseKeyboard.Models;

namespace AutoMouseKeyboard.Services
{
    public class AutomationRunner
    {
    private readonly InputDispatcher _dispatcher;

    public AutomationRunner(InputDispatcher dispatcher)
    {
        _dispatcher = dispatcher;
    }

    public void Run(IEnumerable<ActionStep> actions, int loopCount, int globalDelay, CancellationToken token)
    {
        var steps = actions.ToList();
        if (steps.Count == 0)
        {
            throw new InvalidOperationException("Chuỗi hành động đang trống.");
        }

        loopCount = Math.Max(1, loopCount);
        globalDelay = Math.Max(0, globalDelay);

        for (var loop = 0; loop < loopCount; loop++)
        {
            token.ThrowIfCancellationRequested();
            foreach (var step in steps)
            {
                token.ThrowIfCancellationRequested();
                ExecuteStep(step, token);
                WaitWithCancellation(Math.Max(0, step.Delay) + globalDelay, token);
            }
        }
    }

    private void ExecuteStep(ActionStep step, CancellationToken token)
    {
        var repeat = Math.Max(1, step.Repeat);

        switch (step.Type)
        {
            case ActionKind.Mouse:
                ExecuteMouseStep(step, repeat, token);
                break;
            case ActionKind.Keyboard:
                ExecuteKeyboardStep(step, repeat, token);
                break;
            default:
                throw new InvalidOperationException(string.Format("Loại hành động chưa h�?tr�? {0}", step.Type));
        }
    }

    private void ExecuteMouseStep(ActionStep step, int repeat, CancellationToken token)
    {
        var modifiers = CollectModifierKeys(step);
        try
        {
            foreach (var key in modifiers)
            {
                _dispatcher.SendKeyDown(key);
            }

            for (var i = 0; i < repeat; i++)
            {
                token.ThrowIfCancellationRequested();
                _dispatcher.MoveCursor(step.X, step.Y);
                _dispatcher.SendMouse(step.MouseButton);

                if (i < repeat - 1)
                {
                    WaitWithCancellation(step.Delay, token);
                }
            }
        }
        finally
        {
            for (var i = modifiers.Count - 1; i >= 0; i--)
            {
                _dispatcher.SendKeyUp(modifiers[i]);
            }
        }
    }

    private void ExecuteKeyboardStep(ActionStep step, int repeat, CancellationToken token)
    {
        if (string.IsNullOrWhiteSpace(step.Key))
        {
            throw new InvalidOperationException("Hành động bàn phím không có giá trị phím.");
        }

        var modifiers = CollectModifierKeys(step);

        for (var i = 0; i < repeat; i++)
        {
            token.ThrowIfCancellationRequested();

            try
            {
                foreach (var key in modifiers)
                {
                    _dispatcher.SendKeyDown(key);
                }

                _dispatcher.SendKeySequence(step.Key!);
            }
            finally
            {
                for (var j = modifiers.Count - 1; j >= 0; j--)
                {
                    _dispatcher.SendKeyUp(modifiers[j]);
                }
            }

            if (i < repeat - 1)
            {
                WaitWithCancellation(step.Delay, token);
            }
        }
    }

    private static List<Keys> CollectModifierKeys(ActionStep step)
    {
        var modifiers = new List<Keys>(3);
        if (step.HoldCtrl)
        {
            modifiers.Add(Keys.ControlKey);
        }

        if (step.HoldAlt)
        {
            modifiers.Add(Keys.Menu);
        }

        if (step.HoldShift)
        {
            modifiers.Add(Keys.ShiftKey);
        }

        return modifiers;
    }

    private static void WaitWithCancellation(int milliseconds, CancellationToken token)
    {
        if (milliseconds <= 0)
        {
            return;
        }

        if (token.WaitHandle.WaitOne(milliseconds))
        {
            token.ThrowIfCancellationRequested();
        }
    }
    }
}

