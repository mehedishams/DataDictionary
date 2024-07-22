namespace DataDictionary
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
            this.CurrentDatabaseLbl = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.TablesCmb = new System.Windows.Forms.ComboBox();
            this.CreateDictionaryBtn = new System.Windows.Forms.Button();
            this.ExportToExcelBtn = new System.Windows.Forms.Button();
            this.ColumnsGridView = new System.Windows.Forms.DataGridView();
            this.DatabasesCmb = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.TotalSizeLbl = new System.Windows.Forms.Label();
            this.PKGridView = new System.Windows.Forms.DataGridView();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.FKGridView = new System.Windows.Forms.DataGridView();
            this.ViewLiveExample = new System.Windows.Forms.RadioButton();
            this.ViewHardCodedExample = new System.Windows.Forms.RadioButton();
            ((System.ComponentModel.ISupportInitialize)(this.ColumnsGridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PKGridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.FKGridView)).BeginInit();
            this.SuspendLayout();
            // 
            // CurrentDatabaseLbl
            // 
            this.CurrentDatabaseLbl.AutoSize = true;
            this.CurrentDatabaseLbl.Location = new System.Drawing.Point(27, 28);
            this.CurrentDatabaseLbl.Name = "CurrentDatabaseLbl";
            this.CurrentDatabaseLbl.Size = new System.Drawing.Size(91, 13);
            this.CurrentDatabaseLbl.TabIndex = 0;
            this.CurrentDatabaseLbl.Text = "Current database:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(458, 51);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(66, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Select table:";
            // 
            // TablesCmb
            // 
            this.TablesCmb.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.TablesCmb.FormattingEnabled = true;
            this.TablesCmb.Location = new System.Drawing.Point(530, 48);
            this.TablesCmb.Name = "TablesCmb";
            this.TablesCmb.Size = new System.Drawing.Size(337, 21);
            this.TablesCmb.TabIndex = 2;
            this.TablesCmb.SelectedIndexChanged += new System.EventHandler(this.TablesCmb_SelectedIndexChanged);
            // 
            // CreateDictionaryBtn
            // 
            this.CreateDictionaryBtn.Location = new System.Drawing.Point(30, 81);
            this.CreateDictionaryBtn.Name = "CreateDictionaryBtn";
            this.CreateDictionaryBtn.Size = new System.Drawing.Size(119, 29);
            this.CreateDictionaryBtn.TabIndex = 3;
            this.CreateDictionaryBtn.Text = "Create Dictionary";
            this.CreateDictionaryBtn.UseVisualStyleBackColor = true;
            this.CreateDictionaryBtn.Click += new System.EventHandler(this.CreateDictionaryBtn_Click);
            // 
            // ExportToExcelBtn
            // 
            this.ExportToExcelBtn.Location = new System.Drawing.Point(317, 81);
            this.ExportToExcelBtn.Name = "ExportToExcelBtn";
            this.ExportToExcelBtn.Size = new System.Drawing.Size(119, 29);
            this.ExportToExcelBtn.TabIndex = 4;
            this.ExportToExcelBtn.Text = "Export to Excel";
            this.ExportToExcelBtn.UseVisualStyleBackColor = true;
            this.ExportToExcelBtn.Click += new System.EventHandler(this.ExportToExcelBtn_Click);
            // 
            // ColumnsGridView
            // 
            this.ColumnsGridView.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.ColumnsGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.ColumnsGridView.Location = new System.Drawing.Point(30, 135);
            this.ColumnsGridView.Name = "ColumnsGridView";
            this.ColumnsGridView.Size = new System.Drawing.Size(1386, 435);
            this.ColumnsGridView.TabIndex = 5;
            this.ColumnsGridView.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.ColumnsGridView_CellFormatting);
            this.ColumnsGridView.ColumnHeaderMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.ColumnsGridView_ColumnHeaderMouseClick);
            this.ColumnsGridView.DataBindingComplete += new System.Windows.Forms.DataGridViewBindingCompleteEventHandler(this.ColumnsGridView_DataBindingComplete);
            // 
            // DatabasesCmb
            // 
            this.DatabasesCmb.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.DatabasesCmb.FormattingEnabled = true;
            this.DatabasesCmb.Location = new System.Drawing.Point(120, 48);
            this.DatabasesCmb.Name = "DatabasesCmb";
            this.DatabasesCmb.Size = new System.Drawing.Size(316, 21);
            this.DatabasesCmb.TabIndex = 9;
            this.DatabasesCmb.SelectedIndexChanged += new System.EventHandler(this.DatabasesCmb_SelectedIndexChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(27, 51);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(87, 13);
            this.label2.TabIndex = 8;
            this.label2.Text = "Select database:";
            // 
            // TotalSizeLbl
            // 
            this.TotalSizeLbl.Location = new System.Drawing.Point(976, 618);
            this.TotalSizeLbl.Name = "TotalSizeLbl";
            this.TotalSizeLbl.Size = new System.Drawing.Size(440, 197);
            this.TotalSizeLbl.TabIndex = 10;
            this.TotalSizeLbl.Text = "Max size:";
            // 
            // PKGridView
            // 
            this.PKGridView.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.PKGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.PKGridView.Location = new System.Drawing.Point(30, 618);
            this.PKGridView.Name = "PKGridView";
            this.PKGridView.ReadOnly = true;
            this.PKGridView.Size = new System.Drawing.Size(468, 197);
            this.PKGridView.TabIndex = 11;
            this.PKGridView.ColumnHeaderMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.PKGridView_ColumnHeaderMouseClick);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(27, 592);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(149, 13);
            this.label3.TabIndex = 12;
            this.label3.Text = "Primary Keys and References:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(527, 592);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(150, 13);
            this.label4.TabIndex = 14;
            this.label4.Text = "Foreign Keys and References:";
            // 
            // FKGridView
            // 
            this.FKGridView.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.FKGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.FKGridView.Location = new System.Drawing.Point(530, 618);
            this.FKGridView.Name = "FKGridView";
            this.FKGridView.ReadOnly = true;
            this.FKGridView.Size = new System.Drawing.Size(431, 197);
            this.FKGridView.TabIndex = 13;
            this.FKGridView.ColumnHeaderMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.FKGridView_ColumnHeaderMouseClick);
            // 
            // ViewLiveExample
            // 
            this.ViewLiveExample.AutoSize = true;
            this.ViewLiveExample.Location = new System.Drawing.Point(753, 87);
            this.ViewLiveExample.Name = "ViewLiveExample";
            this.ViewLiveExample.Size = new System.Drawing.Size(262, 17);
            this.ViewLiveExample.TabIndex = 15;
            this.ViewLiveExample.TabStop = true;
            this.ViewLiveExample.Text = "View Live Example (mind the confidentiality issues)";
            this.ViewLiveExample.UseVisualStyleBackColor = true;
            this.ViewLiveExample.CheckedChanged += new System.EventHandler(this.ViewLiveExample_CheckedChanged);
            // 
            // ViewHardCodedExample
            // 
            this.ViewHardCodedExample.AutoSize = true;
            this.ViewHardCodedExample.Checked = true;
            this.ViewHardCodedExample.Location = new System.Drawing.Point(530, 87);
            this.ViewHardCodedExample.Name = "ViewHardCodedExample";
            this.ViewHardCodedExample.Size = new System.Drawing.Size(151, 17);
            this.ViewHardCodedExample.TabIndex = 16;
            this.ViewHardCodedExample.TabStop = true;
            this.ViewHardCodedExample.Text = "View Hard-Coded Example";
            this.ViewHardCodedExample.UseVisualStyleBackColor = true;
            this.ViewHardCodedExample.CheckedChanged += new System.EventHandler(this.ViewHardCodedExample_CheckedChanged);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1449, 870);
            this.Controls.Add(this.ViewHardCodedExample);
            this.Controls.Add(this.ViewLiveExample);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.FKGridView);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.PKGridView);
            this.Controls.Add(this.TotalSizeLbl);
            this.Controls.Add(this.DatabasesCmb);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.ColumnsGridView);
            this.Controls.Add(this.ExportToExcelBtn);
            this.Controls.Add(this.CreateDictionaryBtn);
            this.Controls.Add(this.TablesCmb);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.CurrentDatabaseLbl);
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.Text = "Data Dictionary v4.0";
            this.Load += new System.EventHandler(this.MainForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.ColumnsGridView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PKGridView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.FKGridView)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label CurrentDatabaseLbl;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox TablesCmb;
        private System.Windows.Forms.Button CreateDictionaryBtn;
        private System.Windows.Forms.Button ExportToExcelBtn;
        private System.Windows.Forms.DataGridView ColumnsGridView;
        private System.Windows.Forms.ComboBox DatabasesCmb;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label TotalSizeLbl;
        private System.Windows.Forms.DataGridView PKGridView;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.DataGridView FKGridView;
        private System.Windows.Forms.RadioButton ViewLiveExample;
        private System.Windows.Forms.RadioButton ViewHardCodedExample;
    }
}

