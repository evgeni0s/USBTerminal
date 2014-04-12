using Microsoft.Research.DynamicDataDisplay;
using Microsoft.Research.DynamicDataDisplay.ViewportRestrictions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace USBTetminal2.Graphs
{

    //Helps to add [min, max] for axis
    //http://dynamicdatadisplay.codeplex.com/discussions/281164
    //Most likely there will be need to support Default settings, 
    //I'll export some extra code here
    public static class GraphAxisHelper
    {
        public class DisplayRange
        {
            public double Start { get; set; }
            public double End { get; set; }
            public DisplayRangeType Type { get; set; }

            public DisplayRange(double start, double end)
            {
                Start = start;
                End = end;
                Type = DisplayRangeType.MinMax;
            }

            public DisplayRange(double start, double end, DisplayRangeType type)
            {
                Start = start;
                End = end;
                Type = type;
            }

            public enum DisplayRangeType
            {
                Min,
                Max,
                MinMax
            }

        }

        public class ViewportAxesRangeRestriction : IViewportRestriction
        {

            public DisplayRange XRange = null;
            public DisplayRange YRange = null;


            public Rect Apply(Rect oldVisible, Rect newVisible, Viewport2D viewport)
            {

                if (XRange != null)
                {
                    switch (XRange.Type)
                    {
                        case DisplayRange.DisplayRangeType.Min:
                            newVisible.X = XRange.Start;//min
                            break;
                        case DisplayRange.DisplayRangeType.Max:
                            newVisible.Width = XRange.End - XRange.Start;//max
                            break;
                        case DisplayRange.DisplayRangeType.MinMax:
                            newVisible.X = XRange.Start;//min
                            newVisible.Width = XRange.End - XRange.Start;//max
                            break;
                        default:
                            break;
                    }
                }

                if (YRange != null)
                {
                    switch (YRange.Type)
                    {
                        case DisplayRange.DisplayRangeType.Min:
                            newVisible.Y = YRange.Start;//min
                            break;
                        case DisplayRange.DisplayRangeType.Max:
                            newVisible.Height = YRange.End - YRange.Start;//max
                            break;
                        case DisplayRange.DisplayRangeType.MinMax:
                            newVisible.Y = YRange.Start;//min
                            newVisible.Height = YRange.End - YRange.Start;//max
                            break;
                        default:
                            break;
                    }
                }

                return newVisible;
            }

#pragma warning disable 67
            public event EventHandler Changed;
#pragma warning restore 67
        }



    }
}
