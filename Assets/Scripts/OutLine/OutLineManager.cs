using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;


namespace Tianbo.Wang
{
    public class OutLineManager : SingleMono<OutLineManager>
    {
        public Dictionary<WindowsBase, List<OutLine>> windowsOutlines = new Dictionary<WindowsBase, List<OutLine>>();
        public Dictionary<GameObject, List<OutLine>> objectOutlines = new Dictionary<GameObject, List<OutLine>>();
        public OutLine GetOutLine(WindowsBase windowsBase, string key)
        {
            OutLine outLine;
            if (windowsOutlines.ContainsKey(windowsBase))
            {
                if (windowsOutlines[windowsBase] == null)
                {
                    windowsOutlines[windowsBase] = new List<OutLine>();
                }
                outLine = windowsOutlines[windowsBase].Find((p) =>
                {
                    return p.GetType().Name == key;
                });
                if (outLine == null)
                {
                    Type type = Type.GetType("Tianbo.Wang." + key);
                    outLine = (OutLine)Activator.CreateInstance(type, true);
                    if (outLine != null)
                        outLine.insLineName = key;
                    windowsOutlines[windowsBase].Add(outLine);
                }
            }
            else
            {
                Type type = Type.GetType("Tianbo.Wang." + key);
                outLine = (OutLine)Activator.CreateInstance(type, true);
                if (outLine != null)
                    outLine.insLineName = key;
                windowsOutlines.Add(windowsBase, new List<OutLine>() { outLine });
            }
            return outLine;
        }

        public OutLine GetOutLine(GameObject go, string key)
        {
            OutLine outLine;
            if (objectOutlines.ContainsKey(go))
            {
                if (objectOutlines[go] == null)
                {
                    objectOutlines[go] = new List<OutLine>();
                }
                outLine = objectOutlines[go].Find((p) => {
                    return p.GetType().Name == key;
                });
                if (outLine == null)
                {
                    Type type = Type.GetType("Tianbo.Wang." + key);
                    outLine = (OutLine)Activator.CreateInstance(type, true);
                    if (outLine != null)
                        outLine.insLineName = key;
                    objectOutlines[go].Add(outLine);
                }
            }
            else
            {
                Type type = Type.GetType("Tianbo.Wang." + key);
                outLine = (OutLine)Activator.CreateInstance(type, true);
                if (outLine != null)
                    outLine.insLineName = key;
                objectOutlines.Add(go, new List<OutLine>() { outLine });
            }
            return outLine;
        }

    }
}