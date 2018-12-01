using System.Reflection;
using UnityEngine.Assertions;

public class ReflectionHelper
{
   public static void CallMethod (string methodName, System.Object instance, params System.Object[] args)
    {
        try
        {
            instance.GetType().InvokeMember (methodName, BindingFlags.InvokeMethod, null, instance, args);
        }
        catch (System.Exception mme)
        {
            Assert.IsTrue (false, mme.ToString());
        }
    }
}