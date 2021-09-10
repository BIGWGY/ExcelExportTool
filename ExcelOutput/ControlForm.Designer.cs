namespace ExcelTool
{
    partial class ControlForm
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
	        this.TableCheckBoxList = new System.Windows.Forms.CheckedListBox();
	        this.OpenExcelDataPath = new System.Windows.Forms.Button();
	        this.SelectAllbutton = new System.Windows.Forms.Button();
	        this.ResetAllButton = new System.Windows.Forms.Button();
	        this.ReverseSelectButton = new System.Windows.Forms.Button();
	        this.ExportButton = new System.Windows.Forms.Button();
	        this.ExcelPathText = new System.Windows.Forms.Label();
	        this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
	        this.fileSystemWatcher1 = new System.IO.FileSystemWatcher();
	        this.groupBox1 = new System.Windows.Forms.GroupBox();
	        this.EnumLabel = new System.Windows.Forms.Label();
	        this.OpenEnumPathButton = new System.Windows.Forms.Button();
	        this.ExcelFilterTextBox = new System.Windows.Forms.TextBox();
	        this.FilterExcelButton = new System.Windows.Forms.Button();
	        this.OpenBinaryOutputPath = new System.Windows.Forms.Button();
	        this.BinaryOutPath = new System.Windows.Forms.TextBox();
	        this.BinaryCheckBox = new System.Windows.Forms.CheckBox();
	        this.JsonOutPath = new System.Windows.Forms.TextBox();
	        this.JsonCheckBox = new System.Windows.Forms.CheckBox();
	        this.button2 = new System.Windows.Forms.Button();
	        this.groupBox2 = new System.Windows.Forms.GroupBox();
	        this.button4 = new System.Windows.Forms.Button();
	        this.ClientJsonPath = new System.Windows.Forms.TextBox();
	        this.ClientJsonCheckBox = new System.Windows.Forms.CheckBox();
	        this.AutoCodeNamespace = new System.Windows.Forms.Label();
	        this.CsNamespaceEdit = new System.Windows.Forms.TextBox();
	        this.button3 = new System.Windows.Forms.Button();
	        this.ExportTableCodeCheckBox = new System.Windows.Forms.CheckBox();
	        this.CsTableCodeOutputPath = new System.Windows.Forms.TextBox();
	        this.button1 = new System.Windows.Forms.Button();
	        this.ExportEnumCodeCheckbox = new System.Windows.Forms.CheckBox();
	        this.CsEnumCodeOutputPath = new System.Windows.Forms.TextBox();
	        this.label1 = new System.Windows.Forms.Label();
	        this.ExportTestCodeButton = new System.Windows.Forms.Button();
	        this.LogOutputText = new System.Windows.Forms.RichTextBox();
	        ((System.ComponentModel.ISupportInitialize) (this.fileSystemWatcher1)).BeginInit();
	        this.groupBox1.SuspendLayout();
	        this.groupBox2.SuspendLayout();
	        this.SuspendLayout();
	        // 
	        // TableCheckBoxList
	        // 
	        this.TableCheckBoxList.CheckOnClick = true;
	        this.TableCheckBoxList.FormattingEnabled = true;
	        this.TableCheckBoxList.Location = new System.Drawing.Point(6, 115);
	        this.TableCheckBoxList.Name = "TableCheckBoxList";
	        this.TableCheckBoxList.Size = new System.Drawing.Size(352, 196);
	        this.TableCheckBoxList.TabIndex = 1;
	        this.TableCheckBoxList.SelectedIndexChanged += new System.EventHandler(this.checkedListBox1_SelectedIndexChanged);
	        // 
	        // OpenExcelDataPath
	        // 
	        this.OpenExcelDataPath.Location = new System.Drawing.Point(283, 16);
	        this.OpenExcelDataPath.Name = "OpenExcelDataPath";
	        this.OpenExcelDataPath.Size = new System.Drawing.Size(75, 23);
	        this.OpenExcelDataPath.TabIndex = 3;
	        this.OpenExcelDataPath.Text = "表格文件夹";
	        this.OpenExcelDataPath.UseVisualStyleBackColor = true;
	        this.OpenExcelDataPath.Click += new System.EventHandler(this.SelectExcelDataPathButtonClick);
	        // 
	        // SelectAllbutton
	        // 
	        this.SelectAllbutton.Location = new System.Drawing.Point(90, 556);
	        this.SelectAllbutton.Name = "SelectAllbutton";
	        this.SelectAllbutton.Size = new System.Drawing.Size(76, 25);
	        this.SelectAllbutton.TabIndex = 5;
	        this.SelectAllbutton.Text = "全选";
	        this.SelectAllbutton.UseVisualStyleBackColor = true;
	        this.SelectAllbutton.Click += new System.EventHandler(this.SelectAllButtonClick);
	        // 
	        // ResetAllButton
	        // 
	        this.ResetAllButton.Location = new System.Drawing.Point(8, 556);
	        this.ResetAllButton.Name = "ResetAllButton";
	        this.ResetAllButton.Size = new System.Drawing.Size(76, 25);
	        this.ResetAllButton.TabIndex = 6;
	        this.ResetAllButton.Text = "重置";
	        this.ResetAllButton.UseVisualStyleBackColor = true;
	        this.ResetAllButton.Click += new System.EventHandler(this.ResetButtonClick);
	        // 
	        // ReverseSelectButton
	        // 
	        this.ReverseSelectButton.Location = new System.Drawing.Point(172, 556);
	        this.ReverseSelectButton.Name = "ReverseSelectButton";
	        this.ReverseSelectButton.Size = new System.Drawing.Size(76, 25);
	        this.ReverseSelectButton.TabIndex = 7;
	        this.ReverseSelectButton.Text = "反选";
	        this.ReverseSelectButton.UseVisualStyleBackColor = true;
	        this.ReverseSelectButton.Click += new System.EventHandler(this.ReverseButtonClick);
	        // 
	        // ExportButton
	        // 
	        this.ExportButton.Location = new System.Drawing.Point(352, 556);
	        this.ExportButton.Name = "ExportButton";
	        this.ExportButton.Size = new System.Drawing.Size(76, 25);
	        this.ExportButton.TabIndex = 8;
	        this.ExportButton.Text = "导出";
	        this.ExportButton.UseVisualStyleBackColor = true;
	        this.ExportButton.Click += new System.EventHandler(this.ExportButtonClick);
	        // 
	        // ExcelPathText
	        // 
	        this.ExcelPathText.AutoSize = true;
	        this.ExcelPathText.Location = new System.Drawing.Point(6, 21);
	        this.ExcelPathText.Name = "ExcelPathText";
	        this.ExcelPathText.Size = new System.Drawing.Size(0, 12);
	        this.ExcelPathText.TabIndex = 11;
	        this.ExcelPathText.Click += new System.EventHandler(this.ExcelPathText_Click);
	        // 
	        // fileSystemWatcher1
	        // 
	        this.fileSystemWatcher1.EnableRaisingEvents = true;
	        this.fileSystemWatcher1.SynchronizingObject = this;
	        // 
	        // groupBox1
	        // 
	        this.groupBox1.Controls.Add(this.EnumLabel);
	        this.groupBox1.Controls.Add(this.OpenEnumPathButton);
	        this.groupBox1.Controls.Add(this.ExcelFilterTextBox);
	        this.groupBox1.Controls.Add(this.FilterExcelButton);
	        this.groupBox1.Controls.Add(this.OpenExcelDataPath);
	        this.groupBox1.Controls.Add(this.ExcelPathText);
	        this.groupBox1.Controls.Add(this.TableCheckBoxList);
	        this.groupBox1.Location = new System.Drawing.Point(12, 12);
	        this.groupBox1.Name = "groupBox1";
	        this.groupBox1.Size = new System.Drawing.Size(364, 313);
	        this.groupBox1.TabIndex = 18;
	        this.groupBox1.TabStop = false;
	        this.groupBox1.Text = "表格";
	        this.groupBox1.Enter += new System.EventHandler(this.groupBox1_Enter);
	        // 
	        // EnumLabel
	        // 
	        this.EnumLabel.Location = new System.Drawing.Point(6, 55);
	        this.EnumLabel.Name = "EnumLabel";
	        this.EnumLabel.Size = new System.Drawing.Size(271, 20);
	        this.EnumLabel.TabIndex = 44;
	        this.EnumLabel.Text = "label2";
	        // 
	        // OpenEnumPathButton
	        // 
	        this.OpenEnumPathButton.Location = new System.Drawing.Point(283, 52);
	        this.OpenEnumPathButton.Name = "OpenEnumPathButton";
	        this.OpenEnumPathButton.Size = new System.Drawing.Size(75, 23);
	        this.OpenEnumPathButton.TabIndex = 43;
	        this.OpenEnumPathButton.Text = "枚举文件夹";
	        this.OpenEnumPathButton.UseVisualStyleBackColor = true;
	        this.OpenEnumPathButton.Click += new System.EventHandler(this.OpenEnumPathButton_Click);
	        // 
	        // ExcelFilterTextBox
	        // 
	        this.ExcelFilterTextBox.Location = new System.Drawing.Point(6, 88);
	        this.ExcelFilterTextBox.Name = "ExcelFilterTextBox";
	        this.ExcelFilterTextBox.Size = new System.Drawing.Size(271, 21);
	        this.ExcelFilterTextBox.TabIndex = 42;
	        // 
	        // FilterExcelButton
	        // 
	        this.FilterExcelButton.Location = new System.Drawing.Point(283, 88);
	        this.FilterExcelButton.Name = "FilterExcelButton";
	        this.FilterExcelButton.Size = new System.Drawing.Size(75, 23);
	        this.FilterExcelButton.TabIndex = 12;
	        this.FilterExcelButton.Text = "查找";
	        this.FilterExcelButton.UseVisualStyleBackColor = true;
	        this.FilterExcelButton.Click += new System.EventHandler(this.FilterExcelButton_Click);
	        // 
	        // OpenBinaryOutputPath
	        // 
	        this.OpenBinaryOutputPath.Location = new System.Drawing.Point(332, 16);
	        this.OpenBinaryOutputPath.Name = "OpenBinaryOutputPath";
	        this.OpenBinaryOutputPath.Size = new System.Drawing.Size(26, 23);
	        this.OpenBinaryOutputPath.TabIndex = 4;
	        this.OpenBinaryOutputPath.Text = "..";
	        this.OpenBinaryOutputPath.UseVisualStyleBackColor = true;
	        this.OpenBinaryOutputPath.Click += new System.EventHandler(this.SelectExportBinaryPathButtonClick);
	        // 
	        // BinaryOutPath
	        // 
	        this.BinaryOutPath.Location = new System.Drawing.Point(108, 18);
	        this.BinaryOutPath.Name = "BinaryOutPath";
	        this.BinaryOutPath.ReadOnly = true;
	        this.BinaryOutPath.Size = new System.Drawing.Size(218, 21);
	        this.BinaryOutPath.TabIndex = 16;
	        // 
	        // BinaryCheckBox
	        // 
	        this.BinaryCheckBox.Location = new System.Drawing.Point(6, 20);
	        this.BinaryCheckBox.Name = "BinaryCheckBox";
	        this.BinaryCheckBox.Size = new System.Drawing.Size(84, 16);
	        this.BinaryCheckBox.TabIndex = 17;
	        this.BinaryCheckBox.Text = "导出二进制";
	        this.BinaryCheckBox.UseVisualStyleBackColor = true;
	        this.BinaryCheckBox.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
	        // 
	        // JsonOutPath
	        // 
	        this.JsonOutPath.Location = new System.Drawing.Point(108, 52);
	        this.JsonOutPath.Name = "JsonOutPath";
	        this.JsonOutPath.ReadOnly = true;
	        this.JsonOutPath.Size = new System.Drawing.Size(218, 21);
	        this.JsonOutPath.TabIndex = 34;
	        this.JsonOutPath.TextChanged += new System.EventHandler(this.textBox2_TextChanged_1);
	        // 
	        // JsonCheckBox
	        // 
	        this.JsonCheckBox.Location = new System.Drawing.Point(6, 49);
	        this.JsonCheckBox.Name = "JsonCheckBox";
	        this.JsonCheckBox.Size = new System.Drawing.Size(75, 24);
	        this.JsonCheckBox.TabIndex = 36;
	        this.JsonCheckBox.Text = "导出Json";
	        this.JsonCheckBox.UseVisualStyleBackColor = true;
	        this.JsonCheckBox.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged_1);
	        // 
	        // button2
	        // 
	        this.button2.Location = new System.Drawing.Point(332, 49);
	        this.button2.Name = "button2";
	        this.button2.Size = new System.Drawing.Size(26, 23);
	        this.button2.TabIndex = 37;
	        this.button2.Text = "..";
	        this.button2.UseVisualStyleBackColor = true;
	        this.button2.Click += new System.EventHandler(this.SelectJsonOutputButtonClick);
	        // 
	        // groupBox2
	        // 
	        this.groupBox2.Controls.Add(this.button4);
	        this.groupBox2.Controls.Add(this.ClientJsonPath);
	        this.groupBox2.Controls.Add(this.ClientJsonCheckBox);
	        this.groupBox2.Controls.Add(this.AutoCodeNamespace);
	        this.groupBox2.Controls.Add(this.CsNamespaceEdit);
	        this.groupBox2.Controls.Add(this.button3);
	        this.groupBox2.Controls.Add(this.ExportTableCodeCheckBox);
	        this.groupBox2.Controls.Add(this.CsTableCodeOutputPath);
	        this.groupBox2.Controls.Add(this.button1);
	        this.groupBox2.Controls.Add(this.ExportEnumCodeCheckbox);
	        this.groupBox2.Controls.Add(this.CsEnumCodeOutputPath);
	        this.groupBox2.Controls.Add(this.button2);
	        this.groupBox2.Controls.Add(this.JsonCheckBox);
	        this.groupBox2.Controls.Add(this.JsonOutPath);
	        this.groupBox2.Controls.Add(this.BinaryCheckBox);
	        this.groupBox2.Controls.Add(this.BinaryOutPath);
	        this.groupBox2.Controls.Add(this.OpenBinaryOutputPath);
	        this.groupBox2.Location = new System.Drawing.Point(382, 12);
	        this.groupBox2.Name = "groupBox2";
	        this.groupBox2.Size = new System.Drawing.Size(364, 311);
	        this.groupBox2.TabIndex = 19;
	        this.groupBox2.TabStop = false;
	        this.groupBox2.Text = "导出选项";
	        this.groupBox2.Enter += new System.EventHandler(this.groupBox2_Enter);
	        // 
	        // button4
	        // 
	        this.button4.Location = new System.Drawing.Point(332, 206);
	        this.button4.Name = "button4";
	        this.button4.Size = new System.Drawing.Size(26, 23);
	        this.button4.TabIndex = 48;
	        this.button4.Text = "..";
	        this.button4.UseVisualStyleBackColor = true;
	        this.button4.Click += new System.EventHandler(this.button4_Click);
	        // 
	        // ClientJsonPath
	        // 
	        this.ClientJsonPath.Location = new System.Drawing.Point(161, 208);
	        this.ClientJsonPath.Name = "ClientJsonPath";
	        this.ClientJsonPath.ReadOnly = true;
	        this.ClientJsonPath.Size = new System.Drawing.Size(165, 21);
	        this.ClientJsonPath.TabIndex = 47;
	        this.ClientJsonPath.TextChanged += new System.EventHandler(this.textBox1_TextChanged_2);
	        // 
	        // ClientJsonCheckBox
	        // 
	        this.ClientJsonCheckBox.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
	        this.ClientJsonCheckBox.Location = new System.Drawing.Point(6, 208);
	        this.ClientJsonCheckBox.Name = "ClientJsonCheckBox";
	        this.ClientJsonCheckBox.Size = new System.Drawing.Size(203, 21);
	        this.ClientJsonCheckBox.TabIndex = 46;
	        this.ClientJsonCheckBox.Text = "导出单个客户端json文件";
	        this.ClientJsonCheckBox.UseVisualStyleBackColor = true;
	        this.ClientJsonCheckBox.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged_2);
	        // 
	        // AutoCodeNamespace
	        // 
	        this.AutoCodeNamespace.Location = new System.Drawing.Point(6, 166);
	        this.AutoCodeNamespace.Name = "AutoCodeNamespace";
	        this.AutoCodeNamespace.Size = new System.Drawing.Size(84, 21);
	        this.AutoCodeNamespace.TabIndex = 45;
	        this.AutoCodeNamespace.Text = "CS的命名空间";
	        // 
	        // CsNamespaceEdit
	        // 
	        this.CsNamespaceEdit.Location = new System.Drawing.Point(108, 163);
	        this.CsNamespaceEdit.Name = "CsNamespaceEdit";
	        this.CsNamespaceEdit.Size = new System.Drawing.Size(218, 21);
	        this.CsNamespaceEdit.TabIndex = 44;
	        this.CsNamespaceEdit.TextChanged += new System.EventHandler(this.CsCodeNamespace_TextChanged);
	        // 
	        // button3
	        // 
	        this.button3.Location = new System.Drawing.Point(332, 124);
	        this.button3.Name = "button3";
	        this.button3.Size = new System.Drawing.Size(26, 23);
	        this.button3.TabIndex = 43;
	        this.button3.Text = "..";
	        this.button3.UseVisualStyleBackColor = true;
	        this.button3.Click += new System.EventHandler(this.SelectTableCodeOutputPath);
	        // 
	        // ExportTableCodeCheckBox
	        // 
	        this.ExportTableCodeCheckBox.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
	        this.ExportTableCodeCheckBox.Location = new System.Drawing.Point(6, 127);
	        this.ExportTableCodeCheckBox.Name = "ExportTableCodeCheckBox";
	        this.ExportTableCodeCheckBox.Size = new System.Drawing.Size(96, 21);
	        this.ExportTableCodeCheckBox.TabIndex = 39;
	        this.ExportTableCodeCheckBox.Text = "导出CSTable";
	        this.ExportTableCodeCheckBox.UseVisualStyleBackColor = true;
	        this.ExportTableCodeCheckBox.CheckedChanged += new System.EventHandler(this.checkBox2_CheckedChanged);
	        // 
	        // CsTableCodeOutputPath
	        // 
	        this.CsTableCodeOutputPath.Location = new System.Drawing.Point(108, 127);
	        this.CsTableCodeOutputPath.Name = "CsTableCodeOutputPath";
	        this.CsTableCodeOutputPath.ReadOnly = true;
	        this.CsTableCodeOutputPath.Size = new System.Drawing.Size(218, 21);
	        this.CsTableCodeOutputPath.TabIndex = 41;
	        this.CsTableCodeOutputPath.TextChanged += new System.EventHandler(this.CsTableCodeOutputPath_TextChanged);
	        // 
	        // button1
	        // 
	        this.button1.Location = new System.Drawing.Point(332, 88);
	        this.button1.Name = "button1";
	        this.button1.Size = new System.Drawing.Size(26, 23);
	        this.button1.TabIndex = 40;
	        this.button1.Text = "..";
	        this.button1.UseVisualStyleBackColor = true;
	        this.button1.Click += new System.EventHandler(this.SelectEnumCodeOutputPath);
	        // 
	        // ExportEnumCodeCheckbox
	        // 
	        this.ExportEnumCodeCheckbox.Location = new System.Drawing.Point(6, 88);
	        this.ExportEnumCodeCheckbox.Name = "ExportEnumCodeCheckbox";
	        this.ExportEnumCodeCheckbox.Size = new System.Drawing.Size(75, 24);
	        this.ExportEnumCodeCheckbox.TabIndex = 39;
	        this.ExportEnumCodeCheckbox.Text = "导出枚举";
	        this.ExportEnumCodeCheckbox.UseVisualStyleBackColor = true;
	        this.ExportEnumCodeCheckbox.CheckedChanged += new System.EventHandler(this.checkBox3_CheckedChanged);
	        // 
	        // CsEnumCodeOutputPath
	        // 
	        this.CsEnumCodeOutputPath.Location = new System.Drawing.Point(108, 91);
	        this.CsEnumCodeOutputPath.Name = "CsEnumCodeOutputPath";
	        this.CsEnumCodeOutputPath.ReadOnly = true;
	        this.CsEnumCodeOutputPath.Size = new System.Drawing.Size(218, 21);
	        this.CsEnumCodeOutputPath.TabIndex = 38;
	        this.CsEnumCodeOutputPath.TextChanged += new System.EventHandler(this.textBox1_TextChanged_1);
	        // 
	        // label1
	        // 
	        this.label1.Location = new System.Drawing.Point(817, 45);
	        this.label1.Name = "label1";
	        this.label1.Size = new System.Drawing.Size(100, 23);
	        this.label1.TabIndex = 20;
	        this.label1.Text = "label1";
	        // 
	        // ExportTestCodeButton
	        // 
	        this.ExportTestCodeButton.Location = new System.Drawing.Point(254, 556);
	        this.ExportTestCodeButton.Name = "ExportTestCodeButton";
	        this.ExportTestCodeButton.Size = new System.Drawing.Size(92, 25);
	        this.ExportTestCodeButton.TabIndex = 21;
	        this.ExportTestCodeButton.Text = "导出CS测试类";
	        this.ExportTestCodeButton.UseVisualStyleBackColor = true;
	        this.ExportTestCodeButton.Click += new System.EventHandler(this.ExportTestCodeButton_Click);
	        // 
	        // LogOutputText
	        // 
	        this.LogOutputText.Location = new System.Drawing.Point(12, 331);
	        this.LogOutputText.Name = "LogOutputText";
	        this.LogOutputText.Size = new System.Drawing.Size(702, 219);
	        this.LogOutputText.TabIndex = 44;
	        this.LogOutputText.Text = "";
	        // 
	        // ControlForm
	        // 
	        this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
	        this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
	        this.BackColor = System.Drawing.SystemColors.Control;
	        this.ClientSize = new System.Drawing.Size(749, 590);
	        this.Controls.Add(this.LogOutputText);
	        this.Controls.Add(this.ExportTestCodeButton);
	        this.Controls.Add(this.ResetAllButton);
	        this.Controls.Add(this.SelectAllbutton);
	        this.Controls.Add(this.label1);
	        this.Controls.Add(this.ReverseSelectButton);
	        this.Controls.Add(this.groupBox2);
	        this.Controls.Add(this.ExportButton);
	        this.Controls.Add(this.groupBox1);
	        this.Location = new System.Drawing.Point(15, 15);
	        this.Name = "ControlForm";
	        this.Load += new System.EventHandler(this.ControlForm_Load_1);
	        ((System.ComponentModel.ISupportInitialize) (this.fileSystemWatcher1)).EndInit();
	        this.groupBox1.ResumeLayout(false);
	        this.groupBox1.PerformLayout();
	        this.groupBox2.ResumeLayout(false);
	        this.groupBox2.PerformLayout();
	        this.ResumeLayout(false);
        }

        private System.Windows.Forms.TextBox cl;

        private System.Windows.Forms.TextBox ClientJsonPath;

        private System.Windows.Forms.TextBox ClientJsonBox;

        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Button button4;

        private System.Windows.Forms.CheckBox ClientJsonCheckBox;

        private System.Windows.Forms.CheckBox clientJson;

        private System.Windows.Forms.CheckBox checkBox1;

        private System.Windows.Forms.Label AutoCodeNamespace;
        private System.Windows.Forms.CheckBox BinaryCheckBox;
        private System.Windows.Forms.TextBox BinaryOutPath;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.TextBox CsEnumCodeOutputPath;
        private System.Windows.Forms.TextBox CsNamespaceEdit;
        private System.Windows.Forms.TextBox CsTableCodeOutputPath;
        private System.Windows.Forms.Label EnumLabel;
        private System.Windows.Forms.TextBox ExcelFilterTextBox;
        private System.Windows.Forms.Label ExcelPathText;
        private System.Windows.Forms.Button ExportButton;
        private System.Windows.Forms.CheckBox ExportEnumCodeCheckbox;
        private System.Windows.Forms.CheckBox ExportTableCodeCheckBox;
        private System.Windows.Forms.Button ExportTestCodeButton;
        private System.IO.FileSystemWatcher fileSystemWatcher1;
        private System.Windows.Forms.Button FilterExcelButton;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.CheckBox JsonCheckBox;
        private System.Windows.Forms.TextBox JsonOutPath;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.RichTextBox LogOutputText;
        private System.Windows.Forms.Button OpenBinaryOutputPath;
        private System.Windows.Forms.Button OpenEnumPathButton;
        private System.Windows.Forms.Button OpenExcelDataPath;
        private System.Windows.Forms.Button ResetAllButton;
        private System.Windows.Forms.Button ReverseSelectButton;
        private System.Windows.Forms.Button SelectAllbutton;
        private System.Windows.Forms.CheckedListBox TableCheckBoxList;

        #endregion
    }
}

