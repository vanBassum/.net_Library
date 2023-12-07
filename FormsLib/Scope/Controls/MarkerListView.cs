﻿using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using FormsLib.Scope.Models;

namespace FormsLib.Scope.Controls
{
    public partial class MarkerListView : UserControl
    {
        //DataGridView dataGridView = new DataGridView();
        //private GraphController dataSource;
        //public GraphController DataSource
        //{
        //    get { return dataSource; }
        //    set
        //    {
        //        dataSource = value;
        //        if (dataSource != null)
        //        {
        //            dataGridView.DataSource = dataSource.Markers;
        //            dataSource.Traces.ListChanged += Traces_ListChanged;
        //            dataSource.Markers.ListChanged += Cursors_ListChanged;
        //        }
        //    }
        //}



        //public MarkerListView()
        //{
        //    InitializeComponent();

        //    dataGridView.AutoGenerateColumns = false;
        //    dataGridView.Dock = DockStyle.Fill;
        //    Controls.Add(dataGridView);
        //    dataGridView.AllowUserToAddRows = false;
        //    dataGridView.AllowUserToDeleteRows = false;
        //    dataGridView.AllowUserToResizeRows = false;
        //    dataGridView.RowHeadersVisible = false;
        //    dataGridView.CellFormatting += DataGridView_CellFormatting;

        //    foreach (var pi in typeof(GraphMarker).GetProperties().Where(p => p.GetCustomAttribute<TraceViewAttribute>() != null))
        //    {
        //        TraceViewAttribute attr = pi.GetCustomAttribute<TraceViewAttribute>();
        //        DataGridViewColumn col;
        //        dataGridView.Columns.Add(col = new DataGridViewColumn(new DataGridViewTextBoxCell()));

        //        col.DataPropertyName = pi.Name;
        //        col.Name = pi.Name;
        //        col.HeaderText = attr.HeaderText == null ? pi.Name : attr.HeaderText;
        //        col.Width = attr.Width == 0 ? 100 : attr.Width;
        //        col.AutoSizeMode = attr.AutoSizeMode;
        //    }
        //}

        //private void Traces_ListChanged(object sender, ListChangedEventArgs e)
        //{
        //    //TODO: There is a lot of optimalisation possible here!
        //    //dataGridView.Columns.Clear();
            

        //    if (dataSource != null)
        //    {
        //        foreach (GraphTrace t in dataSource.Traces)
        //        {
        //            DataGridViewColumn? col = dataGridView.Columns.Cast<DataGridViewColumn>().FirstOrDefault(column => column.Tag != null && column.Tag == t);

        //            if (col == null)
        //            {
        //                col = new DataGridViewColumn(new DataGridViewTextBoxCell());
        //                dataGridView.Columns.Add(col = new DataGridViewColumn(new DataGridViewTextBoxCell()));
        //                col.Tag = t;
        //                col.Name = t.Name;
        //                col.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
        //            }

        //            col.HeaderText = t.Name;
        //        }
        //    }
        //    //dataGridView.AutoResizeColumns();
        //}



        //private void Cursors_ListChanged(object sender, ListChangedEventArgs e)
        //{
        //    dataGridView.Refresh();
        //}

        //private void DataGridView_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        //{
        //    DataGridView dgv = sender as DataGridView;

        //    GraphTrace t = dgv.Columns[e.ColumnIndex].Tag as GraphTrace;

        //    if (t != null)
        //    {
        //        e.Value = t.ToHumanReadable(t.GetYValue(DataSource.Markers[e.RowIndex].X));
        //    }

        //    if (dgv.Columns[e.ColumnIndex].DataPropertyName == nameof(GraphMarker.X))
        //    {
        //        e.Value = dataSource.Settings.HorizontalToHumanReadable((double)e.Value);
        //    }

        //}
    }
}