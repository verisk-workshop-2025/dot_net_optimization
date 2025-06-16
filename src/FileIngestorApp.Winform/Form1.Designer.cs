namespace FileIngestorApp.Winform
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            tabControlProcessor = new TabControl();
            tabPageFileProcessor = new TabPage();
            button2 = new Button();
            label2 = new Label();
            textBox1 = new TextBox();
            tabPageFileCreator = new TabPage();
            buttonCreateFile = new Button();
            textBoxFilePath = new TextBox();
            label1 = new Label();
            numericUpDownNumberOfLines = new NumericUpDown();
            labelLineCount = new Label();
            tabControlProcessor.SuspendLayout();
            tabPageFileProcessor.SuspendLayout();
            tabPageFileCreator.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)numericUpDownNumberOfLines).BeginInit();
            SuspendLayout();
            // 
            // tabControlProcessor
            // 
            tabControlProcessor.Controls.Add(tabPageFileProcessor);
            tabControlProcessor.Controls.Add(tabPageFileCreator);
            tabControlProcessor.Dock = DockStyle.Fill;
            tabControlProcessor.Location = new Point(0, 0);
            tabControlProcessor.Name = "tabControlProcessor";
            tabControlProcessor.SelectedIndex = 0;
            tabControlProcessor.Size = new Size(355, 166);
            tabControlProcessor.TabIndex = 0;
            // 
            // tabPageFileProcessor
            // 
            tabPageFileProcessor.Controls.Add(button2);
            tabPageFileProcessor.Controls.Add(label2);
            tabPageFileProcessor.Controls.Add(textBox1);
            tabPageFileProcessor.Location = new Point(4, 29);
            tabPageFileProcessor.Name = "tabPageFileProcessor";
            tabPageFileProcessor.Padding = new Padding(3);
            tabPageFileProcessor.Size = new Size(347, 133);
            tabPageFileProcessor.TabIndex = 0;
            tabPageFileProcessor.Text = "File processor";
            tabPageFileProcessor.UseVisualStyleBackColor = true;
            // 
            // button2
            // 
            button2.Location = new Point(8, 39);
            button2.Name = "button2";
            button2.Size = new Size(331, 29);
            button2.TabIndex = 2;
            button2.Text = "Process file";
            button2.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(8, 9);
            label2.Name = "label2";
            label2.Size = new Size(141, 20);
            label2.TabIndex = 1;
            label2.Text = "File path to process:";
            // 
            // textBox1
            // 
            textBox1.Location = new Point(155, 6);
            textBox1.Name = "textBox1";
            textBox1.Size = new Size(184, 27);
            textBox1.TabIndex = 0;
            // 
            // tabPageFileCreator
            // 
            tabPageFileCreator.Controls.Add(buttonCreateFile);
            tabPageFileCreator.Controls.Add(textBoxFilePath);
            tabPageFileCreator.Controls.Add(label1);
            tabPageFileCreator.Controls.Add(numericUpDownNumberOfLines);
            tabPageFileCreator.Controls.Add(labelLineCount);
            tabPageFileCreator.Location = new Point(4, 29);
            tabPageFileCreator.Name = "tabPageFileCreator";
            tabPageFileCreator.Padding = new Padding(3);
            tabPageFileCreator.Size = new Size(347, 133);
            tabPageFileCreator.TabIndex = 1;
            tabPageFileCreator.Text = "File creator";
            tabPageFileCreator.UseVisualStyleBackColor = true;
            // 
            // buttonCreateFile
            // 
            buttonCreateFile.Location = new Point(8, 72);
            buttonCreateFile.Name = "buttonCreateFile";
            buttonCreateFile.Size = new Size(313, 29);
            buttonCreateFile.TabIndex = 4;
            buttonCreateFile.Text = "Create";
            buttonCreateFile.UseVisualStyleBackColor = true;
            buttonCreateFile.Click += buttonCreateFile_Click;
            // 
            // textBoxFilePath
            // 
            textBoxFilePath.Location = new Point(132, 39);
            textBoxFilePath.Name = "textBoxFilePath";
            textBoxFilePath.Size = new Size(189, 27);
            textBoxFilePath.TabIndex = 3;
            textBoxFilePath.Text = "C:\\TestFiles\\large.jl";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(8, 42);
            label1.Name = "label1";
            label1.Size = new Size(69, 20);
            label1.TabIndex = 2;
            label1.Text = "File path:";
            // 
            // numericUpDownNumberOfLines
            // 
            numericUpDownNumberOfLines.Location = new Point(132, 6);
            numericUpDownNumberOfLines.Maximum = new decimal(new int[] { 1000000000, 0, 0, 0 });
            numericUpDownNumberOfLines.Name = "numericUpDownNumberOfLines";
            numericUpDownNumberOfLines.Size = new Size(189, 27);
            numericUpDownNumberOfLines.TabIndex = 1;
            numericUpDownNumberOfLines.Value = new decimal(new int[] { 1000000, 0, 0, 0 });
            // 
            // labelLineCount
            // 
            labelLineCount.AutoSize = true;
            labelLineCount.Location = new Point(8, 8);
            labelLineCount.Name = "labelLineCount";
            labelLineCount.Size = new Size(118, 20);
            labelLineCount.TabIndex = 0;
            labelLineCount.Text = "Number of lines:";
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(355, 166);
            Controls.Add(tabControlProcessor);
            Name = "Form1";
            Text = "Form1";
            tabControlProcessor.ResumeLayout(false);
            tabPageFileProcessor.ResumeLayout(false);
            tabPageFileProcessor.PerformLayout();
            tabPageFileCreator.ResumeLayout(false);
            tabPageFileCreator.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)numericUpDownNumberOfLines).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private TabControl tabControlProcessor;
        private TabPage tabPageFileProcessor;
        private TabPage tabPageFileCreator;
        private Button buttonCreateFile;
        private TextBox textBoxFilePath;
        private Label label1;
        private NumericUpDown numericUpDownNumberOfLines;
        private Label labelLineCount;
        private Button button2;
        private Label label2;
        private TextBox textBox1;
    }
}
