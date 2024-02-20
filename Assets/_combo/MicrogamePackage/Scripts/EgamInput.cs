using UnityEngine;

public static class EgamInput
{
    // Input mapping
    private static readonly KeyCode[] LeftKeyCodes = 
    {
        KeyCode.A,
        KeyCode.LeftArrow
    };

    private static readonly KeyCode[] RightKeyCodes = 
    {
        KeyCode.D,
        KeyCode.RightArrow
    };

    private static readonly KeyCode[] UpKeyCodes = 
    {
        KeyCode.W,
        KeyCode.UpArrow
    };

    private static readonly KeyCode[] DownKeyCodes = 
    {
        KeyCode.S,
        KeyCode.DownArrow
    };

    private static readonly KeyCode[] ActionKeyCodes = 
    {
        KeyCode.Space,
        KeyCode.Return
    };

    private static readonly KeyCode[] AltActionKeyCodes = 
    {
        KeyCode.Z,
        KeyCode.LeftShift,
        KeyCode.RightShift
    };

    public enum Key
    {
        Up,
        Right,
        Down,
        Left,
        Action,
        AltAction
    }

    public enum State
    {
        Up,
        Down,
        Held
    }

    public enum Axis
    {
        Horizontal,
        Vertical
    }

    private static readonly string[] AxisNames = 
    {
        "Horizontal",
        "Vertical"
    };

    public static bool IsInputEnabled = true;

    public static bool GetKeyDown(Key key, params Key[] altKeys)
    {
        return GetValue(key, State.Down, altKeys);
    }

    public static bool GetKey(Key key, params Key[] altKeys)
    {
        return GetValue(key, State.Held);
    }

    public static bool GetKeyUp(Key key, params Key[] altKeys)
    {
        return GetValue(key, State.Up);
    }

    private static bool GetValue(Key key, State state, params Key[] altKeys)
    {
        // Active transition?
        bool isKey = false;
        if (IsInputEnabled)
        {
            // Query multiple keys
            isKey = GetValue(key, state);
            for (int i = 0; i < altKeys.Length; i++)
            {
                isKey |= GetValue(altKeys[i], state);
            }
        }
        return isKey;
    }

    private static bool GetValue(Key key, State state)
    {
        bool isKey = false;
        KeyCode[] keys = GetKeyCodes(key);
        foreach(KeyCode code in keys)
        {
            switch (state)
            {
                case State.Up:
                    isKey |= Input.GetKeyUp(code);
                    break;
                case State.Down:
                    isKey |= Input.GetKeyDown(code);
                    break;
                case State.Held:
                    isKey |= Input.GetKey(code);
                    break;
            }
        }

        // Check against the mouse too
        int mouseIndex = -1;
        switch (key)
        {
            case Key.Action:
                mouseIndex = 0;
                break;

            case Key.AltAction:
                mouseIndex = 1;
                break;
        }
        if (mouseIndex != -1)
        {
            switch (state)
            {
                case State.Up:
                    isKey |= Input.GetMouseButtonUp(mouseIndex);
                    break;
                case State.Down:
                    isKey |= Input.GetMouseButtonDown(mouseIndex);
                    break;
                case State.Held:
                    isKey |= Input.GetMouseButton(mouseIndex);
                    break;
            }
        }

        return isKey;
    }

    private static KeyCode[] GetKeyCodes(Key key)
    {
        KeyCode[] keys = ActionKeyCodes;
        switch (key)
        {
            case Key.Left:
                keys = LeftKeyCodes;
                break;
            case Key.Right:
                keys = RightKeyCodes;
                break;
            case Key.Up:
                keys = UpKeyCodes;
                break;
            case Key.Down:
                keys = DownKeyCodes;
                break;
            case Key.Action:
                keys = ActionKeyCodes;
                break;
            case Key.AltAction:
                keys = AltActionKeyCodes;
                break;
        }
        return keys;
    }

    public static float GetAxis(Axis axis)
    {
        float value = 0;
        if (IsInputEnabled)
        {
            string axisName = GetAxisName(axis);
            value = Input.GetAxis(axisName);
        }
        return value;
    }

    public static float GetAxisRaw(Axis axis)
    {
        float value = 0;
        if (IsInputEnabled)
        {
            string axisName = GetAxisName(axis);
            value = Input.GetAxisRaw(axisName);
        }
        return value;
    }

    private static string GetAxisName(Axis axis)
    {
        string axisName = AxisNames[(int)axis];
        return axisName;
    }
}
