﻿using CoreLib.Misc;
using FormsLib.Scope.Controls;

namespace FormsLib.Scope
{
    public class Marker : PropertySensitive
    {
        private static int nextId = 0;
        private static int NextId => nextId++;


        [TraceViewAttribute(Width = 25)]
        public int ID { get; /*private set;*/ } = NextId;
        [TraceViewAttribute(Width = 100)]
        public string Name { get => GetPar(""); set => SetPar(value); }
        public Pen Pen { get { return GetPar(new Pen(Color.White) { DashPattern = new float[] { 4.0F, 4.0F, 8.0F, 4.0F } }); } set { SetPar(value); } }
        [TraceViewAttribute(AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells)]
        public double X { get { return GetPar<double>(0); } set { SetPar<double>(value); } }
        public Marker Self { get { return this; } }
    }
}

