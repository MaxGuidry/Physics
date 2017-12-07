using System.Collections;
using System.Collections.Generic;
using Cloth;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using System.Reflection;
public class SliderBehavior : MonoBehaviour
{
    //public List<Slider> sliders = new List<Slider>();
    public Slider slider;
    public Object objectToChange;

   
    public void ChangeValue(string s)
    {
        //List<string> vars = new List<string>();
        Dictionary<string, FieldInfo> vars = new Dictionary<string, FieldInfo>();
        List<FieldInfo> fields = getType(objectToChange).GetFields().ToList();
        foreach (var fieldInfo in fields)
        {
            vars.Add(fieldInfo.Name, fieldInfo);
        }

        vars[s].SetValue(objectToChange, slider.value);
    }
    public System.Type getType(object o)
    {
        return o.GetType();
    }
}
