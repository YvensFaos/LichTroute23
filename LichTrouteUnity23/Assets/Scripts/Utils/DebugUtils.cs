using System;
using Server;
using UnityEngine;

namespace Utils
{
    public static class DebugUtils
    {
        public static void DebugArea(Vector3 position, float distance, float duration = 3.0f)
        {
            if (!LichServer.GetSingleton().DebugIsOn()) return;
            Debug.DrawLine(position, position + distance * Vector3.right, Color.blue, duration);
            Debug.DrawLine(position, position + distance * Vector3.up, Color.green, duration);
            Debug.DrawLine(position, position + distance * Vector3.forward, Color.red, duration);
        }

        public static void DebugLogMsg(string msg)
        {
            if (!LichServer.GetSingleton().DebugIsOn()) return;
            Debug.Log(msg);
        }

        public static void DebugAssertion(bool condition, string msg)
        {
            if (!LichServer.GetSingleton().DebugIsOn()) return;
            Debug.Assert(condition, msg);
        }
        
        public static void DebugLogErrorMsg(string msg)
        {
            if (!LichServer.GetSingleton().DebugIsOn()) return;
            Debug.LogError(msg);
        }

        public static void DebugLogException(Exception exception)
        {
            if (!LichServer.GetSingleton().DebugIsOn()) return;
            Debug.LogError(exception.ToString());
            Debug.LogError(exception.StackTrace);
        }
    }
}