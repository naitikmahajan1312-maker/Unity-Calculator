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
        switch (index)
        {
            case 0: text.text = ""; appendText = "0"; break;
            case 1: break;
            case 2: break;
            case 3: appendText = "%"; break;
            case 4: appendText = "7"; break;
            case 5: appendText = "8"; break;
            case 6: appendText = "9"; break;
            case 7: appendText = "X"; break;
            case 8: appendText = "6"; break;
            case 9: appendText = "5"; break;
            case 10: appendText = "4"; break;
            case 11: appendText = "-"; break;
            case 12: appendText = "1"; break;
            case 13: appendText = "2"; break;
            case 14: appendText = "3"; break;
            case 15: appendText = "+"; break;
            case 16: break;
            case 17: appendText = "0"; break;
            case 18: appendText = "."; break;
            case 19: appendText = "="; break;
        }

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
            else if (ops[i] == '*' && i < nums.Count - 1)
            {
                nums[i] = nums[i] * nums[i + 1];
                nums.RemoveAt(i + 1);
                ops.RemoveAt(i);
                i--;
            }
            else if (ops[i] == '+' && i < nums.Count - 1)
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
