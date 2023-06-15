﻿using FormsLib.Maths;
using FormsLib.Misc;
using System;
using System.Drawing;

namespace FormsLib.Scope
{
    public class ScopeController
    {

        public event EventHandler? DoRedraw;
        public ThreadedBindingList<Trace> Traces                { get; } = new ThreadedBindingList<Trace>();
        public ThreadedBindingList<Cursor> Cursors              { get; } = new ThreadedBindingList<Cursor>();
        public ThreadedBindingList<Marker> Markers              { get; } = new ThreadedBindingList<Marker>();
        public ThreadedBindingList<IScopeDrawable> Drawables    { get; } = new ThreadedBindingList<IScopeDrawable>();
        public ScopeViewSettings Settings                       { get; } = new ScopeViewSettings();


        public void RedrawAll()
        {
            DoRedraw?.Invoke(this, EventArgs.Empty);
        }
        
        public void Clear()
        {
            Cursors.Clear();
            Markers.Clear();
            Traces.Clear();
        }


        public void ClearData()
        {
            Cursors.Clear();
            Markers.Clear();
            foreach (var v in Traces)
                v.Points.Clear();
        }

        public static string TicksToString(double ticks)
        {
            DateTime dt = new DateTime((long)ticks);
            return dt.ToString("dd-MM-yyyy") + " \r\n" + dt.ToString("HH:mm:ss");
        }
    }

    public interface IScopeDrawable
    {
        PointD Point { get; set; }
        void Draw(Graphics g,  Func<PointD, Point> convert);

    }





}
