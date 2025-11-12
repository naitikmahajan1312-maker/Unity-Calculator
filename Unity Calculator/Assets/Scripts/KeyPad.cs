using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class KeyPad : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI text;
    private string appendText;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnKeyPadButtonTap(int index)
    {
        text.text = index == 0 ? "" : text.text;
        appendText = index switch
        {
            0 => "0",
            3 => "%",
            4 => "7",
            5 => "8",
            6 => "9",
            7 => "X",
            8 => "6",
            9 => "5",
            10 => "4",
            11 => "-",
            12 => "1",
            13 => "2",
            14 => "3",
            15 => "+",
            17 => "0",
            18 => ".",
            19 => "=",
            _ => appendText // for unused indexes
        };

        Debug.Log($"appendText - {appendText}");

        if (appendText == "=")
        {
            Calculate();
            return;
        }

        if (IsOperator(appendText))
        {
            if (text.text.Length > 0 && IsOperator(text.text[text.text.Length - 1].ToString()))
            {
                Debug.LogError("Ignored duplicate operator: " + appendText);
                return;
            }
        }
        if (text.text == "0" && !IsOperator(appendText))
        {
            text.text = appendText;
        }
        else
        {
            text.text += appendText;
        }
    }

    void Calculate()
    {
        string expr = text.text;
        Debug.Log("calculating: " + expr);

        if (string.IsNullOrEmpty(expr)) return;

        List<float> nums = new List<float>();
        List<char> ops = new List<char>();
        string num = "";

        foreach (char c in expr)
        {
            Debug.Log("c -> " + c);
            if (char.IsDigit(c) || c == '.')
            {
                num += c;
            }
            else if (c == '+' || c == '-' || c == '*' || c == '/' || c == '%' || c == 'X')
            {
                if (num != "")
                {
                    nums.Add(float.Parse(num));
                    num = "";
                }
                ops.Add(c);
            }
            Debug.Log("num - " + num);
            Debug.Log("nums count - " + nums.Count);
            Debug.Log("ops - " + ops.Count);
        }

        if (num != "")
            nums.Add(float.Parse(num));

        Debug.Log("nums: " + string.Join(",", nums));
        Debug.Log("operators: " + string.Join(",", ops));

        for (int i = 0; i < ops.Count; i++)
        {
            Debug.Log("FOR DIVISION");
            Debug.Log("nums -> " + nums.Count + " -> " + i);
            Debug.Log("nums i -> " + nums[i]);
            Debug.Log("ops -> " + ops.Count + " -> " + i);
            Debug.Log("ops i -> " + ops[i]);
            if (ops[i] == '%' && i < nums.Count - 1)
            {
                nums[i] = nums[i] / nums[i + 1];
                nums.RemoveAt(i + 1);
                ops.RemoveAt(i);
                i--;
            }
        }
        for (int i = 0; i < ops.Count; i++)
        {
            Debug.Log("FOR MULTIPLICATION");
            Debug.Log("nums -> " + nums.Count + " -> " + i);
            Debug.Log("nums i -> " + nums[i]);
            Debug.Log("ops -> " + ops.Count + " -> " + i);
            Debug.Log("ops i -> " + ops[i]);
            if (ops[i] == 'X' && i < nums.Count - 1)
            {
                nums[i] = nums[i] * nums[i + 1];
                nums.RemoveAt(i + 1);
                ops.RemoveAt(i);
                i--;
            }
        }
        for (int i = 0; i < ops.Count; i++)
        {
            Debug.Log("FOR ADD AND SUBSTRACT");
            Debug.Log("nums -> " + nums.Count + " -> " + i);
            Debug.Log("nums i -> " + nums[i]);
            Debug.Log("ops -> " + ops.Count + " -> " + i);
            Debug.Log("ops i -> " + ops[i]);
            if (ops[i] == '+' && i < nums.Count - 1)
            {
                nums[i] = nums[i] + nums[i + 1];
                nums.RemoveAt(i + 1);
                ops.RemoveAt(i);
                i--;
            }
            else if (ops[i] == '-' && i < nums.Count - 1)
            {
                nums[i] = nums[i] - nums[i + 1];
                nums.RemoveAt(i + 1);
                ops.RemoveAt(i);
                i--;
            }
        }

        float result = nums.Count > 0 ? nums[0] : 0f;
        Debug.LogError("Result: " + result);
        text.text = result.ToString();
    }

    bool IsOperator(string c)
    {
        return c == "+" || c == "-" || c == "X" || c == "/" || c == "%" || c == ".";
    }
}
