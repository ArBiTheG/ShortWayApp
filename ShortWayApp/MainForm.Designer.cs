
namespace ShortWayApp
{
    partial class MainForm
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.button1 = new System.Windows.Forms.Button();
            this.starterLabel = new System.Windows.Forms.Label();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.endingLabel = new System.Windows.Forms.Label();
            this.starterComboBox = new System.Windows.Forms.ComboBox();
            this.endingComboBox = new System.Windows.Forms.ComboBox();
            this.button2 = new System.Windows.Forms.Button();
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.shortWayControl1 = new ShortWayApp.ShortWayControl.ShortWayControl();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(371, 415);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 1;
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // starterLabel
            // 
            this.starterLabel.AutoSize = true;
            this.starterLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.starterLabel.Location = new System.Drawing.Point(3, 0);
            this.starterLabel.Name = "starterLabel";
            this.starterLabel.Size = new System.Drawing.Size(201, 31);
            this.starterLabel.TabIndex = 2;
            this.starterLabel.Text = "Начальная точка";
            this.starterLabel.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            this.starterLabel.Click += new System.EventHandler(this.label1_Click);
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Controls.Add(this.button2, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.starterComboBox, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.starterLabel, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.endingLabel, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.endingComboBox, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.richTextBox1, 0, 3);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(374, 12);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 4;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 200F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(414, 294);
            this.tableLayoutPanel1.TabIndex = 3;
            // 
            // endingLabel
            // 
            this.endingLabel.AutoSize = true;
            this.endingLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.endingLabel.Location = new System.Drawing.Point(210, 0);
            this.endingLabel.Name = "endingLabel";
            this.endingLabel.Size = new System.Drawing.Size(201, 31);
            this.endingLabel.TabIndex = 3;
            this.endingLabel.Text = "Конечная точка";
            this.endingLabel.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // starterComboBox
            // 
            this.starterComboBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.starterComboBox.FormattingEnabled = true;
            this.starterComboBox.Location = new System.Drawing.Point(3, 34);
            this.starterComboBox.Name = "starterComboBox";
            this.starterComboBox.Size = new System.Drawing.Size(201, 21);
            this.starterComboBox.TabIndex = 4;
            // 
            // endingComboBox
            // 
            this.endingComboBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.endingComboBox.FormattingEnabled = true;
            this.endingComboBox.Location = new System.Drawing.Point(210, 34);
            this.endingComboBox.Name = "endingComboBox";
            this.endingComboBox.Size = new System.Drawing.Size(201, 21);
            this.endingComboBox.TabIndex = 5;
            // 
            // button2
            // 
            this.button2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.button2.Location = new System.Drawing.Point(3, 65);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(201, 25);
            this.button2.TabIndex = 4;
            this.button2.Text = "Построить маршрут";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // richTextBox1
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.richTextBox1, 2);
            this.richTextBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.richTextBox1.Enabled = false;
            this.richTextBox1.Location = new System.Drawing.Point(3, 96);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.Size = new System.Drawing.Size(408, 195);
            this.richTextBox1.TabIndex = 6;
            this.richTextBox1.Text = "";
            // 
            // shortWayControl1
            // 
            this.shortWayControl1.Location = new System.Drawing.Point(12, 12);
            this.shortWayControl1.Name = "shortWayControl1";
            this.shortWayControl1.Size = new System.Drawing.Size(353, 426);
            this.shortWayControl1.TabIndex = 0;
            this.shortWayControl1.ZoomCam = 1F;
            this.shortWayControl1.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.shortWayControl1_MouseDoubleClick);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.shortWayControl1);
            this.Name = "MainForm";
            this.Text = "MainForm";
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private ShortWayControl.ShortWayControl shortWayControl1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label starterLabel;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.ComboBox starterComboBox;
        private System.Windows.Forms.Label endingLabel;
        private System.Windows.Forms.ComboBox endingComboBox;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.RichTextBox richTextBox1;
    }
}

