using System.Windows.Forms;
namespace hearthdecktracker
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            this.btnClose = new System.Windows.Forms.Button();
            this.btnRefresh = new System.Windows.Forms.Button();
            this.cbDeckCardLists = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.gvDeckCardList = new System.Windows.Forms.DataGridView();
            this.mana = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.name = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.AtkDef = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Dmg = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Tgt = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Amt = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.gvDeckCardList)).BeginInit();
            this.SuspendLayout();
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnClose.ImageAlign = System.Drawing.ContentAlignment.TopRight;
            this.btnClose.Location = new System.Drawing.Point(290, 0);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(28, 26);
            this.btnClose.TabIndex = 0;
            this.btnClose.Text = "X";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.button1_Click);
            // 
            // btnRefresh
            // 
            this.btnRefresh.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnRefresh.BackgroundImage = global::hearthdecktracker.Properties.Resources.Refresh_icon;
            this.btnRefresh.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnRefresh.Location = new System.Drawing.Point(4, 2);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(36, 35);
            this.btnRefresh.TabIndex = 2;
            this.btnRefresh.UseVisualStyleBackColor = true;
            this.btnRefresh.Click += new System.EventHandler(this.button2_Click);
            // 
            // cbDeckCardLists
            // 
            this.cbDeckCardLists.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.cbDeckCardLists.DisplayMember = "Name";
            this.cbDeckCardLists.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbDeckCardLists.Font = new System.Drawing.Font("Franklin Gothic Medium", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbDeckCardLists.FormattingEnabled = true;
            this.cbDeckCardLists.Location = new System.Drawing.Point(51, 3);
            this.cbDeckCardLists.Name = "cbDeckCardLists";
            this.cbDeckCardLists.Size = new System.Drawing.Size(226, 34);
            this.cbDeckCardLists.TabIndex = 4;
            this.cbDeckCardLists.ValueMember = "Cardlist";
            this.cbDeckCardLists.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.label1.Location = new System.Drawing.Point(1, 44);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(320, 2);
            this.label1.TabIndex = 5;
            // 
            // label2
            // 
            this.label2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.label2.Location = new System.Drawing.Point(1, 90);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(320, 2);
            this.label2.TabIndex = 6;
            this.label2.Click += new System.EventHandler(this.label2_Click);
            // 
            // gvDeckCardList
            // 
            this.gvDeckCardList.AllowUserToAddRows = false;
            this.gvDeckCardList.AllowUserToDeleteRows = false;
            this.gvDeckCardList.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCellsExceptHeaders;
            this.gvDeckCardList.BackgroundColor = System.Drawing.SystemColors.ButtonFace;
            this.gvDeckCardList.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.gvDeckCardList.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            this.gvDeckCardList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gvDeckCardList.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.mana,
            this.name,
            this.AtkDef,
            this.Dmg,
            this.Tgt,
            this.Amt});
            this.gvDeckCardList.Cursor = System.Windows.Forms.Cursors.Default;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.gvDeckCardList.DefaultCellStyle = dataGridViewCellStyle1;
            this.gvDeckCardList.Location = new System.Drawing.Point(1, 92);
            this.gvDeckCardList.MultiSelect = false;
            this.gvDeckCardList.Name = "gvDeckCardList";
            this.gvDeckCardList.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            this.gvDeckCardList.RowHeadersVisible = false;
            this.gvDeckCardList.RowTemplate.Height = 25;
            this.gvDeckCardList.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gvDeckCardList.Size = new System.Drawing.Size(315, 700);
            this.gvDeckCardList.TabIndex = 8;
            this.gvDeckCardList.Visible = false;
            this.gvDeckCardList.CellMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dataGridView1_CellContentClick);
            // 
            // mana
            // 
            this.mana.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCellsExceptHeader;
            this.mana.Frozen = true;
            this.mana.HeaderText = "Mana";
            this.mana.Name = "mana";
            this.mana.ReadOnly = true;
            this.mana.Width = 5;
            // 
            // name
            // 
            this.name.Frozen = true;
            this.name.HeaderText = "Name";
            this.name.Name = "name";
            this.name.ReadOnly = true;
            this.name.Width = 155;
            // 
            // AtkDef
            // 
            this.AtkDef.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCellsExceptHeader;
            this.AtkDef.Frozen = true;
            this.AtkDef.HeaderText = "Atk/Def";
            this.AtkDef.Name = "AtkDef";
            this.AtkDef.ReadOnly = true;
            this.AtkDef.Width = 5;
            // 
            // Dmg
            // 
            this.Dmg.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCellsExceptHeader;
            this.Dmg.Frozen = true;
            this.Dmg.HeaderText = "Dmg/Heal/CAtt";
            this.Dmg.Name = "Dmg";
            this.Dmg.ReadOnly = true;
            this.Dmg.Width = 5;
            // 
            // Tgt
            // 
            this.Tgt.Frozen = true;
            this.Tgt.HeaderText = "Tgt";
            this.Tgt.Name = "Tgt";
            this.Tgt.ReadOnly = true;
            this.Tgt.Width = 40;
            // 
            // Amt
            // 
            this.Amt.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCellsExceptHeader;
            this.Amt.Frozen = true;
            this.Amt.HeaderText = "Amt";
            this.Amt.Name = "Amt";
            this.Amt.ReadOnly = true;
            this.Amt.Width = 5;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(320, 800);
            this.Controls.Add(this.gvDeckCardList);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnRefresh);
            this.Controls.Add(this.cbDeckCardLists);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "MainForm";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Form1_MouseDown);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.Form1_MouseMove);
            this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.Form1_MouseUp);
            ((System.ComponentModel.ISupportInitialize)(this.gvDeckCardList)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.ComboBox cbDeckCardLists;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.DataGridView gvDeckCardList;
        private DataGridViewTextBoxColumn mana;
        private DataGridViewTextBoxColumn name;
        private DataGridViewTextBoxColumn AtkDef;
        private DataGridViewTextBoxColumn Dmg;
        private DataGridViewTextBoxColumn Tgt;
        private DataGridViewTextBoxColumn Amt;
    }
}

