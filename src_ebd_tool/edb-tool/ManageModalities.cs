﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace edb_tool
{
    public partial class ManageModalities : Form
    {
        //MySql db;

        BindingSource bs;

        int updateid;

        MainForm mainform;

        public ManageModalities(MainForm mainform)
        {
            InitializeComponent();
            this.CenterToScreen();
            this.mainform = mainform;

            #region gridview configuration
            dataGridView2.ReadOnly = true;
            dataGridView2.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridView2.RowHeadersVisible = false;
            dataGridView2.AllowUserToDeleteRows = true;
            dataGridView2.AllowUserToAddRows = false;
            dataGridView2.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridView2.CellContentClick += new DataGridViewCellEventHandler(dataGridView2_CellContentClick);
            dataGridView2.SelectionChanged += new EventHandler(dataGridView2_SelectionChanged);
            dataGridView2.AllowUserToResizeRows = false;
            dataGridView2.MultiSelect = false;
            dataGridView2.ColumnHeaderMouseClick += new DataGridViewCellMouseEventHandler(dataGridView2_ColumnHeaderMouseClick);
            #endregion

            button2.Visible = false;
        }

        void dataGridView2_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (dataGridView2.Columns[e.ColumnIndex].Name.ToLower() == "name")
            {
                var list = (from DataGridViewRow row in dataGridView2.Rows
                            let mod = row.DataBoundItem as GModality
                            orderby mod.name ascending
                            select mod).ToList();

                dataGridView2.DataSource = list;
            }
        }

        void dataGridView2_SelectionChanged(object sender, EventArgs e)
        {
            if (groupBox1.Text.ToLower().IndexOf("update") >= 0)
            {
                button2_Click(null,null);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (button1.Text.ToLower().IndexOf("add") >= 0)
            {
                GModality m = new GModality(-1, textBox1.Text, textBox2.Text, textBox3.Text);
                ProviderFactory.GetDataProvider().AddModality(m);
                mainform.comboBox4.DataSource = ProviderFactory.GetDataProvider().ListModalities();
                ClearControls();
            }
            else if (button1.Text.ToLower().IndexOf("update") >= 0)
            {
                GModality m = new GModality(updateid, textBox1.Text, textBox2.Text, textBox3.Text);
                ProviderFactory.GetDataProvider().UpdateModality(m);
                button2.Visible = false;
                button2_Click(null, null);
                mainform.comboBox4.DataSource = ProviderFactory.GetDataProvider().ListModalities();
            }

            Bind();
        }

        private void ManageModalities_Load(object sender, EventArgs e)
        {
            //db = new MySql();
            bs = new BindingSource();

            dataGridView2.DataSource = bs;

            Bind();

            if (dataGridView2.ColumnCount > 0)
                dataGridView2.Columns[0].Visible = false;

            DataGridViewLinkColumn Editlink = new DataGridViewLinkColumn();
            Editlink.UseColumnTextForLinkValue = true;
            Editlink.HeaderText = "Edit";
            Editlink.DataPropertyName = "lnkColumn";
            Editlink.LinkBehavior = LinkBehavior.SystemDefault;
            Editlink.Text = "Edit";
            dataGridView2.Columns.Add(Editlink);

            DataGridViewLinkColumn Deletelink = new DataGridViewLinkColumn();
            Deletelink.UseColumnTextForLinkValue = true;
            Deletelink.HeaderText = "delete";
            Deletelink.DataPropertyName = "lnkColumn";
            Deletelink.LinkBehavior = LinkBehavior.SystemDefault;
            Deletelink.Text = "Delete";
            dataGridView2.Columns.Add(Deletelink);
        }

        private void Bind()
        {
            bs.DataSource = ProviderFactory.GetDataProvider().ListModalities();
        }

        void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridView2.Columns[e.ColumnIndex].HeaderText.ToLower() == "delete")
            {
                var confirmResult = MessageBox.Show("Are you sure you want to delete this item ??", "Confirm Delete!!", MessageBoxButtons.YesNo);
                if (confirmResult == DialogResult.Yes)
                {
                    int idcolumn = Helper.LocateColumnInGrid("idmodality",dataGridView2);

                    if (idcolumn != -1)
                    {
                        object stringid = dataGridView2.Rows[e.RowIndex].Cells[idcolumn].Value;
                        int id = Convert.ToInt32(stringid);
                        ProviderFactory.GetDataProvider().DeleteModality(id);

                        Bind();
                        mainform.comboBox4.DataSource = ProviderFactory.GetDataProvider().ListModalities();
                        mainform.ConstructTabsModalities();
                    }
                }
            }
            else if (dataGridView2.Columns[e.ColumnIndex].HeaderText.ToLower() == "edit")
            {
                button1.Text = "Update";
                button2.Visible = true;
                groupBox1.Text = "Update entry";

                int idcolumn = Helper.LocateColumnInGrid("idmodality",dataGridView2);

                updateid = -1;
                if (idcolumn!=-1)
                {
                    object stringid = dataGridView2.Rows[e.RowIndex].Cells[idcolumn].Value;
                    updateid = Convert.ToInt32(stringid);
                }

                //set controls
                textBox1.Text = (string)dataGridView2.Rows[e.RowIndex].Cells[Helper.LocateColumnInGrid("name",dataGridView2)].Value;
                textBox2.Text = (string)dataGridView2.Rows[e.RowIndex].Cells[Helper.LocateColumnInGrid("comment",dataGridView2)].Value;
                textBox3.Text = (string)dataGridView2.Rows[e.RowIndex].Cells[Helper.LocateColumnInGrid("description",dataGridView2)].Value;
            }
        }

        private void button2_Click(object sender, EventArgs e) //cancel update
        {
            button1.Text = "Add";
            button2.Visible = false;
            groupBox1.Text = "Add new";

            ClearControls();
        }

        private void ClearControls()
        {
            textBox1.Text = "";
            textBox2.Text = "";
            textBox3.Text = "";
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}